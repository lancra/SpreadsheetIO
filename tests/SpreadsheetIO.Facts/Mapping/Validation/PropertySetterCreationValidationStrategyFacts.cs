using LanceC.SpreadsheetIO.Facts.Testing.Extensions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation;

public class PropertySetterCreationValidationStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private PropertySetterCreationValidationStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<PropertySetterCreationValidationStrategy>();

    public class TheValidateMethod : PropertySetterCreationValidationStrategyFacts
    {
        [Fact]
        public void ReturnsSuccessResultWhenAllMappedPropertiesHavePublicSetters()
        {
            // Arrange
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(typeof(FakeModel), nameof(FakeModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(typeof(FakeModel), nameof(FakeModel.Name)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsSuccessResultWhenResourceMapSpecifiesImplicitConstructorCreation()
        {
            // Arrange
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var registration = new ImplicitConstructorResourceMapOptionRegistration();
            resourceMapBuilderMock.Setup(builder => builder.TryGetRegistration(out registration))
                .Returns(true);

            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakePropertySetterModel),
                            nameof(FakePropertySetterModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakePropertySetterModel),
                            nameof(FakePropertySetterModel.Amount)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsSuccessResultWhenResourceMapSpecifiesExplicitConstructorCreation()
        {
            // Arrange
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var registration = new ExplicitConstructorResourceMapOptionRegistration(new[] { "Foo", "Bar", });
            resourceMapBuilderMock.Setup(builder => builder.TryGetRegistration(out registration))
                .Returns(true);

            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakePropertySetterModel),
                            nameof(FakePropertySetterModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakePropertySetterModel),
                            nameof(FakePropertySetterModel.Amount)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsFailureResultWhenAnyMappedPropertyDoesNotHavePublicSetter()
        {
            // Arrange
            var resourceType = typeof(FakePropertySetterModel);

            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.ResourceType)
                .Returns(resourceType);
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakePropertySetterModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakePropertySetterModel.Amount)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(
                Messages.InvalidPropertiesForSetterCreation(resourceType, nameof(FakePropertySetterModel.Amount)),
                validationResult.Message);
        }
    }
}
