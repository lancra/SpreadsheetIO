using System;
using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Generators
{
    public class StylesheetStyleMutatorFacts
    {
        private static readonly IndexerKey ExcelIndexerKey = BuiltInExcelStyle.Normal.IndexerKey;

        private static readonly IndexedResource<Style> ExcelIndexedStyle =
            new IndexedResource<Style>(BuiltInExcelStyle.Normal.Style, 0);

        private static readonly IndexerKey PackageIndexerKey = BuiltInPackageStyle.Bold.IndexerKey;

        private static readonly IndexedResource<Style> PackageIndexedStyle =
            new IndexedResource<Style>(BuiltInPackageStyle.Bold.Style, 1);

        private static readonly IndexerKey CustomIndexerKey = new IndexerKey("Custom", IndexerKeyKind.Custom);

        private static readonly IndexedResource<Style> CustomIndexedStyle =
            new IndexedResource<Style>(
                new Style(
                    new Border(new BorderLine(Color.White, BorderLineKind.Thick)),
                    new Fill(FillKind.Solid, Color.Black),
                    new Font("Arial", 20D, Color.Red)),
                2);

        private readonly AutoMocker _mocker = new AutoMocker();

        private StylesheetStyleMutator CreateSystemUnderTest()
            => _mocker.CreateInstance<StylesheetStyleMutator>();

        private void MockBorderIndexer()
        {
            var borderIndexerMock = _mocker.GetMock<IBorderIndexer>();
            borderIndexerMock.SetupGet(borderIndexer => borderIndexer[ExcelIndexedStyle.Resource.Border])
                .Returns(0);
            borderIndexerMock.SetupGet(borderIndexer => borderIndexer[PackageIndexedStyle.Resource.Border])
                .Returns(0);
            borderIndexerMock.SetupGet(borderIndexer => borderIndexer[CustomIndexedStyle.Resource.Border])
                .Returns(1);
        }

        private void MockFillIndexer()
        {
            var fillIndexerMock = _mocker.GetMock<IFillIndexer>();
            fillIndexerMock.SetupGet(fillIndexer => fillIndexer[ExcelIndexedStyle.Resource.Fill])
                .Returns(0);
            fillIndexerMock.SetupGet(fillIndexer => fillIndexer[PackageIndexedStyle.Resource.Fill])
                .Returns(0);
            fillIndexerMock.SetupGet(fillIndexer => fillIndexer[CustomIndexedStyle.Resource.Fill])
                .Returns(1);
        }

        private void MockFontIndexer()
        {
            var fontIndexerMock = _mocker.GetMock<IFontIndexer>();
            fontIndexerMock.SetupGet(fontIndexer => fontIndexer[ExcelIndexedStyle.Resource.Font])
                .Returns(0);
            fontIndexerMock.SetupGet(fontIndexer => fontIndexer[PackageIndexedStyle.Resource.Font])
                .Returns(1);
            fontIndexerMock.SetupGet(fontIndexer => fontIndexer[CustomIndexedStyle.Resource.Font])
                .Returns(2);
        }

        private void MockStyleIndexer()
        {
            var styleIndexerMock = _mocker.GetMock<IStyleIndexer>();
            styleIndexerMock.SetupGet(styleIndexer => styleIndexer.Keys)
                .Returns(
                    new[]
                    {
                        ExcelIndexerKey,
                        PackageIndexerKey,
                        CustomIndexerKey,
                    });

            styleIndexerMock.SetupGet(styleIndexer => styleIndexer[ExcelIndexerKey])
                .Returns(ExcelIndexedStyle);
            styleIndexerMock.SetupGet(styleIndexer => styleIndexer[PackageIndexerKey])
                .Returns(PackageIndexedStyle);
            styleIndexerMock.SetupGet(styleIndexer => styleIndexer[CustomIndexerKey])
                .Returns(CustomIndexedStyle);
        }

        public class TheMutateMethod : StylesheetStyleMutatorFacts
        {
            [Fact]
            public void ModifiesSpreadsheetCellFormatsWithIndexedStyles()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                MockBorderIndexer();
                MockFillIndexer();
                MockFontIndexer();
                MockStyleIndexer();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.CellFormats);
                Assert.Equal(3, stylesheet.CellFormats.ChildElements.Count);

                var firstCellFormat = Assert.IsType<OpenXml.CellFormat>(stylesheet.CellFormats.ChildElements[0]);
                Assert.Equal(0U, firstCellFormat.NumberFormatId.Value);
                Assert.Equal(0U, firstCellFormat.FontId.Value);
                Assert.Equal(0U, firstCellFormat.FillId.Value);
                Assert.Equal(0U, firstCellFormat.BorderId.Value);
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

                MockBorderIndexer();
                MockFillIndexer();
                MockFontIndexer();
                MockStyleIndexer();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.CellStyleFormats);
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

                MockBorderIndexer();
                MockFillIndexer();
                MockFontIndexer();
                MockStyleIndexer();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.CellStyles);
                Assert.Equal(1, stylesheet.CellStyles.ChildElements.Count);

                var cellStyle = Assert.IsType<OpenXml.CellStyle>(stylesheet.CellStyles.ChildElements[0]);
                Assert.Equal(ExcelIndexerKey.Name, cellStyle.Name);
                Assert.Equal(0U, cellStyle.FormatId.Value);
                Assert.Equal(ExcelIndexedStyle.Resource.BuiltInId, cellStyle.BuiltinId.Value);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenIndexedExcelStyleDoesNotHaveBuiltInId()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                _mocker.GetMock<IBorderIndexer>()
                    .SetupGet(borderIndexer => borderIndexer[It.IsAny<Border>()])
                    .Returns(0U);
                _mocker.GetMock<IFillIndexer>()
                    .SetupGet(fillIndexer => fillIndexer[It.IsAny<Fill>()])
                    .Returns(0U);
                _mocker.GetMock<IFontIndexer>()
                    .SetupGet(fontIndexer => fontIndexer[It.IsAny<Font>()])
                    .Returns(0U);

                var indexerKey = new IndexerKey("Invalid Style", IndexerKeyKind.Excel);
                var indexedStyle = new IndexedResource<Style>(
                    new Style(Border.Default, Fill.Default, Font.Default),
                    0U);
                var styleIndexerMock = _mocker.GetMock<IStyleIndexer>();
                styleIndexerMock.SetupGet(styleIndexer => styleIndexer.Keys)
                    .Returns(new[] { indexerKey, });
                styleIndexerMock.SetupGet(styleIndexer => styleIndexer[indexerKey])
                    .Returns(indexedStyle);

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Mutate(stylesheet));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }
    }
}
