using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using LanceC.SpreadsheetIO.Shared;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing;

public class DateTimeOffsetResourcePropertyParserStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private DateTimeOffsetResourcePropertyParserStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<DateTimeOffsetResourcePropertyParserStrategy>();

    public class TheTryParseMethod : DateTimeOffsetResourcePropertyParserStrategyFacts
    {
        public static readonly TheoryData<string, DateTimeOffset> DataForReturnsSuccessParseResultForValid1900Dates =
            new()
            {
                { "61", new DateTimeOffset(new DateTime(1900, 3, 1)) },
                { "59", new DateTimeOffset(new DateTime(1900, 2, 28)) },
                { "58", new DateTimeOffset(new DateTime(1900, 2, 27)) },
                { "2", new DateTimeOffset(new DateTime(1900, 1, 2)) },
                { "1", new DateTimeOffset(new DateTime(1900, 1, 1)) },
                { "-1", new DateTimeOffset(new DateTime(1899, 12, 31)) },
                { "-2", new DateTimeOffset(new DateTime(1899, 12, 30)) },
            };

        [Theory]
        [InlineData(default)]
        [InlineData("")]
        public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffsetNullable);
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
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Missing, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenNoDateKindIsSpecifiedAndCellValueIsNotDouble()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("foo", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenNoDateKindIsSpecifiedAndCellValueIsNotOADate()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(double.MaxValue.ToString(), map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenNoDateKindIsSpecifiedAndCellValueIsOADate()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("12345.67890", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(DateTimeOffset.Parse("1933-10-18T16:17:36.960"), value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenDateKindOfNumberIsSpecifiedAndCellValueIsNotDouble()
        {
            // Arrange
            var map = PropertyMapCreator
                .CreateForFakeResourcePropertyStrategyModel(
                    model => model.DateTimeOffset,
                    new DateKindMapOption(CellDateKind.Number));

            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("foo", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenDateKindOfNumberIsSpecifiedAndCellValueIsNotOADate()
        {
            // Arrange
            var map = PropertyMapCreator
                .CreateForFakeResourcePropertyStrategyModel(
                    model => model.DateTimeOffset,
                    new DateKindMapOption(CellDateKind.Number));

            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(double.MaxValue.ToString(), map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenDateKindOfNumberIsSpecifiedAndCellValueIsOADate()
        {
            // Arrange
            var map = PropertyMapCreator
                .CreateForFakeResourcePropertyStrategyModel(
                    model => model.DateTimeOffset,
                    new DateKindMapOption(CellDateKind.Number));

            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("12345.67890", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(DateTimeOffset.Parse("1933-10-18T16:17:36.960"), value);
        }

        [Fact]
        public void ReturnsInvalidParseResultWhenDateKindOfTextIsSpecifiedAndCellValueIsNotDateTime()
        {
            // Arrange
            var map = PropertyMapCreator
                .CreateForFakeResourcePropertyStrategyModel(
                    model => model.DateTimeOffset,
                    new DateKindMapOption(CellDateKind.Text));

            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("foo", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsSuccessParseResultWhenDateKindOfTextIsSpecifiedAndCellValueIsDateTime()
        {
            // Arrange
            var map = PropertyMapCreator
                .CreateForFakeResourcePropertyStrategyModel(
                    model => model.DateTimeOffset,
                    new DateKindMapOption(CellDateKind.Text));

            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse("2021-02-03T04:05:06.7890-05:00", map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(new DateTimeOffset(2021, 2, 3, 4, 5, 6, 789, new TimeSpan(-5, 0, 0)), value);
        }

        [Theory]
        [MemberData(nameof(DataForReturnsSuccessParseResultForValid1900Dates))]
        public void ReturnsSuccessParseResultForValid1900Dates(string cellValue, DateTimeOffset expectedValue)
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var actualValue);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("60")]
        [InlineData("0")]
        public void ReturnsInvalidParseResultForInvalid1900Dates(string cellValue)
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);
            var sut = CreateSystemUnderTest();

            // Act
            var parseResult = sut.TryParse(cellValue, map, out var value);

            // Assert
            Assert.Equal(ResourcePropertyParseResultKind.Invalid, parseResult);
            Assert.Null(value);
        }
    }
}
