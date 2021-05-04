using System.Collections.Generic;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Indexers
{
    public class NumericFormatIndexerFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private NumericFormatIndexer CreateSystemUnderTest()
            => _mocker.CreateInstance<NumericFormatIndexer>();

        public class TheResourcesProperty : NumericFormatIndexerFacts
        {
            [Fact]
            public void ReturnsIndexedResources()
            {
                // Arrange
                var firstNumericFormat = new NumericFormat("#,##0.0000");
                var secondNumericFormat = new NumericFormat(@"#,##0.0000;\(#,##0.0000\)");

                var sut = CreateSystemUnderTest();
                sut.Add(firstNumericFormat);
                sut.Add(secondNumericFormat);

                // Act
                var numericFormats = sut.Resources;

                // Assert
                Assert.Equal(2, numericFormats.Count);
                Assert.Single(numericFormats, numericFormat => numericFormat == firstNumericFormat);
                Assert.Single(numericFormats, numericFormat => numericFormat == secondNumericFormat);
            }
        }

        public class TheNonBuiltInCountProperty : NumericFormatIndexerFacts
        {
            [Fact]
            public void ReturnsNumberOfNonBuiltInIndexedResources()
            {
                // Arrange
                var firstBuiltInNumericFormat = new NumericFormat("@");
                var secondBuiltInNumericFormat = new NumericFormat("0");
                var firstNumericFormat = new NumericFormat("#,##0.0000");
                var secondNumericFormat = new NumericFormat(@"#,##0.0000;\(#,##0.0000\)");

                var sut = CreateSystemUnderTest();
                sut.Add(firstBuiltInNumericFormat);
                sut.Add(secondBuiltInNumericFormat);
                sut.Add(firstNumericFormat);
                sut.Add(secondNumericFormat);

                // Act
                var nonBuiltInCount = sut.NonBuiltInCount;

                // Assert
                Assert.Equal(2U, nonBuiltInCount);
            }
        }

        public class TheResourceIndexer : NumericFormatIndexerFacts
        {
            [Fact]
            public void ReturnsIndexForNumericFormat()
            {
                // Arrange
                var numericFormat = new NumericFormat("#,##0.0000");
                var sut = CreateSystemUnderTest();

                var expectedIndex = sut.Add(numericFormat);

                // Act
                var actualIndex = sut[numericFormat];

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenBorderHasNotBeenIndexed()
            {
                // Arrange
                var numericFormat = new NumericFormat("#,##0.0000");
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[numericFormat]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }

        public class TheAddMethod : NumericFormatIndexerFacts
        {
            [Fact]
            public void SkipsIndexingWhenNumericFormatIsAlreadyIndexed()
            {
                // Arrange
                var numericFormat = new NumericFormat("#,##0.0000");
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(numericFormat);

                // Act
                var actualIndex = sut.Add(numericFormat);

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }
        }

        public class TheClearMethod : NumericFormatIndexerFacts
        {
            [Fact]
            public void ClearsNumericFormats()
            {
                // Arrange
                var numericFormat = new NumericFormat("#,##0.0000");

                var sut = CreateSystemUnderTest();
                sut.Add(numericFormat);

                // Act
                sut.Clear();
                var exception = Record.Exception(() => sut[numericFormat]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }
    }
}
