using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing
{
    public class IntegerResourcePropertyParserStrategyFacts
    {
        private readonly AutoMocker _mocker = new();

        private IntegerResourcePropertyParserStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<IntegerResourcePropertyParserStrategy>();

        public class TheTryParseMethod : IntegerResourcePropertyParserStrategyFacts
        {
            [Theory]
            [InlineData(default)]
            [InlineData("")]
            public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.IntegerNullable);
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
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Integer);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse(cellValue, map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Missing, parseResult);
                Assert.Null(value);
            }

            [Fact]
            public void ReturnsInvalidParseResultWhenCellValueIsNotInteger()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Integer);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("foo", map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
                Assert.Null(value);
            }

            [Fact]
            public void ReturnsSuccessParseResultWhenCellValueIsInteger()
            {
                // Arrange
                var expectedValue = 1;
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Integer);
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
