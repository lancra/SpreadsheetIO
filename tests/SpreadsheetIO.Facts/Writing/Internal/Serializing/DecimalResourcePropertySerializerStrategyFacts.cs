using LanceC.SpreadsheetIO.Facts.Testing.Assertions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing;

public class DecimalResourcePropertySerializerStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private DecimalResourcePropertySerializerStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<DecimalResourcePropertySerializerStrategy>();

    public class ThePropertyTypesProperty : DecimalResourcePropertySerializerStrategyFacts
    {
        [Fact]
        public void ReturnsDecimalType()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var propertyTypes = sut.PropertyTypes;

            // Assert
            var propertyType = Assert.Single(propertyTypes);
            Assert.Equal(typeof(decimal), propertyType);
        }
    }

    public class TheSerializeMethod : DecimalResourcePropertySerializerStrategyFacts
    {
        public static TheoryData<decimal> DataForReturnsDecimalWritingCellValue
            => new()
            {
                { 0M },
                { 1M },
            };

        public static TheoryData<decimal?> DataForReturnsNullableDecimalWritingCellValue
            => new()
            {
                { null },
                { 0M },
                { 1M },
            };

        [Theory]
        [MemberData(nameof(DataForReturnsDecimalWritingCellValue))]
        public void ReturnsDecimalWritingCellValue(decimal value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Decimal);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Theory]
        [MemberData(nameof(DataForReturnsNullableDecimalWritingCellValue))]
        public void ReturnsNullableDecimalWritingCellValue(decimal? value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DecimalNullable);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }

        [Fact]
        public void ThrowsInvalidCastExceptionWhenNonDecimalTypeIsProvided()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Decimal);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Serialize("foo", map));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidCastException>(exception);
        }
    }
}
