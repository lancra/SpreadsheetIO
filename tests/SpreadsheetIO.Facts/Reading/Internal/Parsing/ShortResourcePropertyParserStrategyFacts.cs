using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing
{
    public class ShortResourcePropertyParserStrategyFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private ShortResourcePropertyParserStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<ShortResourcePropertyParserStrategy>();

        public class TheTryParseMethod : ShortResourcePropertyParserStrategyFacts
        {
            [Theory]
            [InlineData(default)]
            [InlineData("")]
            public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.ShortNullable);
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
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.Short);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse(cellValue, map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Missing, parseResult);
                Assert.Null(value);
            }

            [Fact]
            public void ReturnsInvalidParseResultWhenCellValueIsNotShort()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.Short);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("foo", map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
                Assert.Null(value);
            }

            [Fact]
            public void ReturnsSuccessParseResultWhenCellValueIsShort()
            {
                // Arrange
                short expectedValue = 1;
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.Short);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("1", map, out var actualValue);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
                Assert.Equal(expectedValue, actualValue);
            }
        }
    }
}
