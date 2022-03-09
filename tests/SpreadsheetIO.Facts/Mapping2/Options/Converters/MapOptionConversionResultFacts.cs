using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Options.Converters;

public class MapOptionConversionResultFacts
{
    public class TheSuccessMethod : MapOptionConversionResultFacts
    {
        [Fact]
        public void CreatesValidResult()
        {
            // Arrange
            var registration = new OptionalPropertyMapOption(PropertyElementKind.All);

            // Act
            var result = MapOptionConversionResult.Success(registration, registration);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(registration, result.Registration);
            Assert.Equal(registration, result.Option);
            Assert.Equal(registration, ((MapOptionConversionResult)result).Option);
            Assert.Empty(result.Message);
        }
    }

    public class TheSkippedMethod : MapOptionConversionResultFacts
    {
        [Fact]
        public void CreatesValidResult()
        {
            // Arrange
            var registration = new OptionalPropertyMapOption(PropertyElementKind.All);

            // Act
            var result = MapOptionConversionResult.Skipped<OptionalPropertyMapOption>(registration);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(registration, result.Registration);
            Assert.Null(result.Option);
            Assert.Empty(result.Message);
        }
    }

    public class TheFailureMethod : MapOptionConversionResultFacts
    {
        [Fact]
        public void CreatesInvalidResult()
        {
            // Arrange
            var registration = new OptionalPropertyMapOption(PropertyElementKind.All);
            var message = "foo";

            // Act
            var result = MapOptionConversionResult.Failure<OptionalPropertyMapOption>(registration, message);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(registration, result.Registration);
            Assert.Null(result.Option);
            Assert.Equal(message, result.Message);
        }
    }
}
