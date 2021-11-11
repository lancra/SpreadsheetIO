using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping.Internal.Validators;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Internal.Validators;

public class ExplicitConstructorCreationValidatorFacts
{
    private readonly AutoMocker _mocker = new();

    private ExplicitConstructorCreationValidator CreateSystemUnderTest()
        => _mocker.CreateInstance<ExplicitConstructorCreationValidator>();

    public class TheValidateMethod : ExplicitConstructorCreationValidatorFacts
    {
        [Fact]
        public void ReturnsSuccessValidationResultWhenConstructorExistsForAllSpecifiedPropertyNamesThatAreMapped()
        {
            // Arrange
            var map = new FakeConstructionModelMap(
                options => options.UseExplicitConstructor(
                    nameof(FakeConstructionModel.Id),
                    nameof(FakeConstructionModel.Name),
                    nameof(FakeConstructionModel.Amount)));
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
        public void ReturnsFailureValidationResultWhenSpecifiedPropertyNamesAreUnmapped()
        {
            // Arrange
            var map = new FakeConstructionModelMap(
                options => options.UseExplicitConstructor(
                    nameof(FakeConstructionModel.Id),
                    nameof(FakeConstructionModel.Name),
                    nameof(FakeConstructionModel.Amount)));
            map.Map(model => model.Id);

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(map);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(
                Messages.MissingMapForResourceProperties(typeof(FakeConstructionModel).Name, "Name,Amount"),
                validationResult.Message);
            Assert.Empty(validationResult.InnerValidationResults);
        }

        [Fact]
        public void ReturnsFailureValidationResultWhenNoConstructorExistsForSpecifiedPropertyNamesThatAreMapped()
        {
            // Arrange
            var map = new FakeConstructionModelMap(options => options.UseExplicitConstructor(nameof(FakeConstructionModel.Id)));
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
