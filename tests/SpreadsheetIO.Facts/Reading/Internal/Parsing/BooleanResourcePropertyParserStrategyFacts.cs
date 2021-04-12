using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing
{
    public class BooleanResourcePropertyParserStrategyFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private BooleanResourcePropertyParserStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<BooleanResourcePropertyParserStrategy>();

        public class TheTryParseMethod : BooleanResourcePropertyParserStrategyFacts
        {
            [Theory]
            [InlineData(default)]
            [InlineData("")]
            public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.BooleanNullable);
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
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.Boolean);
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
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.Boolean);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("foo", map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
                Assert.Null(value);
            }

            [Fact]
            public void ReturnsInvalidParseResultWhenCellValueIsNotZeroOrOne()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.Boolean);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("2", map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
                Assert.Null(value);
            }

            [Theory]
            [InlineData("0", false)]
            [InlineData("1", true)]
            public void ReturnsSuccessParseResultWhenCellValueIsZeroOrOne(string cellValue, bool expectedValue)
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeParserStrategyModel(model => model.Boolean);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse(cellValue, map, out var actualValue);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
                Assert.Equal(expectedValue, actualValue);
            }
        }
    }
}
