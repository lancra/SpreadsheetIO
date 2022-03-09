using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing;

public class StringResourcePropertyParserStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private StringResourcePropertyParserStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<StringResourcePropertyParserStrategy>();

    public class TheTryParseMethod : StringResourcePropertyParserStrategyFacts
    {
        [Theory]
        [InlineData(default)]
        [InlineData("")]
        public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmpty(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.String);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Empty, parseResult);
            Assert.Equal(cellValue, value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenCellValueIsNotNullOrEmpty()
        {
            // Arrange
            var cellValue = "foo";

            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.String);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(cellValue, value);
        }
    }
}
