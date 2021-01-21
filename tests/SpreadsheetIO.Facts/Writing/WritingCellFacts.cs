using System;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Writing;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing
{
    public class WritingCellFacts
    {
        public class TheConstructorWithValueParameter
        {
            [Fact]
            public void SetsValueProperty()
            {
                // Arrange
                var cellValue = new WritingCellValue(default(string?));

                // Act
                var cell = new WritingCell(cellValue);

                // Assert
                Assert.Equal(cellValue, cell.Value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                // Arrange
                var cellValue = default(WritingCellValue);

                // Act
                var exception = Record.Exception(() => new WritingCell(cellValue!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheConstructorWithValueAndStyleParameters
        {
            [Fact]
            public void SetsValueAndStyleProperties()
            {
                // Arrange
                var cellValue = new WritingCellValue(default(string?));
                var cellStyle = new WritingCellStyle(BuiltInExcelStyle.Normal);

                // Act
                var cell = new WritingCell(cellValue, cellStyle);

                // Assert
                Assert.Equal(cellValue, cell.Value);
                Assert.Equal(cellStyle, cell.Style);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                // Arrange
                var cellValue = default(WritingCellValue);
                var cellStyle = new WritingCellStyle(BuiltInExcelStyle.Normal);

                // Act
                var exception = Record.Exception(() => new WritingCell(cellValue!, cellStyle));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenStyleIsNull()
            {
                // Arrange
                var cellValue = new WritingCellValue(default(string?));
                var cellStyle = default(WritingCellStyle);

                // Act
                var exception = Record.Exception(() => new WritingCell(cellValue, cellStyle!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }
    }
}
