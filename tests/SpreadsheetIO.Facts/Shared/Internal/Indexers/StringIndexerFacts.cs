using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal.Indexers
{
    public class StringIndexerFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private StringIndexer CreateSystemUnderTest()
            => _mocker.CreateInstance<StringIndexer>();

        public class TheResourcesProperty : StringIndexerFacts
        {
            [Fact]
            public void ReturnsIndexedResources()
            {
                // Arrange
                var firstString = "foo";
                var secondString = "bar";

                var sut = CreateSystemUnderTest();
                sut.Add(firstString);
                sut.Add(secondString);

                // Act
                var strings = sut.Resources;

                // Assert
                Assert.Equal(2, strings.Count);
                Assert.Single(strings, s => s == firstString);
                Assert.Single(strings, s => s == secondString);
            }
        }

        public class TheResourceIndexer : StringIndexerFacts
        {
            [Fact]
            public void ReturnsIndexForString()
            {
                // Arrange
                var resource = "foo";
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(resource);

                // Act
                var actualIndex = sut[resource];

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void ThrowsArgumentExceptionWhenStringIsNullOrEmpty(string resource)
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[resource]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenStringHasNotBeenIndexed()
            {
                // Arrange
                var resource = "foo";
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[resource]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }

        public class TheAddMethod : StringIndexerFacts
        {
            [Fact]
            public void SkipsIndexingWhenStringIsAlreadyIndexed()
            {
                // Arrange
                var resource = "foo";
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(resource);

                // Act
                var actualIndex = sut.Add(resource);

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }
        }

        public class TheClearMethod : StringIndexerFacts
        {
            [Fact]
            public void ClearsStrings()
            {
                // Arrange
                var firstString = "foo";
                var secondString = "bar";

                var sut = CreateSystemUnderTest();
                sut.Add(firstString);
                sut.Add(secondString);

                // Act
                sut.Clear();
                var firstException = Record.Exception(() => sut[firstString]);
                var secondException = Record.Exception(() => sut[secondString]);

                // Assert
                Assert.NotNull(firstException);
                Assert.IsType<KeyNotFoundException>(firstException);
                Assert.NotNull(secondException);
                Assert.IsType<KeyNotFoundException>(secondException);
            }
        }
    }
}
