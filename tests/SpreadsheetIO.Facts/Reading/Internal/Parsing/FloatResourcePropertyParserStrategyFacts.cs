using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing;

public class FloatResourcePropertyParserStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private FloatResourcePropertyParserStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<FloatResourcePropertyParserStrategy>();

    public class TheTryParseMethod : FloatResourcePropertyParserStrategyFacts
    {
        [Theory]
        [InlineData(default)]
        [InlineData("")]
        public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.FloatNullable);
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
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Float);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Missing, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenCellValueIsNotFloat()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Float);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("foo", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenCellValueIsFloat()
        {
            // Arrange
            var expectedValue = 1.5F;
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Float);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("1.5", map, out var actualValue);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
