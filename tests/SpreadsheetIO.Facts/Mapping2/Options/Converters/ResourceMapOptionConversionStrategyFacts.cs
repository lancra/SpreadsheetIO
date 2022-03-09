using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Options.Converters;

public class ResourceMapOptionConversionStrategyFacts
{
    private static IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption> CreateSystemUnderTest()
        => new FakeResourceMapOptionConversionStrategy();

    public class TheConvertToOptionMethod : ResourceMapOptionConversionStrategyFacts
    {
        [Fact]
        public void CallsGenericConversionMethod()
        {
            // Arrange
            var registration = new FakeResourceMapOptionRegistration();
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();
            var sut = CreateSystemUnderTest();

            // Act
            sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.True(((FakeResourceMapOptionConversionStrategy)sut).RanConversion);
        }

        [Fact]
        public void ThrowsInvalidCastExceptionWhenRegistrationIsNotExpectedType()
        {
            // Arrange
            var registration = new FakeOtherResourceMapOptionRegistration();
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ConvertToOption(registration, resourceMapBuilder));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidCastException>(exception);
        }
    }
}
