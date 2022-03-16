using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Options.Converters;

public class ResourceMapOptionConversionStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption> CreateSystemUnderTest()
        => _mocker.CreateInstance<FakeResourceMapOptionConversionStrategy>();

    public class TheConvertToOptionMethod : ResourceMapOptionConversionStrategyFacts
    {
        [Fact]
        public void CallsGenericConversionMethod()
        {
            // Arrange
            var registration = new FakeResourceMapOptionRegistration();
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            var sut = CreateSystemUnderTest();

            // Act
            sut.ConvertToOption(registration, resourceMapBuilderMock.Object);

            // Assert
            Assert.True(((FakeResourceMapOptionConversionStrategy)sut).RanConversion);
        }

        [Fact]
        public void ThrowsInvalidCastExceptionWhenRegistrationIsNotExpectedType()
        {
            // Arrange
            var registration = new FakeOtherResourceMapOptionRegistration();
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ConvertToOption(registration, resourceMapBuilderMock.Object));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidCastException>(exception);
        }
    }
}
