using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping.Internal.Validators;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Internal.Validators;

public class ResourceMapAggregateValidatorFacts
{
    private readonly AutoMocker _mocker = new();

    private ResourceMapAggregateValidator CreateSystemUnderTest()
        => _mocker.CreateInstance<ResourceMapAggregateValidator>();

    public class TheValidateMethod : ResourceMapAggregateValidatorFacts
    {
        [Fact]
        public void ReturnsSuccessValidationResultWhenNoValidatorsAreAvailableToBeRun()
        {
            // Arrange
            var map = new FakeStringResourceMap();

            _mocker.Use<IEnumerable<IResourceMapValidator>>(Array.Empty<IResourceMapValidator>());
            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsSuccessValidationResultWhenValidatorsReturnNull()
        {
            // Arrange
            var map = new FakeStringResourceMap();

            var firstValidatorMock = new Mock<IResourceMapValidator>();
            firstValidatorMock.Setup(validator => validator.CanValidate(map))
                .Returns(true);
            firstValidatorMock.Setup(validator => validator.Validate(map))
                .Returns(default(ResourceMapValidationResult)!);

            var secondValidatorMock = new Mock<IResourceMapValidator>();
            secondValidatorMock.Setup(validator => validator.CanValidate(map))
                .Returns(false);

            _mocker.Use<IEnumerable<IResourceMapValidator>>(new[] { firstValidatorMock.Object, secondValidatorMock.Object, });
            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsSuccessValidationResultWhenValidatorsReturnSuccess()
        {
            // Arrange
            var map = new FakeStringResourceMap();

            var validatorMock = new Mock<IResourceMapValidator>();
            validatorMock.Setup(validator => validator.CanValidate(map))
                .Returns(true);
            validatorMock.Setup(validator => validator.Validate(map))
                .Returns(ResourceMapValidationResult.Success());

            _mocker.Use<IEnumerable<IResourceMapValidator>>(new[] { validatorMock.Object, });
            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsFailureValidationResultWhenValidatorsReturnAtLeastOneFailure()
        {
            // Arrange
            var map = new FakeStringResourceMap();

            var firstValidatorMock = new Mock<IResourceMapValidator>();
            firstValidatorMock.Setup(validator => validator.CanValidate(map))
                .Returns(true);
            firstValidatorMock.Setup(validator => validator.Validate(map))
                .Returns(default(ResourceMapValidationResult)!);

            var secondValidatorMock = new Mock<IResourceMapValidator>();
            secondValidatorMock.Setup(validator => validator.CanValidate(map))
                .Returns(false);

            var thirdValidatorMock = new Mock<IResourceMapValidator>();
            thirdValidatorMock.Setup(validator => validator.CanValidate(map))
                .Returns(true);
            thirdValidatorMock.Setup(validator => validator.Validate(map))
                .Returns(ResourceMapValidationResult.Success());

            var failureValidationResult = ResourceMapValidationResult.Failure("FooBar");
            var fourthValidatorMock = new Mock<IResourceMapValidator>();
            fourthValidatorMock.Setup(validator => validator.CanValidate(map))
                .Returns(true);
            fourthValidatorMock.Setup(validator => validator.Validate(map))
                .Returns(failureValidationResult);

            _mocker.Use<IEnumerable<IResourceMapValidator>>(
                new[]
                {
                        firstValidatorMock.Object,
                        secondValidatorMock.Object,
                        thirdValidatorMock.Object,
                        fourthValidatorMock.Object,
                });
            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(Messages.FailedResourceMapValidation, validationResult.Message);

            Assert.Equal(1, validationResult.InnerValidationResults.Count);
            Assert.Single(
                validationResult.InnerValidationResults,
                innerValidationResult => innerValidationResult == failureValidationResult);
        }
    }
}
