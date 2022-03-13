using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing;

public class CharacterResourcePropertyParserStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private CharacterResourcePropertyParserStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<CharacterResourcePropertyParserStrategy>();

    public class TheTryParseMethod : CharacterResourcePropertyParserStrategyFacts
    {
        [Theory]
        [InlineData(default)]
        [InlineData("")]
        public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.CharacterNullable);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Empty, parseResult);
            Assert.Null(value);
        }

        [Theory]
        [InlineData(default)]
        [InlineData("")]
        public void ReturnsMissingParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNotNullable(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Character);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Missing, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenCellValueIsNotCharacter()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Character);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("foo", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenCellValueIsCharacter()
        {
            // Arrange
            var expectedValue = 'A';
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Character);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("A", map, out var actualValue);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
