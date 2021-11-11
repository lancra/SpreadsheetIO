using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Indexers
{
    public class FontIndexerFacts
    {
        private readonly AutoMocker _mocker = new();

        private FontIndexer CreateSystemUnderTest()
            => _mocker.CreateInstance<FontIndexer>();

        public class TheResourcesProperty : FontIndexerFacts
        {
            [Fact]
            public void ReturnsIndexedResources()
            {
                // Arrange
                var defaultFont = Font.Default;
                var nonDefaultFont = new Font("Arial", 20D, Color.White, true, true);

                var sut = CreateSystemUnderTest();
                sut.Add(defaultFont);
                sut.Add(nonDefaultFont);

                // Act
                var fonts = sut.Resources;

                // Assert
                Assert.Equal(2, fonts.Count);
                Assert.Single(fonts, font => font == defaultFont);
                Assert.Single(fonts, font => font == nonDefaultFont);
            }
        }

        public class TheResourceIndexer : FontIndexerFacts
        {
            [Fact]
            public void ReturnsIndexForFont()
            {
                // Arrange
                var font = new Font("Arial", 20D, Color.White, true, true);
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(font);

                // Act
                var actualIndex = sut[font];

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenFontHasNotBeenIndexed()
            {
                // Arrange
                var font = new Font("Arial", 20D, Color.White, true, true);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[font]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }

        public class TheAddMethod : FontIndexerFacts
        {
            [Fact]
            public void SkipsIndexingWhenFontIsAlreadyIndexed()
            {
                // Arrange
                var font = new Font("Arial", 20D, Color.White, true, true);
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(font);

                // Act
                var actualIndex = sut.Add(font);

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }
        }

        public class TheClearMethod : FontIndexerFacts
        {
            [Fact]
            public void ClearsNonDefaultBorders()
            {
                // Arrange
                var defaultFont = Font.Default;
                var nonDefaultFont = new Font("Arial", 20D, Color.White, true, true);

                var sut = CreateSystemUnderTest();
                sut.Add(nonDefaultFont);

                // Act
                sut.Clear();
                var defaultException = Record.Exception(() => sut[defaultFont]);
                var nonDefaultException = Record.Exception(() => sut[nonDefaultFont]);

                // Assert
                Assert.Null(defaultException);
                Assert.NotNull(nonDefaultException);
                Assert.IsType<KeyNotFoundException>(nonDefaultException);
            }
        }
    }
}
