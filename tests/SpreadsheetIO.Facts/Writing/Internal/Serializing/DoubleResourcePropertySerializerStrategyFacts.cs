using LanceC.SpreadsheetIO.Facts.Testing.Assertions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing;

public class DoubleResourcePropertySerializerStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private DoubleResourcePropertySerializerStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<DoubleResourcePropertySerializerStrategy>();

    public class ThePropertyTypesProperty : DoubleResourcePropertySerializerStrategyFacts
    {
        [Fact]
        public void ReturnsDoubleTypes()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var propertyTypes = sut.PropertyTypes;

            // Assert
            Assert.Equal(5, propertyTypes.Count);
            Assert.Single(propertyTypes, propertyType => propertyType == typeof(uint));
            Assert.Single(propertyTypes, propertyType => propertyType == typeof(long));
            Assert.Single(propertyTypes, propertyType => propertyType == typeof(ulong));
            Assert.Single(propertyTypes, propertyType => propertyType == typeof(float));
            Assert.Single(propertyTypes, propertyType => propertyType == typeof(double));
        }
    }

    public class TheSerializeMethod : DoubleResourcePropertySerializerStrategyFacts
    {
        [Theory]
        [InlineData(0D)]
        [InlineData(1D)]
        public void ReturnsDoubleWritingCellValue(double value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.Double);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0D)]
        [InlineData(1D)]
        public void ReturnsNullableDoubleWritingCellValue(double? value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.DoubleNullable);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(0U)]
        [InlineData(1U)]
        public void ReturnsDoubleWritingCellValueForUnsignedInteger(uint value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.UnsignedInteger);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0U)]
        [InlineData(1U)]
        public void ReturnsNullableDoubleWritingCellValueForNullableUnsignedInteger(uint? value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.UnsignedIntegerNullable);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        public void ReturnsDoubleWritingCellValueForLong(long value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.Long);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0L)]
        [InlineData(1L)]
        public void ReturnsNullableDoubleWritingCellValueForNullableLong(long? value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.LongNullable);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(0UL)]
        [InlineData(1UL)]
        public void ReturnsDoubleWritingCellValueForUnsignedLong(ulong value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.UnsignedLong);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0UL)]
        [InlineData(1UL)]
        public void ReturnsNullableDoubleWritingCellValueForNullableUnsignedLong(ulong? value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.UnsignedLongNullable);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(0F)]
        [InlineData(1F)]
        public void ReturnsDoubleWritingCellValueForFloat(float value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.Float);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0F)]
        [InlineData(1F)]
        public void ReturnsNullableDoubleWritingCellValueForNullableFloat(float? value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.FloatNullable);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Fact]
        public void ThrowsFormatExceptionWhenNonDoubleTypeIsProvided()
        {
            // Arrange
            var map = PropertyMapCreator2.CreateForFakeResourcePropertyStrategyModel(model => model.Double);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Serialize("foo", map));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<FormatException>(exception);
        }
    }
}
