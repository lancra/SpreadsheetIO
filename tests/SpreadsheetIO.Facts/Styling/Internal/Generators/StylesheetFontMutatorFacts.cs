using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Generators;

public class StylesheetFontMutatorFacts
{
    private readonly AutoMocker _mocker = new();

    private StylesheetFontMutator CreateSystemUnderTest()
        => _mocker.CreateInstance<StylesheetFontMutator>();

    public class TheMutateMethod : StylesheetFontMutatorFacts
    {
        [Fact]
        public void ModifiesSpreadsheetFontsWithIndexedFonts()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            var fonts = new[]
            {
                new Font("Arial", 11D, Color.Red),
                new Font("Calibri", 20D, Color.Green),
            };
            _mocker.GetMock<IFontIndexer>()
                .Setup(fontIndexer => fontIndexer.Resources)
                .Returns(fonts);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Fonts);
            Assert.Equal(2U, stylesheet.Fonts.Count.Value);
            Assert.Equal(2, stylesheet.Fonts.ChildElements.Count);

            var firstFont = Assert.IsType<OpenXml.Font>(stylesheet.Fonts.ChildElements[0]);
            Assert.Equal("Arial", firstFont.FontName.Val);
            Assert.Equal(11D, firstFont.FontSize.Val.Value);
            Assert.Equal("FFFF0000", firstFont.Color.Rgb);
            Assert.Null(firstFont.Bold);
            Assert.Null(firstFont.Italic);

            var secondFont = Assert.IsType<OpenXml.Font>(stylesheet.Fonts.ChildElements[1]);
            Assert.Equal("Calibri", secondFont.FontName.Val);
            Assert.Equal(20D, secondFont.FontSize.Val.Value);
            Assert.Equal("FF008000", secondFont.Color.Rgb);
            Assert.Null(secondFont.Bold);
            Assert.Null(secondFont.Italic);
        }

        [Fact]
        public void CreatesEmptySpreadsheetFontsWhenNoFontsAreIndexed()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            _mocker.GetMock<IFontIndexer>()
                .Setup(fontIndexer => fontIndexer.Resources)
                .Returns(Array.Empty<Font>());

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Fonts);
            Assert.Equal(0U, stylesheet.Fonts.Count.Value);
            Assert.Equal(0, stylesheet.Fonts.ChildElements.Count);
        }

        [Fact]
        public void SetsBoldPropertyWhenFontIsBold()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            var fonts = new[]
            {
                Font.Default with { IsBold = true },
            };
            _mocker.GetMock<IFontIndexer>()
                .Setup(fontIndexer => fontIndexer.Resources)
                .Returns(fonts);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Fonts);
            Assert.Equal(1, stylesheet.Fonts.ChildElements.Count);

            var font = Assert.IsType<OpenXml.Font>(stylesheet.Fonts.ChildElements[0]);
            Assert.NotNull(font.Bold);
        }

        [Fact]
        public void SetsFontItalicPropertyWhenFontIsItalic()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            var fonts = new[]
            {
                Font.Default with { IsItalic = true },
            };
            _mocker.GetMock<IFontIndexer>()
                .Setup(fontIndexer => fontIndexer.Resources)
                .Returns(fonts);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Fonts);
            Assert.Equal(1U, stylesheet.Fonts.Count.Value);
            Assert.Equal(1, stylesheet.Fonts.ChildElements.Count);

            var font = Assert.IsType<OpenXml.Font>(stylesheet.Fonts.ChildElements[0]);
            Assert.NotNull(font.Italic);
        }
    }
}
