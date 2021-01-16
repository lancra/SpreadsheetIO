using System.Collections.Generic;
using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Indexers
{
    public class BorderIndexerFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private BorderIndexer CreateSystemUnderTest()
            => _mocker.CreateInstance<BorderIndexer>();

        public class TheResourcesProperty : BorderIndexerFacts
        {
            [Fact]
            public void ReturnsIndexedResources()
            {
                // Arrange
                var defaultBorder = Border.Default;
                var nonDefaultBorder = new Border(new BorderLine(Color.White, BorderLineKind.Thick));

                var sut = CreateSystemUnderTest();
                sut.Add(defaultBorder);
                sut.Add(nonDefaultBorder);

                // Act
                var borders = sut.Resources;

                // Assert
                Assert.Equal(2, borders.Count);
                Assert.Single(borders, border => border == defaultBorder);
                Assert.Single(borders, border => border == nonDefaultBorder);
            }
        }

        public class TheResourceIndexer : BorderIndexerFacts
        {
            [Fact]
            public void ReturnsIndexForBorder()
            {
                // Arrange
                var border = new Border(new BorderLine(Color.White, BorderLineKind.Thick));
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(border);

                // Act
                var actualIndex = sut[border];

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenBorderHasNotBeenIndexed()
            {
                // Arrange
                var border = new Border(new BorderLine(Color.White, BorderLineKind.Thick));
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[border]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }

        public class TheAddMethod : BorderIndexerFacts
        {
            [Fact]
            public void SkipsIndexingWhenBorderIsAlreadyIndexed()
            {
                // Arrange
                var border = new Border(new BorderLine(Color.White, BorderLineKind.Thick));
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(border);

                // Act
                var actualIndex = sut.Add(border);

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }
        }

        public class TheClearMethod : BorderIndexerFacts
        {
            [Fact]
            public void ClearsNonDefaultBorders()
            {
                // Arrange
                var defaultBorder = Border.Default;
                var nonDefaultBorder = new Border(new BorderLine(Color.White, BorderLineKind.Thick));

                var sut = CreateSystemUnderTest();
                sut.Add(nonDefaultBorder);

                // Act
                sut.Clear();
                var defaultException = Record.Exception(() => sut[defaultBorder]);
                var nonDefaultException = Record.Exception(() => sut[nonDefaultBorder]);

                // Assert
                Assert.Null(defaultException);
                Assert.NotNull(nonDefaultException);
                Assert.IsType<KeyNotFoundException>(nonDefaultException);
            }
        }
    }
}
