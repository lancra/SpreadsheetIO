using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping.Internal.Validators;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Internal.Validators;

public class SingleConstructorCreationValidatorFacts
{
    private readonly AutoMocker _mocker = new();

    private SingleConstructorCreationValidator CreateSystemUnderTest()
        => _mocker.CreateInstance<SingleConstructorCreationValidator>();

    public class TheCanValidateMethod : SingleConstructorCreationValidatorFacts
    {
        [Fact]
        public void ReturnsTrueWhenExplicitConstructorIsConfigured()
        {
            // Arrange
            var map = new FakeConstructionModelMap(options => options.UseExplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(map);

            // Assert
            Assert.True(canValidate);
        }

        [Fact]
        public void ReturnsTrueWhenImplicitConstructorIsConfigured()
        {
            // Arrange
            var map = new FakeConstructionModelMap(options => options.UseImplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(map);

            // Assert
            Assert.True(canValidate);
        }

        [Fact]
        public void ReturnsTrueWhenExplicitAndImplicitConstructorsAreConfigured()
        {
            // Arrange
            var map = new FakeConstructionModelMap(options => options.UseExplicitConstructor()
                .UseImplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(map);

            // Assert
            Assert.True(canValidate);
        }

        [Fact]
        public void ReturnsFalseWhenNoConstructorsAreConfigured()
        {
            // Arrange
            var map = new FakeConstructionModelMap();
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(map);

            // Assert
            Assert.False(canValidate);
        }
    }

    public class TheValidateMethod : SingleConstructorCreationValidatorFacts
    {
        [Fact]
        public void ReturnsSuccessValidationResultWhenOnlyExplicitConstructorIsConfigured()
        {
            // Arrange
            var map = new FakeConstructionModelMap(options => options.UseExplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsSuccessValidationResultWhenOnlyImplicitConstructorIsConfigured()
        {
            // Arrange
            var map = new FakeConstructionModelMap(options => options.UseImplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsFailureValidationResultWhenExplicitAndImplicitConstructorsAreConfigured()
        {
            // Arrange
            var map = new FakeConstructionModelMap(options => options.UseExplicitConstructor()
                .UseImplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(Messages.DuplicateResourceConstructor(typeof(FakeConstructionModel).Name), validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }
    }
}
