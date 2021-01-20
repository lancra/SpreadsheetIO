using System;
using LanceC.SpreadsheetIO.Shared.Internal;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal
{
    public class ColumnLocationFacts
    {
        public class TheConstructorWithNumberParameter
        {
            [Fact]
            public void SetsNumberProperty()
            {
                // Arrange
                var number = 1U;

                // Act
                var columnLocation = new ColumnLocation(number);

                // Assert
                Assert.Equal(number, columnLocation.Number);
            }

            [Theory]
            [InlineData(1U, "A")]
            [InlineData(26U, "Z")]
            [InlineData(27U, "AA")]
            [InlineData(53U, "BA")]
            [InlineData(702U, "ZZ")]
            [InlineData(703U, "AAA")]
            public void SetsLetterPropertyWithMatchingValue(uint number, string expectedLetter)
            {
                // Act
                var columnLocation = new ColumnLocation(number);

                // Assert
                Assert.Equal(expectedLetter, columnLocation.Letter);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenNumberIsZero()
            {
                // Arrange
                var number = 0U;

                // Act
                var exception = Record.Exception(() => new ColumnLocation(number));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }
        }

        public class TheConstructorWithLetterParameter
        {
            [Fact]
            public void SetsLetterProperty()
            {
                // Arrange
                var letter = "A";

                // Act
                var columnLocation = new ColumnLocation(letter);

                // Assert
                Assert.Equal(letter, columnLocation.Letter);
            }

            [Theory]
            [InlineData("A", 1U)]
            [InlineData("Z", 26U)]
            [InlineData("AA", 27U)]
            [InlineData("BA", 53U)]
            [InlineData("ZZ", 702U)]
            [InlineData("AAA", 703U)]
            public void SetsNumberPropertyWithMatchingValue(string letter, uint expectedNumber)
            {
                // Act
                var columnLocation = new ColumnLocation(letter);

                // Assert
                Assert.Equal(expectedNumber, columnLocation.Number);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenLetterIsEmpty()
            {
                // Arrange
                var letter = string.Empty;

                // Act
                var exception = Record.Exception(() => new ColumnLocation(letter));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Theory]
            [InlineData("1")]
            [InlineData("A1")]
            [InlineData("A,")]
            [InlineData("A()")]
            [InlineData("*A*")]
            public void ThrowsArgumentExceptionWhenLetterIsNonAlphabetic(string letter)
            {
                // Act
                var exception = Record.Exception(() => new ColumnLocation(letter));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenLetterIsNull()
            {
                // Arrange
                var letter = default(string?);

                // Act
                var exception = Record.Exception(() => new ColumnLocation(letter!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }
    }
}
