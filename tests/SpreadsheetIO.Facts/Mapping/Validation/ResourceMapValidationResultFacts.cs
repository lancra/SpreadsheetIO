using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation;

public class ResourceMapValidationResultFacts
{
    public class TheSuccessMethod : ResourceMapValidationResultFacts
    {
        [Fact]
        public void ReturnsSuccessValidationResult()
        {
            // Act
            var validationResult = ResourceMapValidationResult.Success();

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }
    }

    public class TheFailureMethod : ResourceMapValidationResultFacts
    {
        [Fact]
        public void ReturnsFailureValidationResult()
        {
            // Arrange
            var message = "FooBar";

            // Act
            var validationResult = ResourceMapValidationResult.Failure(message);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(message, validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }
    }

    public class TheAggregateMethod : ResourceMapValidationResultFacts
    {
        [Fact]
        public void ReturnsSuccessValidationResultForNullValidationResultCollection()
        {
            // Arrange
            var validationResults = default(IEnumerable<ResourceMapValidationResult>);

            // Act
            var aggregateValidationResult = ResourceMapValidationResult.Aggregate(validationResults!);

            // Assert
            Assert.True(aggregateValidationResult.IsValid);
            Assert.Empty(aggregateValidationResult.Message);
            Assert.Empty(aggregateValidationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsSuccessValidationResultForEmptyValidationResultCollection()
        {
            // Arrange
            var validationResults = Array.Empty<ResourceMapValidationResult>();

            // Act
            var aggregateValidationResult = ResourceMapValidationResult.Aggregate(validationResults);

            // Assert
            Assert.True(aggregateValidationResult.IsValid);
            Assert.Empty(aggregateValidationResult.Message);
            Assert.Empty(aggregateValidationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsSuccessValidationResultForSuccessOnlyValidationResultCollection()
        {
            // Arrange
            var validationResults = new[]
            {
                ResourceMapValidationResult.Success(),
                ResourceMapValidationResult.Success(),
            };

            // Act
            var aggregateValidationResult = ResourceMapValidationResult.Aggregate(validationResults);

            // Assert
            Assert.True(aggregateValidationResult.IsValid);
            Assert.Empty(aggregateValidationResult.Message);
            Assert.Empty(aggregateValidationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsFailureValidationResultForSuccessAndFailureValidationResultCollection()
        {
            // Arrange
            var failureValidationResult = ResourceMapValidationResult.Failure("Foo");
            var validationResults = new[]
            {
                ResourceMapValidationResult.Success(),
                failureValidationResult,
            };

            // Act
            var aggregateValidationResult = ResourceMapValidationResult.Aggregate(validationResults);

            // Assert
            Assert.False(aggregateValidationResult.IsValid);
            Assert.Equal(Messages.FailedResourceMapValidation, aggregateValidationResult.Message);

            Assert.Equal(1, aggregateValidationResult.InnerValidationResults.Count);
            Assert.Single(
                aggregateValidationResult.InnerValidationResults,
                validationResult => validationResult == failureValidationResult);
        }

        [Fact]
        public void ReturnsFailureValidationResultForFailureOnlyValidationResultCollection()
        {
            // Arrange
            var firstFailureValidationResult = ResourceMapValidationResult.Failure("Foo");
            var secondFailureValidationResult = ResourceMapValidationResult.Failure("Bar");
            var validationResults = new[]
            {
                firstFailureValidationResult,
                secondFailureValidationResult,
            };

            // Act
            var aggregateValidationResult = ResourceMapValidationResult.Aggregate(validationResults);

            // Assert
            Assert.False(aggregateValidationResult.IsValid);
            Assert.Equal(Messages.FailedResourceMapValidation, aggregateValidationResult.Message);

            Assert.Equal(2, aggregateValidationResult.InnerValidationResults.Count);
            Assert.Single(
                aggregateValidationResult.InnerValidationResults,
                validationResult => validationResult == firstFailureValidationResult);
            Assert.Single(
                aggregateValidationResult.InnerValidationResults,
                validationResult => validationResult == secondFailureValidationResult);
        }
    }
}
