using System;
using LanceC.SpreadsheetIO.Facts.Testing.Assertions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing
{
    public class BooleanResourcePropertySerializerStrategyFacts
    {
        private readonly AutoMocker _mocker = new();

        private BooleanResourcePropertySerializerStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<BooleanResourcePropertySerializerStrategy>();

        public class ThePropertyTypesProperty : BooleanResourcePropertySerializerStrategyFacts
        {
            [Fact]
            public void ReturnsBooleanType()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var propertyTypes = sut.PropertyTypes;

                // Assert
                var propertyType = Assert.Single(propertyTypes);
                Assert.Equal(typeof(bool), propertyType);
            }
        }

        public class TheSerializeMethod : BooleanResourcePropertySerializerStrategyFacts
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void ReturnsBooleanWritingCellValue(bool value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Boolean);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [InlineData(null)]
            [InlineData(false)]
            [InlineData(true)]
            public void ReturnsNullableBooleanWritingCellValue(bool? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.BooleanNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void ThrowsInvalidCastExceptionWhenNonBooleanTypeIsProvided()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Boolean);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Serialize("foo", map));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidCastException>(exception);
            }
        }
    }
}
