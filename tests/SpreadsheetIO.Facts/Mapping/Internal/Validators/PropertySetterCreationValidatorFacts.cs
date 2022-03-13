using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping.Internal.Validators;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Internal.Validators;

public class PropertySetterCreationValidatorFacts
{
    private readonly AutoMocker _mocker = new();

    private PropertySetterCreationValidator CreateSystemUnderTest()
        => _mocker.CreateInstance<PropertySetterCreationValidator>();

    public class TheCanValidateMethod : PropertySetterCreationValidatorFacts
    {
        [Fact]
        public void ReturnsTrueWhenNoConstructorExtensionIsConfigured()
        {
            // Arrange
            var map = new FakePropertySetterModelMap();
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(map);

            // Assert
            Assert.True(canValidate);
        }

        [Fact]
        public void ReturnsFalseWhenExplicitConstructorIsConfigured()
        {
            // Arrange
            var map = new FakePropertySetterModelMap(options => options.UseExplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(map);

            // Assert
            Assert.False(canValidate);
        }

        [Fact]
        public void ReturnsFalseWhenImplicitConstructorIsConfigured()
        {
            // Arrange
            var map = new FakePropertySetterModelMap(options => options.UseImplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(map);

            // Assert
            Assert.False(canValidate);
        }
    }

    public class TheValidateMethod : PropertySetterCreationValidatorFacts
    {
        [Fact]
        public void ReturnsSuccessValidationResultWhenAllMappedPropertiesHavePublicSetter()
        {
            // Arrange
            var map = new FakePropertySetterModelMap();
            map.Map(model => model.Id);
            map.Map(model => model.Name);

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsFailureValidationResultWhenAnyMappedPropertiesDoesNotHavePublicSetter()
        {
            // Arrange
            var map = new FakePropertySetterModelMap();
            map.Map(model => model.Id);
            map.Map(model => model.Name);
            map.Map(model => model.Amount);

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(
                Messages.InvalidPropertiesForSetterCreation(
                    typeof(FakePropertySetterModel).Name,
                    nameof(FakePropertySetterModel.Amount)),
                validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }
    }
}
