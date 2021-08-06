using System.Collections.Generic;
using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Indexers
{
    public class FillIndexerFacts
    {
        private readonly AutoMocker _mocker = new();

        private FillIndexer CreateSystemUnderTest()
            => _mocker.CreateInstance<FillIndexer>();

        public class TheResourcesProperty : FillIndexerFacts
        {
            [Fact]
            public void ReturnsIndexedResources()
            {
                // Arrange
                var firstDefaultFill = Fill.Default;
                var secondDefaultFill = Fill.Default with { Kind = FillKind.Gray125, };
                var nonDefaultFill = new Fill(FillKind.Solid, Color.Black);

                var sut = CreateSystemUnderTest();
                sut.Add(firstDefaultFill);
                sut.Add(secondDefaultFill);
                sut.Add(nonDefaultFill);

                // Act
                var fills = sut.Resources;

                // Assert
                Assert.Equal(3, fills.Count);
                Assert.Single(fills, fill => fill == firstDefaultFill);
                Assert.Single(fills, fill => fill == secondDefaultFill);
                Assert.Single(fills, fill => fill == nonDefaultFill);
            }
        }

        public class TheResourceIndexer : FillIndexerFacts
        {
            [Fact]
            public void ReturnsIndexForFill()
            {
                // Arrange
                var fill = new Fill(FillKind.Solid, Color.Black);
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(fill);

                // Act
                var actualIndex = sut[fill];

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenFillHasNotBeenIndexed()
            {
                // Arrange
                var fill = new Fill(FillKind.Solid, Color.Black);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[fill]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }

        public class TheAddMethod : FillIndexerFacts
        {
            [Fact]
            public void SkipsIndexingWhenFillIsAlreadyIndexed()
            {
                // Arrange
                var fill = new Fill(FillKind.Solid, Color.Black);
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(fill);

                // Act
                var actualIndex = sut.Add(fill);

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }
        }

        public class TheClearMethod : FillIndexerFacts
        {
            [Fact]
            public void ClearsNonDefaultFills()
            {
                // Arrange
                var firstDefaultFill = Fill.Default;
                var secondDefaultFill = Fill.Default with { Kind = FillKind.Gray125, };
                var nonDefaultFill = new Fill(FillKind.Solid, Color.Black);

                var sut = CreateSystemUnderTest();
                sut.Add(nonDefaultFill);

                // Act
                sut.Clear();
                var firstDefaultException = Record.Exception(() => sut[firstDefaultFill]);
                var secondDefaultException = Record.Exception(() => sut[secondDefaultFill]);
                var nonDefaultException = Record.Exception(() => sut[nonDefaultFill]);

                // Assert
                Assert.Null(firstDefaultException);
                Assert.Null(secondDefaultException);
                Assert.NotNull(nonDefaultException);
                Assert.IsType<KeyNotFoundException>(nonDefaultException);
            }
        }
    }
}
