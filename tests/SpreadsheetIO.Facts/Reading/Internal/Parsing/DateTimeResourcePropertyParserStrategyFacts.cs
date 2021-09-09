using System;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Mapping.Internal.Extensions;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using LanceC.SpreadsheetIO.Shared;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing
{
    public class DateTimeResourcePropertyParserStrategyFacts
    {
        private readonly AutoMocker _mocker = new();

        private DateTimeResourcePropertyParserStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<DateTimeResourcePropertyParserStrategy>();

        public class TheTryParseMethod : DateTimeResourcePropertyParserStrategyFacts
        {
            [Theory]
            [InlineData(default)]
            [InlineData("")]
            public void ReturnsEmptyParseResultWhenCellValueIsNullOrEmptyAndPropertyTypeIsNullable(string cellValue)
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeNullable);
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
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTime);
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
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTime);
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
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTime);
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
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTime);
                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("12345.67890", map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
                Assert.Equal(new DateTime(1933, 10, 18, 16, 17, 36, 960), value);
            }

            [Fact]
            public void ReturnsInvalidParseResultWhenDateKindOfNumberIsSpecifiedAndCellValueIsNotDouble()
            {
                // Arrange
                var map = PropertyMapCreator
                    .CreateForFakeResourcePropertyStrategyModel(model => model.DateTime, new DateKindMapOptionsExtension(CellDateKind.Number));

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
                    .CreateForFakeResourcePropertyStrategyModel(model => model.DateTime, new DateKindMapOptionsExtension(CellDateKind.Number));

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
                    .CreateForFakeResourcePropertyStrategyModel(model => model.DateTime, new DateKindMapOptionsExtension(CellDateKind.Number));

                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("12345.67890", map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
                Assert.Equal(new DateTime(1933, 10, 18, 16, 17, 36, 960), value);
            }

            [Fact]
            public void ReturnsInvalidParseResultWhenDateKindOfTextIsSpecifiedAndCellValueIsNotDateTime()
            {
                // Arrange
                var map = PropertyMapCreator
                    .CreateForFakeResourcePropertyStrategyModel(model => model.DateTime, new DateKindMapOptionsExtension(CellDateKind.Text));

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
                    .CreateForFakeResourcePropertyStrategyModel(model => model.DateTime, new DateKindMapOptionsExtension(CellDateKind.Text));

                var sut = CreateSystemUnderTest();

                // Act
                var parseResult = sut.TryParse("2021-02-03 04:05:06.789", map, out var value);

                // Assert
                Assert.Equal(ResourcePropertyParseResultKind.Success, parseResult);
                Assert.Equal(new DateTime(2021, 2, 3, 4, 5, 6, 789), value);
            }
        }
    }
}
