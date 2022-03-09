using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Reading;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Options.Converters;

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
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>()
                .HasDefaultPropertyReadingResolutions(ResourcePropertyDefaultReadingResolution.Empty);

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilder);

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
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>()
                .HasDefaultPropertyReadingResolutions(ResourcePropertyDefaultReadingResolution.Empty);

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilder);

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
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();

            var sut = CreateSystemUnderTest();

            // Act
            var optionConversionResult = sut.ConvertToOption(registration, resourceMapBuilder);

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
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();

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
            var registration = new DefaultValuePropertyMapOptionRegistration("foo", ResourcePropertyDefaultReadingResolution.Missing);
            var resourceMapBuilder = default(ResourceMapBuilder<FakeModel>);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ConvertToOption(registration, resourceMapBuilder!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
