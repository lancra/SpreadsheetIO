using System;
using LanceC.SpreadsheetIO.Shared.Internal;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal
{
    public class CellLocationFacts
    {
        public class TheConstructorWithColumnNumberParameter
        {
            [Fact]
            public void SetsRowNumberAndColumnProperties()
            {
                // Arrange
                var rowNumber = 1U;
                var columnNumber = 1U;

                // Act
                var cellLocation = new CellLocation(rowNumber, columnNumber);

                // Assert
                Assert.Equal(rowNumber, cellLocation.RowNumber);
                Assert.Equal(columnNumber, cellLocation.Column.Number);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenRowNumberIsZero()
            {
                // Arrange
                var rowNumber = 0U;
                var columnNumber = 1U;

                // Act
                var exception = Record.Exception(() => new CellLocation(rowNumber, columnNumber));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenColumnNumberIsZero()
            {
                // Arrange
                var rowNumber = 1U;
                var columnNumber = 0U;

                // Act
                var exception = Record.Exception(() => new CellLocation(rowNumber, columnNumber));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }
        }

        public class TheConstructorWithColumnLetterParameter
        {
            [Fact]
            public void SetsRowNumberAndColumnProperties()
            {
                // Arrange
                var rowNumber = 1U;
                var columnLetter = "A";

                // Act
                var cellLocation = new CellLocation(rowNumber, columnLetter);

                // Assert
                Assert.Equal(rowNumber, cellLocation.RowNumber);
                Assert.Equal(columnLetter, cellLocation.Column.Letter);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenRowNumberIsZero()
            {
                // Arrange
                var rowNumber = 0U;
                var columnLetter = "A";

                // Act
                var exception = Record.Exception(() => new CellLocation(rowNumber, columnLetter));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenColumnLetterIsEmpty()
            {
                // Arrange
                var rowNumber = 1U;
                var columnLetter = string.Empty;

                // Act
                var exception = Record.Exception(() => new CellLocation(rowNumber, columnLetter));

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
            public void ThrowsArgumentExceptionWhenColumnLetterIsNonAlphabetic(string columnLetter)
            {
                // Arrange
                var rowNumber = 1U;

                // Act
                var exception = Record.Exception(() => new CellLocation(rowNumber, columnLetter));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenColumnLetterIsNull()
            {
                // Arrange
                var rowNumber = 1U;
                var columnLetter = default(string?);

                // Act
                var exception = Record.Exception(() => new CellLocation(rowNumber, columnLetter!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheCellReferenceProperty
        {
            [Theory]
            [InlineData(1U, "A", "A1")]
            [InlineData(2U, "C", "C2")]
            [InlineData(11U, "AA", "AA11")]
            public void ReturnsCombinationOfColumnLetterAndRowNumber(uint rowNumber, string columnLetter, string expectedCellReference)
            {
                // Arrange
                var cellLocation = new CellLocation(rowNumber, columnLetter);

                // Act
                var actualCellReference = cellLocation.CellReference;

                // Assert
                Assert.Equal(expectedCellReference, actualCellReference);
            }
        }
    }
}
