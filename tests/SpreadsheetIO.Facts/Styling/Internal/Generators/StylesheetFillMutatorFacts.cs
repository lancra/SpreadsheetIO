using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Generators;

public class StylesheetFillMutatorFacts
{
    private readonly AutoMocker _mocker = new();

    private StylesheetFillMutator CreateSystemUnderTest()
        => _mocker.CreateInstance<StylesheetFillMutator>();

    public class TheMutateMethod : StylesheetFillMutatorFacts
    {
        [Fact]
        public void ModifiesSpreadsheetFillsWithIndexedFills()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            var fills = new[]
            {
                new Fill(FillKind.Gray125, Color.Red),
                new Fill(FillKind.Solid, Color.Green),
            };
            _mocker.GetMock<IFillIndexer>()
                .Setup(fillIndexer => fillIndexer.Resources)
                .Returns(fills);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Fills);
            Assert.Equal(2U, stylesheet.Fills.Count.Value);
            Assert.Equal(2, stylesheet.Fills.ChildElements.Count);

            var firstFill = Assert.IsType<OpenXml.Fill>(stylesheet.Fills.ChildElements[0]);
            Assert.Equal(FillKind.Gray125.OpenXmlValue, firstFill.PatternFill.PatternType.Value);
            Assert.Equal("FFFF0000", firstFill.PatternFill.ForegroundColor.Rgb);

            var secondFill = Assert.IsType<OpenXml.Fill>(stylesheet.Fills.ChildElements[1]);
            Assert.Equal(FillKind.Solid.OpenXmlValue, secondFill.PatternFill.PatternType.Value);
            Assert.Equal("FF008000", secondFill.PatternFill.ForegroundColor.Rgb);
        }

        [Fact]
        public void CreatesEmptySpreadsheetFillsWhenNoFillsAreIndexed()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            _mocker.GetMock<IFillIndexer>()
                .Setup(fillIndexer => fillIndexer.Resources)
                .Returns(Array.Empty<Fill>());

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Fills);
            Assert.Equal(0U, stylesheet.Fills.Count.Value);
            Assert.Equal(0, stylesheet.Fills.ChildElements.Count);
        }

        [Fact]
        public void DoesNotSetForegroundColorWhenFillKindIsNone()
        {
            // Arrange
            var stylesheet = new OpenXml.Stylesheet();

            var fills = new[]
            {
                new Fill(FillKind.None, Color.White),
            };
            _mocker.GetMock<IFillIndexer>()
                .Setup(fillIndexer => fillIndexer.Resources)
                .Returns(fills);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Mutate(stylesheet);

            // Assert
            Assert.NotNull(stylesheet.Fills);
            Assert.Equal(1U, stylesheet.Fills.Count.Value);
            Assert.Equal(1, stylesheet.Fills.ChildElements.Count);

            var fill = Assert.IsType<OpenXml.Fill>(stylesheet.Fills.ChildElements[0]);
            Assert.Equal(FillKind.None.OpenXmlValue, fill.PatternFill.PatternType.Value);
            Assert.Null(fill.PatternFill.ForegroundColor);
        }
    }
}
