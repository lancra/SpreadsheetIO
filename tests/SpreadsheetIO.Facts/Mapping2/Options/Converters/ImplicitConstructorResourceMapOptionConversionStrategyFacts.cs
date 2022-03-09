using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Options.Converters;

public class ImplicitConstructorResourceMapOptionConversionStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private ImplicitConstructorResourceMapOptionConversionStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<ImplicitConstructorResourceMapOptionConversionStrategy>();

    public class TheConvertToOptionMethod : ImplicitConstructorResourceMapOptionConversionStrategyFacts
    {
        [Fact]
        public void ReturnsSuccessResultWhenConstructorIsFoundForMappedProperties()
        {
            // Arrange
            var registration = new ImplicitConstructorResourceMapOptionRegistration();
            var resourceMapBuilder = new ResourceMapBuilder<FakeConstructionModel>();
            resourceMapBuilder.Property(model => model.Id);
            resourceMapBuilder.Property(model => model.Name);
            resourceMapBuilder.Property(model => model.Amount);

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.True(optionConversionResult.IsValid);
            Assert.Equal(registration, optionConversionResult.Registration);
            Assert.Empty(optionConversionResult.Message);

            var option = Assert.IsType<ConstructorResourceMapOption>(optionConversionResult.Option);
            Assert.Equal(typeof(FakeConstructionModel), option.Constructor.DeclaringType);
            Assert.Single(option.PropertyKeys, key => key.Name == nameof(FakeConstructionModel.Id));
            Assert.Single(option.PropertyKeys, key => key.Name == nameof(FakeConstructionModel.Name));
            Assert.Single(option.PropertyKeys, key => key.Name == nameof(FakeConstructionModel.Amount));
        }

        [Fact]
        public void ReturnsFailureResultWhenConstructorIsNotFoundForMappedProperties()
        {
            // Arrange
            var registration = new ImplicitConstructorResourceMapOptionRegistration();
            var resourceMapBuilder = new ResourceMapBuilder<FakeConstructionModel>();
            resourceMapBuilder.Property(model => model.Id);

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.False(optionConversionResult.IsValid);
            Assert.Equal(registration, optionConversionResult.Registration);
            Assert.Null(optionConversionResult.Option);
            Assert.Equal(Messages.MissingResourceConstructor(typeof(FakeConstructionModel).Name), optionConversionResult.Message);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenRegistrationIsNull()
        {
            // Arrange
            var registration = default(ImplicitConstructorResourceMapOptionRegistration);
            var resourceMapBuilder = new ResourceMapBuilder<FakeConstructionModel>();

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ConvertToOption(registration!, resourceMapBuilder));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenResourceMapBuilderIsNull()
        {
            // Arrange
            var registration = new ImplicitConstructorResourceMapOptionRegistration();
            var resourceMapBuilder = default(ResourceMapBuilder<FakeConstructionModel>);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ConvertToOption(registration, resourceMapBuilder!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
