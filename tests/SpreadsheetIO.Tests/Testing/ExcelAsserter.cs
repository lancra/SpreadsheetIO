using System.IO.Packaging;
using System.Xml.Linq;
using Ardalis.SmartEnum;
using Xunit;
using Xunit.Abstractions;

namespace LanceC.SpreadsheetIO.Tests.Testing
{
    public class ExcelAsserter
    {
        private readonly ITestOutputHelper _output;

        public ExcelAsserter(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Equal(Uri expected, Uri actual)
        {
            using var expectedPackage = OpenPackage(expected);
            using var actualPackage = OpenPackage(actual);

            var expectedPackageParts = GetPackageParts(expectedPackage);
            var actualPackageParts = GetPackageParts(actualPackage);

            _output.WriteLine(
                "Checking overall package part count: Expected={0}, Actual={1}...",
                expectedPackageParts.Count,
                actualPackageParts.Count);
            Assert.Equal(expectedPackageParts.Count, actualPackageParts.Count);

            foreach (var fileKind in SmartEnum<ExcelFileKind>.List)
            {
                var hasExpectedFileKindPackageParts = expectedPackageParts.TryGetValue(fileKind, out var expectedFileKindPackageParts);
                var hasActualFileKindPackageParts = actualPackageParts.TryGetValue(fileKind, out var actualFileKindPackageParts);
                _output.WriteLine(
                    "Checking package part existence for {0}: Expected={1}, Actual={2}...",
                    fileKind,
                    hasExpectedFileKindPackageParts,
                    hasActualFileKindPackageParts);
                Assert.Equal(hasExpectedFileKindPackageParts, hasActualFileKindPackageParts);

                if (!hasExpectedFileKindPackageParts)
                {
                    continue;
                }

                _output.WriteLine(
                    "Checking package part count for {0}: Expected={1}, Actual={2}...",
                    fileKind,
                    expectedFileKindPackageParts!.Count,
                    actualFileKindPackageParts!.Count);
                Assert.Equal(expectedFileKindPackageParts.Count, actualFileKindPackageParts.Count);

                foreach (var expectedPackagePart in expectedFileKindPackageParts)
                {
                    var actualPackagePart = actualFileKindPackageParts.SingleOrDefault(p => p.Uri.Equals(expectedPackagePart.Uri));

                    _output.WriteLine("Checking existence of {0} package part...", expectedPackagePart.Uri.OriginalString);
                    Assert.NotNull(actualPackagePart);

                    var expectedDocument = XDocument.Load(
                        expectedPackagePart.GetStream(FileMode.Open, FileAccess.Read),
                        LoadOptions.None);
                    var actualDocument = XDocument.Load(
                        actualPackagePart!.GetStream(FileMode.Open, FileAccess.Read),
                        LoadOptions.None);

                    DocumentsEqual(expectedDocument, actualDocument);
                }
            }
        }

        private static Package OpenPackage(Uri uri)
            => Package.Open(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read);

        private static IDictionary<ExcelFileKind, IList<PackagePart>> GetPackageParts(Package package)
        {
            var parts = new Dictionary<ExcelFileKind, IList<PackagePart>>();
            foreach (var part in package.GetParts())
            {
                if (ExcelFileKind.TryFromUri(part.Uri, out var fileKind) &&
                    part.Uri.OriginalString.EndsWith(".xml"))
                {
                    parts.TryAdd(fileKind!, new List<PackagePart>());
                    parts[fileKind!].Add(part);
                }
            }

            return parts;
        }

        private static IReadOnlyList<ExcelElement> GetElements(XDocument document)
        {
            var descendants = document.Descendants();
            var elements = new List<ExcelElement>();
            foreach (var descendant in descendants)
            {
                if (ExcelElementKind.TryFromLocalName(descendant.Name.LocalName, out var elementKind) &&
                    (elementKind!.IsRoot || ExcelElementKind.TryFromLocalName(descendant.Parent!.Name.LocalName, out var _)))
                {
                    elements.Add(new ExcelElement(elementKind, descendant));
                }
            }

            return elements;
        }

        private static IReadOnlyList<ExcelAttribute> GetAttributes(ExcelElement element)
        {
            var xAttributes = element.XElement.Attributes();
            var attributes = new List<ExcelAttribute>();
            foreach (var xAttribute in xAttributes)
            {
                if (ExcelAttributeKind.TryFromLocalName(xAttribute.Name.LocalName, out var attributeKind) &&
                    element.Kind.AttributeKinds.Contains(attributeKind))
                {
                    attributes.Add(new ExcelAttribute(attributeKind!, xAttribute));
                }
            }

            return attributes;
        }

        private void DocumentsEqual(XDocument expected, XDocument actual)
        {
            var expectedElements = GetElements(expected);
            var actualElements = GetElements(actual);

            _output.WriteLine("Checking element count: Expected={0}, Actual={1}...", expectedElements.Count, actualElements.Count);
            Assert.Equal(expectedElements.Count, actualElements.Count);

            for (var i = 0; i < expectedElements.Count; i++)
            {
                var expectedElement = expectedElements[i];
                var actualElement = actualElements[i];

                _output.WriteLine("Checking element kind: Expected={0}, Actual={1}...", expectedElement.Kind, actualElement.Kind);
                Assert.Equal(expectedElement.Kind, actualElement.Kind);

                _output.WriteLine(
                    "Checking element value: Expected='{0}', Actual='{1}'...",
                    expectedElement.XElement.Value,
                    actualElement.XElement.Value);
                Assert.Equal(expectedElement.XElement.Value, actualElement.XElement.Value);

                var expectedAttributes = GetAttributes(expectedElement);
                var actualAttributes = GetAttributes(actualElement);

                _output.WriteLine(
                    "Checking element attribute count: Expected={0}, Actual={1}...",
                    expectedAttributes.Count,
                    actualAttributes.Count);
                Assert.Equal(expectedAttributes.Count, actualAttributes.Count);

                for (var j = 0; j < expectedAttributes.Count; j++)
                {
                    var expectedAttribute = expectedAttributes[j];
                    var actualAttribute = actualAttributes[j];

                    _output.WriteLine(
                        "Checking element attribute kind: Expected={0}, Actual={1}...",
                        expectedAttribute.Kind,
                        actualAttribute.Kind);
                    Assert.Equal(expectedAttribute.Kind, actualAttribute.Kind);

                    _output.WriteLine(
                        "Checking element attribute value: Expected='{0}', Actual='{1}'...",
                        expectedAttribute.XAttribute.Value,
                        actualAttribute.XAttribute.Value);
                    Assert.Equal(expectedAttribute.XAttribute.Value, actualAttribute.XAttribute.Value);
                }
            }
        }
    }
}
