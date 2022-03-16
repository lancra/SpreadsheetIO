using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Validation;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation;

public class ResourceMapBuilderValidatorFacts
{
    private readonly AutoMocker _mocker = new();

    private ResourceMapBuilderValidator CreateSystemUnderTest()
        => _mocker.CreateInstance<ResourceMapBuilderValidator>();

    public class TheValidateMethod : ResourceMapBuilderValidatorFacts
    {
        [Fact]
        public void ReturnsFailedValidationResultsFromAllStrategies()
        {
            // Arrange
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var strategyOne = new Mock<IResourceMapBuilderValidationStrategy>();
            strategyOne.Setup(strategy => strategy.Validate(resourceMapBuilderMock.Object))
                .Returns(ResourceMapBuilderValidationResult.Success());

            var expectedValidationResult = ResourceMapBuilderValidationResult.Failure("foo");
            var strategyTwo = new Mock<IResourceMapBuilderValidationStrategy>();
            strategyTwo.Setup(strategy => strategy.Validate(resourceMapBuilderMock.Object))
                .Returns(expectedValidationResult);

            _mocker.Use<IEnumerable<IResourceMapBuilderValidationStrategy>>(
                new[]
                {
                    strategyOne.Object,
                    strategyTwo.Object,
                });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResults = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            var actualValidationResult = Assert.Single(validationResults);
            Assert.Equal(expectedValidationResult, actualValidationResult);
        }
    }
}
