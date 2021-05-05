using System;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal;
using Moq;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal
{
    public class WritingSpreadsheetPageFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private static WritingCell DefaultCell
            => new WritingCell(new WritingCellValue(default(string?)));

        private WritingSpreadsheetPage CreateSystemUnderTest()
        {
            var writerMock = _mocker.GetMock<IOpenXmlWriterWrapper>();
            writerMock.Setup(writer => writer.WriteElement(It.IsAny<OpenXml.OpenXmlElement>()))
                .Returns(writerMock.Object);
            writerMock.Setup(writer => writer.WriteStartElement(It.IsAny<OpenXml.OpenXmlElement>()))
                .Returns(writerMock.Object);
            writerMock.Setup(writer => writer.WriteEndElement())
                .Returns(writerMock.Object);

            _mocker.GetMock<IWorksheetPartWrapper>()
                .Setup(worksheetPart => worksheetPart.CreateWriter())
                .Returns(writerMock.Object);

            return _mocker.CreateInstance<WritingSpreadsheetPage>();
        }

        public class TheConstructor : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void WritesWorksheetStart()
            {
                // Act
                CreateSystemUnderTest();

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteStartElement(It.IsAny<OpenXml.Spreadsheet.Worksheet>()));
            }

            [Fact]
            public void WritesSheetDataStart()
            {
                // Act
                CreateSystemUnderTest();

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteStartElement(It.IsAny<OpenXml.Spreadsheet.SheetData>()));
            }
        }

        public class TheAddCellMethodWithCellParameter : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void WritesCellWithCellReference()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(DefaultCell);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.CellReference == "A1")));
            }

            [Fact]
            public void WritesCellWithStyleIndexWhenStyleIsProvided()
            {
                // Arrange
                var style = BuiltInExcelStyle.Normal;
                var styleIndex = 10U;
                var cell = new WritingCell(
                    new WritingCellValue(default(string?)),
                    new WritingCellStyle(style));

                _mocker.GetMock<IStyleIndexer>()
                    .SetupGet(styleIndexer => styleIndexer[style.IndexerKey])
                    .Returns(new IndexedResource<Style>(style.Style, styleIndex));

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(cell);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.StyleIndex == styleIndex)));
            }

            [Fact]
            public void WritesCellWithModificationsFromCellValue()
            {
                // Arrange
                var cell = new WritingCell(new WritingCellValue(1));
                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(cell);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.CellValue.Text == "1")));
            }

            [Fact]
            public void WritesCellWithStringIndexWhenDataTypeIsSharedString()
            {
                // Arrange
                var value = "foo";
                var cell = new WritingCell(new WritingCellValue(value));
                var stringIndex = 10U;

                _mocker.GetMock<IStringIndexer>()
                    .Setup(stringIndexer => stringIndexer.Add(value))
                    .Returns(stringIndex);

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(cell);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.CellValue.Text == stringIndex.ToString())));
            }

            [Fact]
            public void WritesRowStartWhenCellIsFirstOnTheRow()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AdvanceRow();

                // Act
                sut.AddCell(DefaultCell);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteStartElement(
                        It.Is<OpenXml.Spreadsheet.Row>(row => row.RowIndex == 2U)));
            }

            [Fact]
            public void AdvancesCurrentColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(DefaultCell);

                // Assert
                Assert.Equal(2U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AddCell(DefaultCell));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAddCellMethodWithValueParameter : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void WritesCellWithCellReference()
            {
                // Arrange
                var value = default(string?);
                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(value!);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.CellReference == "A1")));
            }

            [Fact]
            public void WritesCellWithStringIndexWhenDataTypeIsSharedString()
            {
                // Arrange
                var value = "foo";
                var stringIndex = 10U;

                _mocker.GetMock<IStringIndexer>()
                    .Setup(stringIndexer => stringIndexer.Add(value))
                    .Returns(stringIndex);

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(value);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.CellValue.Text == stringIndex.ToString())));
            }

            [Fact]
            public void WritesRowStartWhenCellIsFirstOnTheRow()
            {
                // Arrange
                var value = default(string?);
                var sut = CreateSystemUnderTest();
                sut.AdvanceRow();

                // Act
                sut.AddCell(value!);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteStartElement(
                        It.Is<OpenXml.Spreadsheet.Row>(row => row.RowIndex == 2U)));
            }

            [Fact]
            public void AdvancesCurrentColumnNumber()
            {
                // Arrange
                var value = default(string?);
                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(value!);

                // Assert
                Assert.Equal(2U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var value = default(string?);
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AddCell(value!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAddCellMethodWithValueAndStyleNameParameters : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void WritesCellWithCellReference()
            {
                // Arrange
                var value = default(string?);
                var styleName = "Style";

                _mocker.GetMock<IStyleIndexer>()
                    .SetupGet(styleIndexer => styleIndexer[
                        It.Is<IndexerKey>(styleKey =>
                            styleKey.Name == styleName &&
                            styleKey.Kind == IndexerKeyKind.Custom)])
                    .Returns(new IndexedResource<Style>(Style.Default, 10U));

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(value!, styleName);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.CellReference == "A1")));
            }

            [Fact]
            public void WritesCellWithStyleIndexWhenStyleIsProvided()
            {
                // Arrange
                var value = default(string?);
                var styleName = "Style";
                var styleIndex = 10U;

                _mocker.GetMock<IStyleIndexer>()
                    .SetupGet(styleIndexer => styleIndexer[
                        It.Is<IndexerKey>(styleKey =>
                            styleKey.Name == styleName &&
                            styleKey.Kind == IndexerKeyKind.Custom)])
                    .Returns(new IndexedResource<Style>(Style.Default, styleIndex));

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(value!, styleName);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.StyleIndex == styleIndex)));
            }

            [Fact]
            public void WritesCellWithStringIndexWhenDataTypeIsSharedString()
            {
                // Arrange
                var value = "foo";
                var styleName = "Style";
                var stringIndex = 10U;

                _mocker.GetMock<IStyleIndexer>()
                    .SetupGet(styleIndexer => styleIndexer[
                        It.Is<IndexerKey>(styleKey =>
                            styleKey.Name == styleName &&
                            styleKey.Kind == IndexerKeyKind.Custom)])
                    .Returns(new IndexedResource<Style>(Style.Default, 10U));

                _mocker.GetMock<IStringIndexer>()
                    .Setup(stringIndexer => stringIndexer.Add(value))
                    .Returns(stringIndex);

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(value, styleName);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteElement(
                        It.Is<OpenXml.Spreadsheet.Cell>(openXmlCell => openXmlCell.CellValue.Text == stringIndex.ToString())));
            }

            [Fact]
            public void WritesRowStartWhenCellIsFirstOnTheRow()
            {
                // Arrange
                var value = default(string?);
                var styleName = "Style";

                _mocker.GetMock<IStyleIndexer>()
                    .SetupGet(styleIndexer => styleIndexer[
                        It.Is<IndexerKey>(styleKey =>
                            styleKey.Name == styleName &&
                            styleKey.Kind == IndexerKeyKind.Custom)])
                    .Returns(new IndexedResource<Style>(Style.Default, 10U));

                var sut = CreateSystemUnderTest();
                sut.AdvanceRow();

                // Act
                sut.AddCell(value!, styleName);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteStartElement(
                        It.Is<OpenXml.Spreadsheet.Row>(row => row.RowIndex == 2U)));
            }

            [Fact]
            public void AdvancesCurrentColumnNumber()
            {
                // Arrange
                var value = default(string?);
                var styleName = "Style";

                _mocker.GetMock<IStyleIndexer>()
                    .SetupGet(styleIndexer => styleIndexer[
                        It.Is<IndexerKey>(styleKey =>
                            styleKey.Name == styleName &&
                            styleKey.Kind == IndexerKeyKind.Custom)])
                    .Returns(new IndexedResource<Style>(Style.Default, 10U));

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddCell(value!, styleName);

                // Assert
                Assert.Equal(2U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var value = default(string?);
                var styleName = "Style";

                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AddCell(value!, styleName));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAdvanceRowMethod : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void IncrementsCurrentRowNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.AdvanceRow();

                // Assert
                Assert.Equal(2U, sut.CurrentRowNumber);
            }

            [Fact]
            public void ResetsCurrentColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AdvanceColumn();

                // Act
                sut.AdvanceRow();

                // Assert
                Assert.Equal(1U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void WritesRowEndWhenCurrentRowContainsAtLeastOneCell()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AddCell(DefaultCell);

                // Act
                sut.AdvanceRow();

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteEndElement());
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AdvanceRow());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAdvanceRowsMethod : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void IncrementsCurrentRowNumberByCount()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.AdvanceRows(2U);

                // Assert
                Assert.Equal(3U, sut.CurrentRowNumber);
            }

            [Fact]
            public void ResetsCurrentColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AdvanceColumn();

                // Act
                sut.AdvanceRows(2U);

                // Assert
                Assert.Equal(1U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void WritesRowEndWhenCurrentRowContainsAtLeastOneCell()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AddCell(DefaultCell);

                // Act
                sut.AdvanceRows(2U);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteEndElement());
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenCountIsZero()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AdvanceRows(0U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AdvanceRows(2U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAdvanceToRowMethod : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void IncrementsCurrentRowNumberUntilEqualToProvidedRowNumber()
            {
                // Arrange
                var rowNumber = 3U;
                var sut = CreateSystemUnderTest();

                // Act
                sut.AdvanceToRow(rowNumber);

                // Assert
                Assert.Equal(rowNumber, sut.CurrentRowNumber);
            }

            [Fact]
            public void ResetsCurrentColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AdvanceColumn();

                // Act
                sut.AdvanceToRow(3U);

                // Assert
                Assert.Equal(1U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void WritesRowEndWhenCurrentRowContainsAtLeastOneCell()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AddCell(DefaultCell);

                // Act
                sut.AdvanceToRow(3U);

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteEndElement());
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenRowNumberIsZero()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToRow(0U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenRowNumberIsLessThanCurrentRowNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AdvanceRow();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToRow(1U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToRow(3U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAdvanceColumnMethod : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void IncrementsCurrentColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.AdvanceColumn();

                // Assert
                Assert.Equal(2U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AdvanceColumn());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAdvanceColumnsMethod : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void IncrementsCurrentColumnNumberByCount()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.AdvanceColumns(2U);

                // Assert
                Assert.Equal(3U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenCountIsZero()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AdvanceColumns(0U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AdvanceColumns(2U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAdvanceToColumnMethodWithColumnNumberParameter : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void IncrementsCurrentColumnNumberUntilEqualToProvidedColumnNumber()
            {
                // Arrange
                var rowNumber = 3U;
                var sut = CreateSystemUnderTest();

                // Act
                sut.AdvanceToColumn(rowNumber);

                // Assert
                Assert.Equal(rowNumber, sut.CurrentColumnNumber);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenColumnNumberIsZero()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToColumn(0U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenColumnNumberIsLessThanCurrentColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AdvanceColumn();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToColumn(1U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToColumn(3U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheAdvanceToColumnMethodWithColumnLetterParameter : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void IncrementsCurrentColumnNumberUntilEquivalentToProvidedColumnLetter()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.AdvanceToColumn("C");

                // Assert
                Assert.Equal(3U, sut.CurrentColumnNumber);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenColumnLetterIsLessThanCurrentColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AdvanceColumn();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToColumn("A"));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPageIsNotWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.Finish();

                // Act
                var exception = Record.Exception(() => sut.AdvanceToColumn("C"));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheFinishMethod : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void WritesPageEndsWhenPageIsWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.Finish();

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteEndElement(), Times.Exactly(2));
            }

            [Fact]
            public void WritesPageAndRowEndsWhenCurrentRowContainsAtLeastOneCellAndPageIsWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AddCell(DefaultCell);

                // Act
                sut.Finish();

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteEndElement(), Times.Exactly(3));
            }
        }

        public class TheDisposeMethod : WritingSpreadsheetPageFacts
        {
            [Fact]
            public void WritesPageEndsWhenPageIsWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.Dispose();

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteEndElement(), Times.Exactly(2));
            }

            [Fact]
            public void WritesPageAndRowEndsWhenCurrentRowContainsAtLeastOneCellAndPageIsWritable()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                sut.AddCell(DefaultCell);

                // Act
                sut.Dispose();

                // Assert
                _mocker.GetMock<IOpenXmlWriterWrapper>()
                    .Verify(writer => writer.WriteEndElement(), Times.Exactly(3));
            }
        }
    }
}
