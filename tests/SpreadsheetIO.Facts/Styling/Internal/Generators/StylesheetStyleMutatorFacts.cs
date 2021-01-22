using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Generators
{
    public class StylesheetStyleMutatorFacts
    {
        private static readonly FakeStyle ExcelStyleMock =
            new FakeStyle(
                BuiltInExcelStyle.Normal.IndexerKey,
                new IndexedResource<Style>(BuiltInExcelStyle.Normal.Style, 0),
                0,
                0,
                0);

        private static readonly FakeStyle PackageStyleMock =
            new FakeStyle(
                BuiltInPackageStyle.Bold.IndexerKey,
                new IndexedResource<Style>(BuiltInPackageStyle.Bold.Style, 1),
                0,
                0,
                1);

        private static readonly FakeStyle CustomStyleMock =
            new FakeStyle(
                new IndexerKey("Custom", IndexerKeyKind.Custom),
                new IndexedResource<Style>(
                    new Style(
                        new Border(new BorderLine(Color.White, BorderLineKind.Thick)),
                        new Fill(FillKind.Solid, Color.Black),
                        new Font("Arial", 20D, Color.Red)),
                    2),
                1,
                1,
                2);

        private readonly AutoMocker _mocker = new AutoMocker();
        private readonly IList<IndexerKey> _mockStyleKeys = new List<IndexerKey>();

        private StylesheetStyleMutator CreateSystemUnderTest()
            => _mocker.CreateInstance<StylesheetStyleMutator>();

        private void MockIndexers(FakeStyle styleMock)
        {
            _mocker.GetMock<IBorderIndexer>()
                .SetupGet(borderIndexer => borderIndexer[styleMock.Style.Resource.Border])
                .Returns(styleMock.BorderId);

            _mocker.GetMock<IFillIndexer>()
                .SetupGet(fillIndexer => fillIndexer[styleMock.Style.Resource.Fill])
                .Returns(styleMock.FillId);

            _mocker.GetMock<IFontIndexer>()
                .SetupGet(fontIndexer => fontIndexer[styleMock.Style.Resource.Font])
                .Returns(styleMock.FontId);

            _mocker.GetMock<IStyleIndexer>()
                .SetupGet(styleIndexer => styleIndexer[styleMock.Key])
                .Returns(styleMock.Style);

            _mockStyleKeys.Add(styleMock.Key);
            _mocker.GetMock<IStyleIndexer>()
                .Setup(styleIndexer => styleIndexer.Keys)
                .Returns(_mockStyleKeys.ToArray());
        }

        private void MockIndexersForDefaultStyles()
        {
            MockIndexers(ExcelStyleMock);
            MockIndexers(PackageStyleMock);
            MockIndexers(CustomStyleMock);
        }

        public class TheMutateMethod : StylesheetStyleMutatorFacts
        {
            [Fact]
            public void ModifiesSpreadsheetCellFormatsWithIndexedStyles()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                MockIndexersForDefaultStyles();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.CellFormats);
                Assert.Equal(3U, stylesheet.CellFormats.Count.Value);
                Assert.Equal(3, stylesheet.CellFormats.ChildElements.Count);

                var firstCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[0]);
                Assert.Equal(0U, firstCellFormat.NumberFormatId.Value);
                Assert.Equal(0U, firstCellFormat.FontId.Value);
                Assert.Equal(0U, firstCellFormat.FillId.Value);
                Assert.Equal(0U, firstCellFormat.BorderId.Value);
                Assert.Equal(0U, firstCellFormat.FormatId.Value);
                Assert.False(firstCellFormat.ApplyNumberFormat);
                Assert.True(firstCellFormat.ApplyFont);
                Assert.True(firstCellFormat.ApplyFill);
                Assert.True(firstCellFormat.ApplyBorder);
                Assert.False(firstCellFormat.ApplyAlignment);
                Assert.False(firstCellFormat.ApplyProtection);

                var secondCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[1]);
                Assert.Equal(0U, secondCellFormat.NumberFormatId.Value);
                Assert.Equal(1U, secondCellFormat.FontId.Value);
                Assert.Equal(0U, secondCellFormat.FillId.Value);
                Assert.Equal(0U, secondCellFormat.BorderId.Value);
                Assert.Equal(0U, secondCellFormat.FormatId.Value);
                Assert.False(secondCellFormat.ApplyNumberFormat);
                Assert.True(secondCellFormat.ApplyFont);
                Assert.True(secondCellFormat.ApplyFill);
                Assert.True(secondCellFormat.ApplyBorder);
                Assert.False(secondCellFormat.ApplyAlignment);
                Assert.False(secondCellFormat.ApplyProtection);

                var thirdCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[2]);
                Assert.Equal(0U, thirdCellFormat.NumberFormatId.Value);
                Assert.Equal(2U, thirdCellFormat.FontId.Value);
                Assert.Equal(1U, thirdCellFormat.FillId.Value);
                Assert.Equal(1U, thirdCellFormat.BorderId.Value);
                Assert.Equal(0U, thirdCellFormat.FormatId.Value);
                Assert.False(thirdCellFormat.ApplyNumberFormat);
                Assert.True(thirdCellFormat.ApplyFont);
                Assert.True(thirdCellFormat.ApplyFill);
                Assert.True(thirdCellFormat.ApplyBorder);
                Assert.False(thirdCellFormat.ApplyAlignment);
                Assert.False(thirdCellFormat.ApplyProtection);
            }

            [Fact]
            public void ModifiesSpreadsheetCellStyleFormatsWithIndexedExcelStyles()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                MockIndexersForDefaultStyles();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.CellStyleFormats);
                Assert.Equal(1U, stylesheet.CellStyleFormats.Count.Value);
                Assert.Equal(1, stylesheet.CellStyleFormats.ChildElements.Count);

                var cellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellStyleFormats.ChildElements[0]);
                Assert.Equal(0U, cellFormat.NumberFormatId.Value);
                Assert.Equal(0U, cellFormat.FontId.Value);
                Assert.Equal(0U, cellFormat.FillId.Value);
                Assert.Equal(0U, cellFormat.BorderId.Value);
                Assert.False(cellFormat.ApplyNumberFormat);
                Assert.True(cellFormat.ApplyFont);
                Assert.True(cellFormat.ApplyFill);
                Assert.True(cellFormat.ApplyBorder);
                Assert.False(cellFormat.ApplyAlignment);
                Assert.False(cellFormat.ApplyProtection);
            }

            [Fact]
            public void ModifiesSpreadsheetCellStylesWithIndexedExcelStyles()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                MockIndexersForDefaultStyles();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.CellStyles);
                Assert.Equal(1U, stylesheet.CellStyles.Count.Value);
                Assert.Equal(1, stylesheet.CellStyles.ChildElements.Count);

                var cellStyle = Assert.IsType<OpenXml.CellStyle>(stylesheet.CellStyles.ChildElements[0]);
                Assert.Equal(ExcelStyleMock.Key.Name, cellStyle.Name);
                Assert.Equal(0U, cellStyle.FormatId.Value);
                Assert.Equal(ExcelStyleMock.Style.Resource.BuiltInId, cellStyle.BuiltinId.Value);
            }

            [Fact]
            public void SetsNonZeroFormatIdentifierForAdditionalExcelStyles()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                MockIndexersForDefaultStyles();

                var secondExcelStyleMock = new FakeStyle(
                    BuiltInExcelStyle.Bad.IndexerKey,
                    new IndexedResource<Style>(BuiltInExcelStyle.Bad.Style, 3),
                    0,
                    2,
                    3);
                MockIndexers(secondExcelStyleMock);

                var thirdExcelStyleMock = new FakeStyle(
                    BuiltInExcelStyle.Good.IndexerKey,
                    new IndexedResource<Style>(BuiltInExcelStyle.Good.Style, 4),
                    0,
                    3,
                    4);
                MockIndexers(thirdExcelStyleMock);

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.CellFormats);
                Assert.Equal(5, stylesheet.CellFormats.ChildElements.Count);

                var firstCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[0]);
                Assert.Equal(0U, firstCellFormat.FormatId.Value);
                var secondCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[1]);
                Assert.Equal(0U, secondCellFormat.FormatId.Value);
                var thirdCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[2]);
                Assert.Equal(0U, thirdCellFormat.FormatId.Value);
                var fourthCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[3]);
                Assert.Equal(1U, fourthCellFormat.FormatId.Value);
                var fifthCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[4]);
                Assert.Equal(2U, fifthCellFormat.FormatId.Value);

                Assert.NotNull(stylesheet.CellStyles);
                Assert.Equal(3, stylesheet.CellStyles.ChildElements.Count);

                var firstCellStyle = Assert.IsType<OpenXml.CellStyle>(stylesheet.CellStyles.ChildElements[0]);
                Assert.Equal(0U, firstCellStyle.FormatId.Value);
                var secondCellStyle = Assert.IsType<OpenXml.CellStyle>(stylesheet.CellStyles.ChildElements[1]);
                Assert.Equal(1U, secondCellStyle.FormatId.Value);
                var thirdCellStyle = Assert.IsType<OpenXml.CellStyle>(stylesheet.CellStyles.ChildElements[2]);
                Assert.Equal(2U, thirdCellStyle.FormatId.Value);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenIndexedExcelStyleDoesNotHaveBuiltInId()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                var invalidExcelStyleMock = new FakeStyle(
                    new IndexerKey("Invalid Style", IndexerKeyKind.Excel),
                    new IndexedResource<Style>(
                        new Style(Border.Default, Fill.Default, Font.Default),
                        0),
                    0,
                    0,
                    0);
                MockIndexers(invalidExcelStyleMock);

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Mutate(stylesheet));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        private class FakeStyle
        {
            public FakeStyle(IndexerKey key, IndexedResource<Style> style, uint borderId, uint fillId, uint fontId)
            {
                Key = key;
                Style = style;
                BorderId = borderId;
                FillId = fillId;
                FontId = fontId;
            }

            public IndexerKey Key { get; }

            public IndexedResource<Style> Style { get; }

            public uint BorderId { get; }

            public uint FillId { get; }

            public uint FontId { get; }
        }
    }
}
