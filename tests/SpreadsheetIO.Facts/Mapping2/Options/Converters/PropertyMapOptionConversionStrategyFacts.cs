using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Options.Converters;

public class PropertyMapOptionConversionStrategyFacts
{
    private static IMapOptionConversionStrategy<IPropertyMapOptionRegistration, IPropertyMapOption> CreateSystemUnderTest()
        => new FakePropertyMapOptionConversionStrategy();

    public class TheConvertToOptionMethod : PropertyMapOptionConversionStrategyFacts
    {
        [Fact]
        public void CallsGenericConversionMethod()
        {
            // Arrange
            var registration = new FakePropertyMapOptionRegistration();
            var resourceMapBuilder = new ResourceMapBuilder<FakeModel>();
            var sut = CreateSystemUnderTest();

            // Act
            sut.ConvertToOption(registration, resourceMapBuilder);

            // Assert
            Assert.True(((FakePropertyMapOptionConversionStrategy)sut).RanConversion);
        }

        [Fact]
        public void ThrowsInvalidCastExceptionWhenRegistrationIsNotExpectedType()
        {
            // Arrange
            var registration = new FakeOtherPropertyMapOptionRegistration();
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
