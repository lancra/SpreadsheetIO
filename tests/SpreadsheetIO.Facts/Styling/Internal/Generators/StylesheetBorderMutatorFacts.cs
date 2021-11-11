using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Generators;

public class StylesheetBorderMutatorFacts
{
    private readonly AutoMocker _mocker = new();

    private StylesheetBorderMutator CreateSystemUnderTest()
        => _mocker.CreateInstance<StylesheetBorderMutator>();

    public class TheMutateMethod : StylesheetBorderMutatorFacts
    {
        [Fact]
        public void ModifiesSpreadsheetBordersWithIndexedBorders()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            var borders = new[]
            {
                new Border(
                    new BorderLine(Color.Red, BorderLineKind.Dashed),
                    new BorderLine(Color.Green, BorderLineKind.Dotted),
                    new BorderLine(Color.Blue, BorderLineKind.Double),
                    new BorderLine(Color.Yellow, BorderLineKind.Thick)),
                new Border(new BorderLine(Color.White, BorderLineKind.Thin)),
            };
            _mocker.GetMock<IBorderIndexer>()
                .Setup(borderIndexer => borderIndexer.Resources)
                .Returns(borders);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Borders);
            Assert.Equal(2U, stylesheet.Borders.Count.Value);
            Assert.Equal(2, stylesheet.Borders.ChildElements.Count);

            var firstBorder = Assert.IsType<OpenXml.Border>(stylesheet.Borders.ChildElements[0]);
            Assert.Equal("FFFF0000", firstBorder.LeftBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Dashed.OpenXmlValue, firstBorder.LeftBorder.Style.Value);
            Assert.Equal("FF008000", firstBorder.RightBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Dotted.OpenXmlValue, firstBorder.RightBorder.Style.Value);
            Assert.Equal("FF0000FF", firstBorder.TopBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Double.OpenXmlValue, firstBorder.TopBorder.Style.Value);
            Assert.Equal("FFFFFF00", firstBorder.BottomBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Thick.OpenXmlValue, firstBorder.BottomBorder.Style.Value);

            var secondBorder = Assert.IsType<OpenXml.Border>(stylesheet.Borders.ChildElements[1]);
            Assert.Equal("FFFFFFFF", secondBorder.LeftBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Thin.OpenXmlValue, secondBorder.LeftBorder.Style.Value);
            Assert.Equal("FFFFFFFF", secondBorder.RightBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Thin.OpenXmlValue, secondBorder.RightBorder.Style.Value);
            Assert.Equal("FFFFFFFF", secondBorder.TopBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Thin.OpenXmlValue, secondBorder.TopBorder.Style.Value);
            Assert.Equal("FFFFFFFF", secondBorder.BottomBorder.Color.Rgb);
            Assert.Equal(BorderLineKind.Thin.OpenXmlValue, secondBorder.BottomBorder.Style.Value);
        }

        [Fact]
        public void CreatesEmptySpreadsheetBordersWhenNoBordersAreIndexed()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            _mocker.GetMock<IBorderIndexer>()
                .Setup(borderIndexer => borderIndexer.Resources)
                .Returns(Array.Empty<Border>());

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Borders);
            Assert.Equal(0U, stylesheet.Borders.Count.Value);
            Assert.Equal(0, stylesheet.Borders.ChildElements.Count);
        }

        [Fact]
        public void DoesNotSetBorderLinePropertiesWhenBorderLineKindIsNone()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            var borders = new[]
            {
                new Border(new BorderLine(Color.Black, BorderLineKind.None)),
            };
            _mocker.GetMock<IBorderIndexer>()
                .Setup(borderIndexer => borderIndexer.Resources)
                .Returns(borders);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Borders);
            Assert.Equal(1U, stylesheet.Borders.Count.Value);
            Assert.Equal(1, stylesheet.Borders.ChildElements.Count);

            var border = Assert.IsType<OpenXml.Border>(stylesheet.Borders.ChildElements[0]);
            Assert.Null(border.LeftBorder.Color);
            Assert.Null(border.LeftBorder.Style);
            Assert.Null(border.RightBorder.Color);
            Assert.Null(border.RightBorder.Style);
            Assert.Null(border.TopBorder.Color);
            Assert.Null(border.TopBorder.Style);
            Assert.Null(border.BottomBorder.Color);
            Assert.Null(border.BottomBorder.Style);
        }
    }
}
