using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Options.Converters;

public class MapOptionConverterFacts
{
    private readonly AutoMocker _mocker = new();

    private static Mock<IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>> MockStrategy(
        Type registrationType)
    {
        var strategyMock = new Mock<IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>>();
        strategyMock.SetupGet(strategy => strategy.RegistrationType)
            .Returns(registrationType);

        return strategyMock;
    }

    private MapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption> CreateSystemUnderTest()
        => _mocker.CreateInstance<MapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();

    public class TheConstructor : MapOptionConverterFacts
    {
        [Fact]
        public void DoesNotThrowExceptionWhenNoStrategiesAreDuplicatedByRegistrationType()
        {
            // Arrange
            var strategyMock = MockStrategy(typeof(FakeResourceMapOptionRegistration));
            var otherStrategyMock = MockStrategy(typeof(FakeOtherResourceMapOptionRegistration));

            _mocker.Use<IEnumerable<IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>>>(
                new[]
                {
                    strategyMock.Object,
                    otherStrategyMock.Object,
                });

            // Act
            var exception = Record.Exception(() => CreateSystemUnderTest());

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenTwoStrategiesAreDefinedForTheSameRegistrationType()
        {
            // Arrange
            var firstStrategyMock = MockStrategy(typeof(FakeResourceMapOptionRegistration));
            var secondStrategyMock = MockStrategy(typeof(FakeResourceMapOptionRegistration));

            _mocker.Use<IEnumerable<IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>>>(
                new[]
                {
                    firstStrategyMock.Object,
                    secondStrategyMock.Object,
                });

            // Act
            var exception = Record.Exception(() => CreateSystemUnderTest());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    public class TheConvertToOptionMethod : MapOptionConverterFacts
    {
        [Fact]
        public void ReturnsImplicitlyConvertedResultWhenRegistrationIsAlsoOption()
        {
            // Arrange
            var registration = new FakeResourceMapOption();
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();

            var strategyMock = MockStrategy(registration.GetType());

            _mocker.Use<IEnumerable<IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>>>(
                new[]
                {
                    strategyMock.Object,
                });

            var sut = CreateSystemUnderTest();

            // Act
            var result = sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(registration, result.Registration);
            Assert.Equal(registration, result.Option);
            Assert.Empty(result.Message);

            strategyMock.Verify(strategy => strategy.ConvertToOption(registration, resourceMapBuilder), Times.Never);
        }

        [Fact]
        public void ReturnsResultFromMatchingStrategy()
        {
            // Arrange
            var registration = new FakeResourceMapOptionRegistration();
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();

            var expectedResult = MapOptionConversionResult.Success<IResourceMapOption>(registration, new FakeResourceMapOption());
            var registrationStrategyMock = MockStrategy(registration.GetType());
            registrationStrategyMock.Setup(strategy => strategy.ConvertToOption(registration, resourceMapBuilder))
                .Returns(expectedResult);

            var otherRegistrationStrategyMock = MockStrategy(typeof(FakeOtherResourceMapOptionRegistration));

            _mocker.Use<IEnumerable<IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>>>(
                new[]
                {
                    registrationStrategyMock.Object,
                    otherRegistrationStrategyMock.Object,
                });

            var sut = CreateSystemUnderTest();

            // Act
            var actualResult = sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ReturnsSkippedResultWhenNoMatchingStrategyIsFound()
        {
            // Arrange
            var registration = new FakeResourceMapOptionRegistration();
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();

            _mocker.Use(Array.Empty<IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>>()
                .AsEnumerable());

            var sut = CreateSystemUnderTest();

            // Act
            var result = sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(registration, result.Registration);
            Assert.Null(result.Option);
            Assert.Empty(result.Message);
        }
    }
}
