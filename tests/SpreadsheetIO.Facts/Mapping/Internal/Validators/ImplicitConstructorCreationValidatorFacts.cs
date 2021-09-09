using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping.Internal.Validators;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Internal.Validators
{
    public class ImplicitConstructorCreationValidatorFacts
    {
        private readonly AutoMocker _mocker = new();

        private ImplicitConstructorCreationValidator CreateSystemUnderTest()
            => _mocker.CreateInstance<ImplicitConstructorCreationValidator>();

        public class TheValidateMethod : ImplicitConstructorCreationValidatorFacts
        {
            [Fact]
            public void ReturnsSuccessValidationResultWhenConstructorExistsForMappedProperties()
            {
                // Arrange
                var map = new FakeConstructionModelMap(options => options.UseImplicitConstructor());
                map.Map(model => model.Id);
                map.Map(model => model.Name);
                map.Map(model => model.Amount);

                var sut = CreateSystemUnderTest();

                // Act
                var validationResult = sut.Validate(map);

                // Assert
                Assert.True(validationResult.IsValid);
                Assert.Empty(validationResult.Message);
                Assert.Empty(validationResult.InnerValidationResults);
            }

            [Fact]
            public void ReturnsFailureValidationResultWhenNoConstructorExistsForMappedProperties()
            {
                // Arrange
                var map = new FakeConstructionModelMap(options => options.UseImplicitConstructor());
                map.Map(model => model.Id);

                var sut = CreateSystemUnderTest();

                // Act
                var validationResult = sut.Validate(map);

                // Assert
                Assert.False(validationResult.IsValid);
                Assert.Equal(Messages.MissingResourceConstructor(typeof(FakeConstructionModel).Name), validationResult.Message);
                Assert.Empty(validationResult.InnerValidationResults);
            }
        }
    }
}
