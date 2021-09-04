using System;
using LanceC.SpreadsheetIO.Facts.Testing.Assertions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Mapping.Internal.Extensions;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing
{
    public class DateTimeResourcePropertySerializerStrategyFacts
    {
        private readonly AutoMocker _mocker = new();

        private DateTimeResourcePropertySerializerStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<DateTimeResourcePropertySerializerStrategy>();

        public class ThePropertyTypesProperty : DateTimeResourcePropertySerializerStrategyFacts
        {
            [Fact]
            public void ReturnsDateTimeType()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var propertyTypes = sut.PropertyTypes;

                // Assert
                var propertyType = Assert.Single(propertyTypes);
                Assert.Equal(typeof(DateTime), propertyType);
            }
        }

        public class TheSerializeMethod : DateTimeResourcePropertySerializerStrategyFacts
        {
            public static TheoryData<DateTime> DataForReturnsDateTimeWritingCellValue
                => new()
                {
                    { default },
                    { new DateTime(2021, 2, 3, 4, 5, 6) },
                };

            public static TheoryData<DateTime?> DataForReturnsNullableDateTimeWritingCellValue
                => new()
                {
                    { null },
                    { default(DateTime) },
                    { new DateTime(2021, 2, 3, 4, 5, 6) },
                };

            public static TheoryData<DateTime, CellDateKind> DataForUsesDateKindFromExtensionWhenSpecified
                => new()
                {
                    { new DateTime(2021, 2, 3, 4, 5, 6), CellDateKind.Number },
                    { new DateTime(2021, 2, 3, 4, 5, 6), CellDateKind.Text },
                };

            [Theory]
            [MemberData(nameof(DataForReturnsDateTimeWritingCellValue))]
            public void ReturnsDateTimeWritingCellValue(DateTime value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTime);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForReturnsNullableDateTimeWritingCellValue))]
            public void ReturnsNullableDateTimeWritingCellValue(DateTime? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForUsesDateKindFromExtensionWhenSpecified))]
            public void UsesDateKindFromExtensionWhenSpecified(DateTime value, CellDateKind dateKind)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value, dateKind);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(
                    model => model.DateTime,
                    new DateKindMapOptionsExtension(dateKind));

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void ThrowsInvalidCastExceptionWhenNonDateTimeTypeIsProvided()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTime);
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
