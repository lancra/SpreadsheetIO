using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing;

public class ByteResourcePropertyParserStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private ByteResourcePropertyParserStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<ByteResourcePropertyParserStrategy>();

    public class TheTryParseMethod : ByteResourcePropertyParserStrategyFacts
    {
        [Theory]
        [InlineData(default)]
        [InlineData("")]
        public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.ByteNullable);
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
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Byte);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Missing, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenCellValueIsNotByte()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Byte);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("foo", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenCellValueIsByte()
        {
            // Arrange
            byte expectedValue = 1;
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Byte);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("1", map, out var actualValue);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
