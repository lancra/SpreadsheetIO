using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Moq;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal.Generators
{
    public class SharedStringTableGeneratorFacts
    {
        private readonly AutoMocker _mocker = new();

        private SharedStringTableGenerator CreateSystemUnderTest()
            => _mocker.CreateInstance<SharedStringTableGenerator>();

        public class TheGenerateMethod : SharedStringTableGeneratorFacts
        {
            [Fact]
            public void WritesIndexedStrings()
            {
                // Arrange
                var writerMock = _mocker.GetMock<IOpenXmlWriterWrapper>();

                var sharedStringTablePartMock = _mocker.GetMock<ISharedStringTablePartWrapper>();
                sharedStringTablePartMock.Setup(sharedStringTablePart => sharedStringTablePart.CreateWriter())
                    .Returns(writerMock.Object);

                var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
                spreadsheetDocumentMock.Setup(spreadsheetDocument => spreadsheetDocument.AddSharedStringTablePart())
                    .Returns(sharedStringTablePartMock.Object);

                var firstString = "foo";
                var secondString = "bar";
                var thirdString = "baz";
                var strings = new[] { firstString, secondString, thirdString, };
                _mocker.GetMock<IStringIndexer>()
                    .SetupGet(stringIndexer => stringIndexer.Resources)
                    .Returns(strings);

                var sut = CreateSystemUnderTest();

                // Act
                sut.Generate(spreadsheetDocumentMock.Object);

                // Assert
                writerMock.Verify(writer => writer.WriteElement(
                    It.Is<OpenXml.SharedStringItem>(sharedStringItem => sharedStringItem.Text.Text == firstString)));
                writerMock.Verify(writer => writer.WriteElement(
                    It.Is<OpenXml.SharedStringItem>(sharedStringItem => sharedStringItem.Text.Text == secondString)));
                writerMock.Verify(writer => writer.WriteElement(
                    It.Is<OpenXml.SharedStringItem>(sharedStringItem => sharedStringItem.Text.Text == thirdString)));
            }

            [Fact]
            public void WritesSharedStringTableElementForAtLeastOneIndexedString()
            {
                // Arrange
                var writerMock = _mocker.GetMock<IOpenXmlWriterWrapper>();

                var sharedStringTablePartMock = _mocker.GetMock<ISharedStringTablePartWrapper>();
                sharedStringTablePartMock.Setup(sharedStringTablePart => sharedStringTablePart.CreateWriter())
                    .Returns(writerMock.Object);

                var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
                spreadsheetDocumentMock.Setup(spreadsheetDocument => spreadsheetDocument.AddSharedStringTablePart())
                    .Returns(sharedStringTablePartMock.Object);

                var indexedString = "foo";
                _mocker.GetMock<IStringIndexer>()
                    .SetupGet(stringIndexer => stringIndexer.Resources)
                    .Returns(new[] { indexedString, });

                var sut = CreateSystemUnderTest();

                // Act
                sut.Generate(spreadsheetDocumentMock.Object);

                // Assert
                writerMock.Verify(writer => writer.WriteStartElement(It.IsAny<OpenXml.SharedStringTable>()));
                writerMock.Verify(writer => writer.WriteEndElement());
            }

            [Fact]
            public void DoesNotWriteSharedStringTableElementWhenNoStringsIndexed()
            {
                // Arrange
                var writerMock = _mocker.GetMock<IOpenXmlWriterWrapper>();

                var sharedStringTablePartMock = _mocker.GetMock<ISharedStringTablePartWrapper>();
                sharedStringTablePartMock.Setup(sharedStringTablePart => sharedStringTablePart.CreateWriter())
                    .Returns(writerMock.Object);

                var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
                spreadsheetDocumentMock.Setup(spreadsheetDocument => spreadsheetDocument.AddSharedStringTablePart())
                    .Returns(sharedStringTablePartMock.Object);

                _mocker.GetMock<IStringIndexer>()
                    .SetupGet(stringIndexer => stringIndexer.Resources)
                    .Returns(Array.Empty<string>());

                var sut = CreateSystemUnderTest();

                // Act
                sut.Generate(spreadsheetDocumentMock.Object);

                // Assert
                writerMock.Verify(writer => writer.WriteStartElement(It.IsAny<OpenXml.SharedStringTable>()), Times.Never);
                writerMock.Verify(writer => writer.WriteEndElement(), Times.Never);
            }
        }
    }
}
