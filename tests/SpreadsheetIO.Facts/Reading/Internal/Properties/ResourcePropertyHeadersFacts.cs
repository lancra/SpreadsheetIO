using System.Collections.Generic;
using System.Linq;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Properties
{
    public class ResourcePropertyHeadersFacts
    {
        private readonly AutoMocker _mocker = new();

        private ResourcePropertyHeaders<FakeModel> CreateSystemUnderTest()
            => _mocker.CreateInstance<ResourcePropertyHeaders<FakeModel>>();

        public class TheColumnNumbersProperty : ResourcePropertyHeadersFacts
        {
            [Fact]
            public void ReturnsAddedColumnNumbers()
            {
                // Arrange
                var firstMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var secondMap = PropertyMapCreator.CreateForFakeModel(model => model.Name);
                var thirdMap = PropertyMapCreator.CreateForFakeModel(model => model.Decimal);

                var firstColumnNumber = 1U;
                var secondColumnNumber = 2U;
                var thirdColumnNumber = 3U;

                var sut = CreateSystemUnderTest();

                sut.Add(firstMap, firstColumnNumber);
                sut.Add(secondMap, secondColumnNumber);
                sut.Add(thirdMap, thirdColumnNumber);

                // Act
                var columnNumbers = sut.ColumnNumbers;

                // Assert
                Assert.Equal(3, columnNumbers.Count());
                Assert.Single(columnNumbers, columnNumber => columnNumber == firstColumnNumber);
                Assert.Single(columnNumbers, columnNumber => columnNumber == secondColumnNumber);
                Assert.Single(columnNumbers, columnNumber => columnNumber == thirdColumnNumber);
            }

            [Fact]
            public void ReturnsEmptyCollectionWhenNoHeadersHaveBeenAdded()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var columnNumbers = sut.ColumnNumbers;

                // Assert
                Assert.Empty(columnNumbers);
            }
        }

        public class TheAddMethod : ResourcePropertyHeadersFacts
        {
            [Fact]
            public void AddsMapByNumber()
            {
                // Arrange
                var expectedMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var columnNumber = 1U;

                var sut = CreateSystemUnderTest();

                // Act
                sut.Add(expectedMap, columnNumber);

                // Assert
                var actualMap = sut.GetMap(columnNumber);
                Assert.Equal(expectedMap, actualMap);
            }

            [Fact]
            public void DoesNotOverrideExistingMap()
            {
                // Arrange
                var expectedMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var notExpectedMap = PropertyMapCreator.CreateForFakeModel(model => model.Name);
                var columnNumber = 1U;

                var sut = CreateSystemUnderTest();
                sut.Add(expectedMap, columnNumber);

                // Act
                sut.Add(notExpectedMap, columnNumber);

                // Assert
                var actualMap = sut.GetMap(columnNumber);
                Assert.Equal(expectedMap, actualMap);
            }
        }

        public class TheContainsMapMethod : ResourcePropertyHeadersFacts
        {
            [Fact]
            public void ReturnsTrueWhenTheMapHasBeenAdded()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);

                var sut = CreateSystemUnderTest();
                sut.Add(map, 1U);

                // Act
                var containsMap = sut.ContainsMap(map);

                // Assert
                Assert.True(containsMap);
            }

            [Fact]
            public void ReturnsFalseWhenTheMapHasNotBeenAdded()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var sut = CreateSystemUnderTest();

                // Act
                var containsMap = sut.ContainsMap(map);

                // Assert
                Assert.False(containsMap);
            }
        }

        public class TheCreateUsageTrackerMethod : ResourcePropertyHeadersFacts
        {
            [Fact]
            public void ReturnsUsageTrackerContainingAllColumnNumbers()
            {
                // Arrange
                var firstMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var secondMap = PropertyMapCreator.CreateForFakeModel(model => model.Name);
                var thirdMap = PropertyMapCreator.CreateForFakeModel(model => model.Decimal);

                var firstColumnNumber = 1U;
                var secondColumnNumber = 2U;
                var thirdColumnNumber = 3U;

                var sut = CreateSystemUnderTest();

                sut.Add(firstMap, firstColumnNumber);
                sut.Add(secondMap, secondColumnNumber);
                sut.Add(thirdMap, thirdColumnNumber);

                // Act
                var usageTracker = sut.CreateUsageTracker();

                // Assert
                Assert.Equal(sut.ColumnNumbers, usageTracker.GetUnusedColumnNumbers());
            }
        }

        public class TheGetMapMethod : ResourcePropertyHeadersFacts
        {
            [Fact]
            public void ReturnsMapForColumnNumber()
            {
                // Arrange
                var expectedMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var columnNumber = 1U;

                var sut = CreateSystemUnderTest();
                sut.Add(expectedMap, columnNumber);

                // Act
                var actualMap = sut.GetMap(columnNumber);

                // Assert
                Assert.Equal(expectedMap, actualMap);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenNoMapIsFoundForColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.GetMap(1U));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }

        public class TheTryGetMapMethod : ResourcePropertyHeadersFacts
        {
            [Fact]
            public void ReturnsTrueWhenMapExistsForColumnNumber()
            {
                // Arrange
                var expectedMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var columnNumber = 1U;

                var sut = CreateSystemUnderTest();
                sut.Add(expectedMap, columnNumber);

                // Act
                var hasMap = sut.TryGetMap(columnNumber, out var actualMap);

                // Assert
                Assert.True(hasMap);
                Assert.Equal(expectedMap, actualMap);
            }

            [Fact]
            public void ReturnsFalseWhenNoMapExistsForColumnNumber()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var hasMap = sut.TryGetMap(1U, out var map);

                // Assert
                Assert.False(hasMap);
                Assert.Null(map);
            }
        }
    }
}
