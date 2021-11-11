using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Writing;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing;

public class WritingCellStyleFacts
{
    public class TheConstructorWithNameParameter
    {
        [Fact]
        public void SetsKeyProperty()
        {
            // Arrange
            var name = "Style";

            // Act
            var cellStyle = new WritingCellStyle(name);

            // Assert
            Assert.Equal(name, cellStyle.Key.Name);
            Assert.Equal(IndexerKeyKind.Custom, cellStyle.Key.Kind);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenNameIsEmpty()
        {
            // Arrange
            var name = string.Empty;

            // Act
            var exception = Record.Exception(() => new WritingCellStyle(name));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenNameIsNull()
        {
            // Arrange
            var name = default(string?);

            // Act
            var exception = Record.Exception(() => new WritingCellStyle(name!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheConstructorWithBuiltInExcelStyleParameter
    {
        [Fact]
        public void SetsKeyProperty()
        {
            // Arrange
            var style = BuiltInExcelStyle.Normal;

            // Act
            var cellStyle = new WritingCellStyle(style);

            // Assert
            Assert.Equal(style.IndexerKey, cellStyle.Key);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInExcelStyle);

            // Act
            var exception = Record.Exception(() => new WritingCellStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheConstructorWithBuiltInPackageStyleParameter
    {
        [Fact]
        public void SetsKeyProperty()
        {
            // Arrange
            var style = BuiltInPackageStyle.Bold;

            // Act
            var cellStyle = new WritingCellStyle(style);

            // Assert
            Assert.Equal(style.IndexerKey, cellStyle.Key);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInPackageStyle);

            // Act
            var exception = Record.Exception(() => new WritingCellStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
