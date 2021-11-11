using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;
using OpenXmlAttribute = DocumentFormat.OpenXml.OpenXmlAttribute;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Readers;

public class WorksheetElementReaderFacts
{
    private readonly AutoMocker _mocker = new();

    private WorksheetElementReader CreateSystemUnderTest()
        => _mocker.CreateInstance<WorksheetElementReader>();

    public class TheReadToRowMethod : WorksheetElementReaderFacts
    {
        [Fact]
        public void ReturnsTrueWhenTheSpecifiedRowIsRead()
        {
            // Arrange
            var rowNumber = 2U;

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Row),
                    attributes: new OpenXmlAttribute(default, ElementAttributeKind.RowNumber.LocalName, default, "1")),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Row),
                    attributes: new OpenXmlAttribute(
                        default,
                        ElementAttributeKind.RowNumber.LocalName,
                        default,
                        rowNumber.ToString())),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Row),
                    attributes: new OpenXmlAttribute(default, ElementAttributeKind.RowNumber.LocalName, default, "3")),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasRow = sut.ReadToRow(rowNumber);

            // Assert
            Assert.True(hasRow);
        }

        [Fact]
        public void ReturnsFalseWhenTheSpecifiedRowCannotBeFound()
        {
            // Arrange
            var rowNumber = 3U;

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Row),
                    attributes: new OpenXmlAttribute(default, ElementAttributeKind.RowNumber.LocalName, default, "1")),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Row),
                    attributes: new OpenXmlAttribute(default, ElementAttributeKind.RowNumber.LocalName, default, "2")),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasRow = sut.ReadToRow(rowNumber);

            // Assert
            Assert.False(hasRow);
        }
    }

    public class TheReadNextRowMethod : WorksheetElementReaderFacts
    {
        [Fact]
        public void ReturnsTrueWhenTheNextRowIsRead()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasRow = sut.ReadNextRow();

            // Assert
            Assert.True(hasRow);
        }

        [Fact]
        public void ReturnsFalseWhenTheEndOfTheSheetDataIsReached()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.SheetData)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasRow = sut.ReadNextRow();

            // Assert
            Assert.False(hasRow);
        }
    }

    public class TheGetRowNumberMethod : WorksheetElementReaderFacts
    {
        [Fact]
        public void ReturnsRowNumberFromTheCurrentRowElement()
        {
            // Arrange
            var expectedRowNumber = 1U;

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Row),
                    attributes: new OpenXmlAttribute(
                        default,
                        ElementAttributeKind.RowNumber.LocalName,
                        default,
                        expectedRowNumber.ToString())),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var actualRowNumber = sut.GetRowNumber();

            // Assert
            Assert.Equal(expectedRowNumber, actualRowNumber);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenTheCurrentElementIsNotRowElement()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Cell)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.GetRowNumber());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    public class TheReadNextCellMethod : WorksheetElementReaderFacts
    {
        [Fact]
        public void ReturnsTrueWhenTheNextCellIsRead()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasCell = sut.ReadNextCell();

            // Assert
            Assert.True(hasCell);
        }

        [Fact]
        public void ReturnsFalseWhenTheEndOfTheRowIsReached()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasCell = sut.ReadNextCell();

            // Assert
            Assert.False(hasCell);
        }
    }

    public class TheGetCellLocationMethod : WorksheetElementReaderFacts
    {
        [Fact]
        public void ReturnsTheCellLocationThatRepresentsTheCellReferenceFromTheCurrentCellElement()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Cell),
                    attributes: new OpenXmlAttribute(default, ElementAttributeKind.CellReference.LocalName, default, "A2")),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var cellLocation = sut.GetCellLocation();

            // Assert
            Assert.Equal(2U, cellLocation.RowNumber);
            Assert.Equal(1U, cellLocation.Column.Number);
            Assert.Equal("A", cellLocation.Column.Letter);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenTheCurrentElementIsNotCellElement()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.GetCellLocation());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    public class TheGetCellValueMethod : WorksheetElementReaderFacts
    {
        [Fact]
        public void ReturnsContentsOfChildTextElementWhenValueKindIsInlineString()
        {
            // Arrange
            var expectedCellValue = "foo";

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Cell),
                    attributes: new OpenXmlAttribute(
                        default,
                        ElementAttributeKind.CellValueType.LocalName,
                        default,
                        CellValueKind.InlineString.XmlName)),
                FakeOpenXmlElement.Create(typeof(OpenXml.Text), text: expectedCellValue),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.GetCellValue();

            // Assert
            Assert.Equal(expectedCellValue, actualCellValue);
        }

        [Fact]
        public void ReturnsContentsOfChildCellValueElementWhenValueKindIsNotSpecified()
        {
            // Arrange
            var expectedCellValue = "123";

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Cell)),
                FakeOpenXmlElement.Create(typeof(OpenXml.CellValue), text: expectedCellValue),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.GetCellValue();

            // Assert
            Assert.Equal(expectedCellValue, actualCellValue);
        }

        [Fact]
        public void ReturnsContentsOfChildCellValueElementWhenValueKindIsNotSharedStringOrInlineString()
        {
            // Arrange
            var expectedCellValue = "foo";

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Cell),
                    attributes: new OpenXmlAttribute(
                        default,
                        ElementAttributeKind.CellValueType.LocalName,
                        default,
                        CellValueKind.Boolean.XmlName)),
                FakeOpenXmlElement.Create(typeof(OpenXml.CellValue), text: expectedCellValue),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.GetCellValue();

            // Assert
            Assert.Equal(expectedCellValue, actualCellValue);
        }

        [Fact]
        public void ReturnsIndexedStringFromContentsOfChildCellValueElementWhenValueKindIsSharedString()
        {
            // Arrange
            var stringIndex = 1U;
            var expectedCellValue = "foo";

            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(
                    typeof(OpenXml.Cell),
                    attributes: new OpenXmlAttribute(
                        default,
                        ElementAttributeKind.CellValueType.LocalName,
                        default,
                        CellValueKind.SharedString.XmlName)),
                FakeOpenXmlElement.Create(typeof(OpenXml.CellValue), text: stringIndex.ToString()),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Cell)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            _mocker.GetMock<IStringIndexer>()
                .SetupGet(stringIndexer => stringIndexer[stringIndex])
                .Returns(expectedCellValue);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.GetCellValue();

            // Assert
            Assert.Equal(expectedCellValue, actualCellValue);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenTheCurrentElementIsNotCellElement()
        {
            // Arrange
            var readerElements = new[]
            {
                FakeOpenXmlElement.CreateStart(typeof(OpenXml.Row)),
                FakeOpenXmlElement.CreateEnd(typeof(OpenXml.Row)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.GetCellValue());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    public class TheDisposeMethod : WorksheetElementReaderFacts
    {
        [Fact]
        public void DisposesOpenXmlReader()
        {
            // Arrange
            var readerMock = _mocker.GetMock<IOpenXmlReaderWrapper>();
            var sut = CreateSystemUnderTest();

            // Act
            sut.Dispose();

            // Assert
            readerMock.Verify(reader => reader.Dispose());
        }
    }
}
