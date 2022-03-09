using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing;

public class DoubleResourcePropertyParserStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private DoubleResourcePropertyParserStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<DoubleResourcePropertyParserStrategy>();

    public class TheTryParseMethod : DoubleResourcePropertyParserStrategyFacts
    {
        [Theory]
        [InlineData(default)]
        [InlineData("")]
        public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.DoubleNullable);
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
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.Double);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Missing, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenCellValueIsNotDouble()
        {
            // Arrange
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.Double);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("foo", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenCellValueIsDouble()
        {
            // Arrange
            var expectedValue = 1.5D;
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.Double);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("1.5", map, out var actualValue);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
