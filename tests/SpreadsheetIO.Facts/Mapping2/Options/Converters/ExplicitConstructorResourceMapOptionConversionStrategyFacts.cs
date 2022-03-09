using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Options.Converters;

public class ExplicitConstructorResourceMapOptionConversionStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private ExplicitConstructorResourceMapOptionConversionStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<ExplicitConstructorResourceMapOptionConversionStrategy>();

    public class TheConvertToOptionMethod : ExplicitConstructorResourceMapOptionConversionStrategyFacts
    {
        [Fact]
        public void ReturnsSuccessResultWhenConstructorIsFoundForMappedPropertyNames()
        {
            // Arrange
            var registration = new ExplicitConstructorResourceMapOptionRegistration(
                nameof(FakeConstructionModel.Id),
                nameof(FakeConstructionModel.Name),
                nameof(FakeConstructionModel.Amount));
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
        public void ReturnsFailureResultWhenPropertyNamesAreUnmapped()
        {
            // Arrange
            var registration = new ExplicitConstructorResourceMapOptionRegistration(
                nameof(FakeConstructionModel.Id),
                nameof(FakeConstructionModel.Name),
                nameof(FakeConstructionModel.Amount));
            var resourceMapBuilder = new ResourceMapBuilder<FakeConstructionModel>();
            resourceMapBuilder.Property(model => model.Id);

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.False(optionConversionResult.IsValid);
            Assert.Equal(registration, optionConversionResult.Registration);
            Assert.Null(optionConversionResult.Option);
            Assert.Equal(
                Messages.MissingMapForResourceProperties(typeof(FakeConstructionModel).Name, "Name,Amount"),
                optionConversionResult.Message);
        }

        [Fact]
        public void ReturnsFailureResultWhenConstructorIsNotFoundForMappedPropertyNames()
        {
            // Arrange
            var registration = new ExplicitConstructorResourceMapOptionRegistration(nameof(FakeConstructionModel.Id));
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
            var registration = default(ExplicitConstructorResourceMapOptionRegistration);
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
            var registration = new ExplicitConstructorResourceMapOptionRegistration("foo", "bar");
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
