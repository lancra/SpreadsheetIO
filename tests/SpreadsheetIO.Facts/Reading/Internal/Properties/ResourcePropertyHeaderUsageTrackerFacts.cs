using System.Collections.Generic;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Properties
{
    public class ResourcePropertyHeaderUsageTrackerFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private ResourcePropertyHeaderUsageTracker CreateSystemUnderTest()
            => _mocker.CreateInstance<ResourcePropertyHeaderUsageTracker>();

        public class TheGetUnusedColumnNumbersMethod : ResourcePropertyHeaderUsageTrackerFacts
        {
            [Fact]
            public void ReturnsAllProvidedColumnNumbersAfterInitialization()
            {
                // Arrange
                var expectedColumnNumbers = new[]
                {
                    1U,
                    2U,
                    3U,
                };

                _mocker.Use<IEnumerable<uint>>(expectedColumnNumbers);

                var sut = CreateSystemUnderTest();

                // Act
                var actualColumnNumbers = sut.GetUnusedColumnNumbers();

                // Assert
                Assert.Equal(expectedColumnNumbers, actualColumnNumbers);
            }
        }

        public class TheMarkAsUsedMethod : ResourcePropertyHeaderUsageTrackerFacts
        {
            [Fact]
            public void MarksColumnNumberAsUsed()
            {
                // Arrange
                var columnNumbers = new[]
                {
                    1U,
                    2U,
                    3U,
                };

                _mocker.Use<IEnumerable<uint>>(columnNumbers);

                var sut = CreateSystemUnderTest();

                // Act
                sut.MarkAsUsed(1U);

                // Assert
                var unusedColumnNumbers = sut.GetUnusedColumnNumbers();
                Assert.Equal(2, unusedColumnNumbers.Count);
                Assert.Single(unusedColumnNumbers, columnNumber => columnNumber == 2U);
                Assert.Single(unusedColumnNumbers, columnNumber => columnNumber == 3U);
            }
        }
    }
}
