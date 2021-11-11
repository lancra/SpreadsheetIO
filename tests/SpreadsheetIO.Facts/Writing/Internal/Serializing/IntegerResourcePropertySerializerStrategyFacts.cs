using LanceC.SpreadsheetIO.Facts.Testing.Assertions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing
{
    public class IntegerResourcePropertySerializerStrategyFacts
    {
        private readonly AutoMocker _mocker = new();

        private IntegerResourcePropertySerializerStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<IntegerResourcePropertySerializerStrategy>();

        public class ThePropertyTypesProperty : IntegerResourcePropertySerializerStrategyFacts
        {
            [Fact]
            public void ReturnsIntegerTypes()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var propertyTypes = sut.PropertyTypes;

                // Assert
                Assert.Equal(5, propertyTypes.Count);
                Assert.Single(propertyTypes, propertyType => propertyType == typeof(sbyte));
                Assert.Single(propertyTypes, propertyType => propertyType == typeof(byte));
                Assert.Single(propertyTypes, propertyType => propertyType == typeof(short));
                Assert.Single(propertyTypes, propertyType => propertyType == typeof(ushort));
                Assert.Single(propertyTypes, propertyType => propertyType == typeof(int));
            }
        }

        public class TheSerializeMethod : IntegerResourcePropertySerializerStrategyFacts
        {
            public static TheoryData<sbyte?> DataForReturnsNullableIntegerWritingCellValueForNullableSignedByte
                => new()
                {
                    { null },
                    { 0 },
                    { 1 },
                };

            public static TheoryData<byte?> DataForReturnsNullableIntegerWritingCellValueForNullableByte
                => new()
                {
                    { null },
                    { 0 },
                    { 1 },
                };

            public static TheoryData<short?> DataForReturnsNullableIntegerWritingCellValueForNullableShort
                => new()
                {
                    { null },
                    { 0 },
                    { 1 },
                };

            public static TheoryData<ushort?> DataForReturnsNullableIntegerWritingCellValueForNullableUnsignedShort
                => new()
                {
                    { null },
                    { 0 },
                    { 1 },
                };

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            public void ReturnsIntegerWritingCellValue(int value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Integer);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [InlineData(null)]
            [InlineData(0)]
            [InlineData(1)]
            public void ReturnsNullableIntegerWritingCellValue(int? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.IntegerNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            public void ReturnsIntegerWritingCellValueForSignedByte(sbyte value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.SignedByte);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForReturnsNullableIntegerWritingCellValueForNullableSignedByte))]
            public void ReturnsNullableIntegerWritingCellValueForNullableSignedByte(sbyte? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.SignedByteNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            public void ReturnsIntegerWritingCellValueForByte(byte value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Byte);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForReturnsNullableIntegerWritingCellValueForNullableByte))]
            public void ReturnsNullableIntegerWritingCellValueForNullableByte(byte? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.ByteNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            public void ReturnsIntegerWritingCellValueForShort(short value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Short);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForReturnsNullableIntegerWritingCellValueForNullableShort))]
            public void ReturnsNullableIntegerWritingCellValueForNullableShort(short? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.ShortNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            public void ReturnsIntegerWritingCellValueForUnsignedShort(ushort value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.UnsignedShort);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForReturnsNullableIntegerWritingCellValueForNullableUnsignedShort))]
            public void ReturnsNullableIntegerWritingCellValueForNullableUnsignedShort(ushort? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.UnsignedShortNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void ThrowsFormatExceptionWhenNonIntegerTypeIsProvided()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Integer);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Serialize("foo", map));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<FormatException>(exception);
            }
        }
    }
}
