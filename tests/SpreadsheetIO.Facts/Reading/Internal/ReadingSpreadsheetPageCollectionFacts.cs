using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal
{
    public class ReadingSpreadsheetPageCollectionFacts
    {
        private readonly AutoMocker _mocker = new();

        private ReadingSpreadsheetPageCollection CreateSystemUnderTest()
            => _mocker.CreateInstance<ReadingSpreadsheetPageCollection>();

        public class TheCountProperty : ReadingSpreadsheetPageCollectionFacts
        {
            [Fact]
            public void ReturnsTheNumberOfAddedPages()
            {
                // Arrange
                var firstPage = new Mock<IReadingSpreadsheetPage>();
                var secondPage = new Mock<IReadingSpreadsheetPage>();

                var sut = CreateSystemUnderTest();
                sut.Add(firstPage.Object, "Foo");
                sut.Add(secondPage.Object, "Bar");

                // Act
                var count = sut.Count;

                // Assert
                Assert.Equal(2, count);
            }

            [Fact]
            public void ReturnsZeroWhenNoPagesHaveBeenAdded()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var count = sut.Count;

                // Assert
                Assert.Equal(0, count);
            }
        }

        public class TheIndexerByIndex : ReadingSpreadsheetPageCollectionFacts
        {
            [Fact]
            public void ReturnsThePageAtTheSpecifiedIndex()
            {
                // Arrange
                var notExpectedPage = new Mock<IReadingSpreadsheetPage>();
                var expectedPage = new Mock<IReadingSpreadsheetPage>();

                var sut = CreateSystemUnderTest();
                sut.Add(notExpectedPage.Object, "Foo");
                sut.Add(expectedPage.Object, "Bar");

                // Act
                var actualPage = sut[1];

                // Assert
                Assert.Equal(expectedPage.Object, actualPage);
            }

            [Fact]
            public void ThrowsArgumentOutOfRangeExceptionWhenIndexIsInvalid()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[int.MaxValue]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentOutOfRangeException>(exception);
            }
        }

        public class TheIndexerByName : ReadingSpreadsheetPageCollectionFacts
        {
            [Fact]
            public void ReturnsThePageWithTheSpecifiedName()
            {
                // Arrange
                var notExpectedName = "Foo";
                var notExpectedPage = new Mock<IReadingSpreadsheetPage>();

                var expectedName = "Bar";
                var expectedPage = new Mock<IReadingSpreadsheetPage>();

                var sut = CreateSystemUnderTest();
                sut.Add(notExpectedPage.Object, notExpectedName);
                sut.Add(expectedPage.Object, expectedName);

                // Act
                var actualPage = sut[expectedName];

                // Assert
                Assert.Equal(expectedPage.Object, actualPage);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenNoMatchingPageIsFound()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut["Baz"]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }
    }
}
