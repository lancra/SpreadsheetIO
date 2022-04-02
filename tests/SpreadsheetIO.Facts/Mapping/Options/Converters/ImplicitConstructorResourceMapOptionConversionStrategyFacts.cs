using LanceC.SpreadsheetIO.Facts.Testing.Extensions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Options.Converters;

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

            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.ResourceType)
                .Returns(typeof(FakeConstructionModel));
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakeConstructionModel),
                            nameof(FakeConstructionModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakeConstructionModel),
                            nameof(FakeConstructionModel.Name)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakeConstructionModel),
                            nameof(FakeConstructionModel.Amount)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilderMock.Object);

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

            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.ResourceType)
                .Returns(typeof(FakeConstructionModel));
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            typeof(FakeConstructionModel),
                            nameof(FakeConstructionModel.Id)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilderMock.Object);

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
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ConvertToOption(registration!, resourceMapBuilderMock.Object));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenResourceMapBuilderIsNull()
        {
            // Arrange
            var registration = new ImplicitConstructorResourceMapOptionRegistration();
            var resourceMapBuilder = default(IInternalResourceMapBuilder);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ConvertToOption(registration, resourceMapBuilder!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
