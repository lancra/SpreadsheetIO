using LanceC.SpreadsheetIO.Mapping2.Validation;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Validation;

public class ResourceMapBuilderValidationResultFacts
{
    public class TheSuccessMethod : ResourceMapBuilderValidationResultFacts
    {
        [Fact]
        public void CreatesValidResult()
        {
            // Act
            var result = ResourceMapBuilderValidationResult.Success();

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Message);
        }
    }

    public class TheFailureMethod : ResourceMapBuilderValidationResultFacts
    {
        [Fact]
        public void CreatesInvalidResult()
        {
            // Arrange
            var message = "foo";

            // Act
            var result = ResourceMapBuilderValidationResult.Failure(message);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(message, result.Message);
        }
    }
}
