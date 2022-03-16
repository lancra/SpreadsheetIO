using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Reading;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Options.Converters;

public class DefaultValuePropertyMapOptionConversionStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private DefaultValuePropertyMapOptionConversionStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<DefaultValuePropertyMapOptionConversionStrategy>();

    public class TheConvertToOptionMethod : DefaultValuePropertyMapOptionConversionStrategyFacts
    {
        [Fact]
        public void ReturnsSuccessResult()
        {
            // Arrange
            var registration = new DefaultValuePropertyMapOptionRegistration("foo", ResourcePropertyDefaultReadingResolution.Missing);
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var resourceRegistration = new DefaultPropertyReadingResolutionsResourceMapOptionRegistration(
                ResourcePropertyDefaultReadingResolution.Empty);
            resourceMapBuilderMock.Setup(builder => builder.TryGetRegistration(out resourceRegistration))
                .Returns(true);

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilderMock.Object);

            // Assert
            Assert.True(optionConversionResult.IsValid);
            Assert.Equal(registration, optionConversionResult.Registration);
            Assert.Empty(optionConversionResult.Message);

            var option = Assert.IsType<DefaultValuePropertyMapOption>(optionConversionResult.Option);
            Assert.Equal(registration.Value, option.Value);
            var resolution = Assert.Single(option.Resolutions);
            Assert.Equal(ResourcePropertyDefaultReadingResolution.Missing, resolution);
        }

        [Fact]
        public void ReturnsSuccessResultWithResolutionsFromDefaultResolutionsResourceRegistrationWhenNoneDefinedForRegistration()
        {
            // Arrange
            var registration = new DefaultValuePropertyMapOptionRegistration("foo");
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var resourceRegistration = new DefaultPropertyReadingResolutionsResourceMapOptionRegistration(
                ResourcePropertyDefaultReadingResolution.Empty);
            resourceMapBuilderMock.Setup(builder => builder.TryGetRegistration(out resourceRegistration))
                .Returns(true);

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilderMock.Object);

            // Assert
            Assert.True(optionConversionResult.IsValid);
            Assert.Equal(registration, optionConversionResult.Registration);
            Assert.Empty(optionConversionResult.Message);

            var option = Assert.IsType<DefaultValuePropertyMapOption>(optionConversionResult.Option);
            Assert.Equal(registration.Value, option.Value);
            var resolution = Assert.Single(option.Resolutions);
            Assert.Equal(ResourcePropertyDefaultReadingResolution.Empty, resolution);
        }

        [Fact]
        public void ReturnsSuccessResultWithAllResolutionsWhenNoneAreRegistered()
        {
            // Arrange
            var registration = new DefaultValuePropertyMapOptionRegistration("foo");
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilderMock.Object);

            // Assert
            Assert.True(optionConversionResult.IsValid);
            Assert.Equal(registration, optionConversionResult.Registration);
            Assert.Empty(optionConversionResult.Message);

            var option = Assert.IsType<DefaultValuePropertyMapOption>(optionConversionResult.Option);
            Assert.Equal(registration.Value, option.Value);
            Assert.Equal(ResourcePropertyDefaultReadingResolution.List, option.Resolutions);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenRegistrationIsNull()
        {
            // Arrange
            var registration = default(DefaultValuePropertyMapOptionRegistration);
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
            var registration = new DefaultValuePropertyMapOptionRegistration("foo", ResourcePropertyDefaultReadingResolution.Missing);
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
