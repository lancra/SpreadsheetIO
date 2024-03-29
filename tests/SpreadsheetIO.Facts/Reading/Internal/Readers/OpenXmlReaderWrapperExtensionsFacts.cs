using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;
using OpenXmlAttribute = DocumentFormat.OpenXml.OpenXmlAttribute;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Readers;

public class OpenXmlReaderWrapperExtensionsFacts
{
    public class TheReadNextElementMethod : OpenXmlReaderWrapperExtensionsFacts
    {
        [Fact]
        public void ReturnsTrueWhenStartElementOfSpecifiedStartTypeIsFound()
        {
            // Arrange
            var expectedElement = FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row));
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Worksheet)),
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.SheetData)),
                expectedElement,
                FakeOpenXmlElement.Create(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.SheetData)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Worksheet)),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);

            // Act
            var isRead = sut.ReadNextElement(typeof(OpenXml.Row), typeof(OpenXml.SheetData));

            // Assert
            Assert.True(isRead);
            Assert.Equal(expectedElement.IsStartElement, sut.IsStartElement);
            Assert.Equal(expectedElement.IsEndElement, sut.IsEndElement);
            Assert.Equal(expectedElement.ElementType, sut.ElementType);
        }

        [Fact]
        public void ReturnsFalseWhenEndElementOfSpecifiedEndTypeIsFoundBeforeStartElementOfSpecifiedStartType()
        {
            // Arrange
            var expectedElement = FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row));
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                expectedElement,
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                FakeOpenXmlElement.Create(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);

            // Act
            var isRead = sut.ReadNextElement(typeof(OpenXml.Cell), typeof(OpenXml.Row));

            // Assert
            Assert.False(isRead);
            Assert.Equal(expectedElement.IsStartElement, sut.IsStartElement);
            Assert.Equal(expectedElement.IsEndElement, sut.IsEndElement);
            Assert.Equal(expectedElement.ElementType, sut.ElementType);
        }

        [Fact]
        public void ReturnsFalseIfReaderReachesEndOfDocument()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);

            // Act
            var isRead = sut.ReadNextElement(typeof(OpenXml.Cell), typeof(OpenXml.SheetData));

            // Assert
            Assert.False(isRead);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenReaderIsNull()
        {
            // Arrange
            var sut = default(IOpenXmlReaderWrapper);
            var startElementType = typeof(OpenXml.Cell);
            var endElementType = typeof(OpenXml.Row);

            // Act
            var exception = Record.Exception(() => sut!.ReadNextElement(startElementType, endElementType));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStartElementTypeIsNull()
        {
            // Arrange
            using var sut = new FakeOpenXmlReaderWrapper(Array.Empty<FakeOpenXmlElement>());
            var startElementType = default(Type);
            var endElementType = typeof(OpenXml.Row);

            // Act
            var exception = Record.Exception(() => sut.ReadNextElement(startElementType!, endElementType));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenEndElementTypeIsNull()
        {
            // Arrange
            using var sut = new FakeOpenXmlReaderWrapper(Array.Empty<FakeOpenXmlElement>());
            var startElementType = typeof(OpenXml.Cell);
            var endElementType = default(Type);

            // Act
            var exception = Record.Exception(() => sut.ReadNextElement(startElementType, endElementType!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheGetAttributeMethod : OpenXmlReaderWrapperExtensionsFacts
    {
        [Fact]
        public void ReturnsAttributeWhenContainedWithinCurrentElement()
        {
            // Arrange
            var kind = ElementAttributeKind.CellReference;

            var expectedAttribute = new OpenXmlAttribute(string.Empty, kind.LocalName, string.Empty, "foo");
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Cell),
                    string.Empty,
                    expectedAttribute,
                    new OpenXmlAttribute(string.Empty, ElementAttributeKind.CellValueType.LocalName, string.Empty, "bar")),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);
            sut.Read();

            // Act
            var actualAttribute = sut.GetAttribute(kind);

            // Assert
            Assert.Equal(expectedAttribute, actualAttribute);
        }

        [Fact]
        public void ReturnsDefaultAttributeWhenNotContainedWithinCurrentElement()
        {
            // Arrange
            var kind = ElementAttributeKind.CellReference;

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Cell),
                    string.Empty,
                    new OpenXmlAttribute(string.Empty, ElementAttributeKind.CellValueType.LocalName, string.Empty, "bar")),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);
            sut.Read();

            // Act
            var attribute = sut.GetAttribute(kind);

            // Assert
            Assert.Equal(default, attribute);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenReaderIsNull()
        {
            // Arrange
            var sut = default(IOpenXmlReaderWrapper);
            var kind = ElementAttributeKind.CellReference;

            // Act
            var exception = Record.Exception(() => sut!.GetAttribute(kind));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenKindIsNull()
        {
            // Arrange
            using var sut = new FakeOpenXmlReaderWrapper(Array.Empty<FakeOpenXmlElement>());
            var kind = default(ElementAttributeKind);

            // Act
            var exception = Record.Exception(() => sut.GetAttribute(kind!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheIsStartElementOfTypeMethod : OpenXmlReaderWrapperExtensionsFacts
    {
        [Fact]
        public void ReturnsTrueWhenCurrentElementMatchesSpecifiedTypeAndIsStartElement()
        {
            // Arrange
            var elementType = typeof(OpenXml.Cell);
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(elementType),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);
            sut.Read();

            // Act
            var isStartElementOfType = sut.IsStartElementOfType(elementType);

            // Assert
            Assert.True(isStartElementOfType);
        }

        [Fact]
        public void ReturnsFalseWhenCurrentElementDoesNotMatchSpecifiedType()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);
            sut.Read();

            // Act
            var isStartElementOfType = sut.IsStartElementOfType(typeof(OpenXml.Cell));

            // Assert
            Assert.False(isStartElementOfType);
        }

        [Theory]
        [InlineData(typeof(OpenXml.Row))]
        [InlineData(typeof(OpenXml.Cell))]
        public void ReturnsFalseWhenCurrentElementIsNotStartElement(Type elementType)
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateEnd(elementType),
            };

            using var sut = new FakeOpenXmlReaderWrapper(readerElements);
            sut.Read();

            // Act
            var isStartElementOfType = sut.IsStartElementOfType(typeof(OpenXml.Cell));

            // Assert
            Assert.False(isStartElementOfType);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenReaderIsNull()
        {
            // Arrange
            var sut = default(IOpenXmlReaderWrapper);
            var elementType = typeof(OpenXml.Cell);

            // Act
            var exception = Record.Exception(() => sut!.IsStartElementOfType(elementType));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenElementTypeIsNull()
        {
            // Arrange
            using var sut = new FakeOpenXmlReaderWrapper(Array.Empty<FakeOpenXmlElement>());
            var elementType = default(Type);

            // Act
            var exception = Record.Exception(() => sut.IsStartElementOfType(elementType!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
