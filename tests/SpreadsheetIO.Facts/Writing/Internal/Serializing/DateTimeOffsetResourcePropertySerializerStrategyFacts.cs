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
    public class DateTimeOffsetResourcePropertySerializerStrategyFacts
    {
        private readonly AutoMocker _mocker = new();

        private DateTimeOffsetResourcePropertySerializerStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<DateTimeOffsetResourcePropertySerializerStrategy>();

        public class ThePropertyTypesProperty : DateTimeOffsetResourcePropertySerializerStrategyFacts
        {
            [Fact]
            public void ReturnsDateTimeOffsetType()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var propertyTypes = sut.PropertyTypes;

                // Assert
                var propertyType = Assert.Single(propertyTypes);
                Assert.Equal(typeof(DateTimeOffset), propertyType);
            }
        }

        public class TheSerializeMethod : DateTimeOffsetResourcePropertySerializerStrategyFacts
        {
            public static TheoryData<DateTimeOffset> DataForReturnsDateTimeOffsetWritingCellValue
                => new()
                {
                    { default },
                    { new DateTimeOffset(2021, 2, 3, 4, 5, 6, new TimeSpan(-4, 0, 0)) },
                };

            public static TheoryData<DateTimeOffset?> DataForReturnsNullableDateTimeOffsetWritingCellValue
                => new()
                {
                    { null },
                    { default(DateTimeOffset) },
                    { new DateTimeOffset(2021, 2, 3, 4, 5, 6, new TimeSpan(-4, 0, 0)) },
                };

            public static TheoryData<DateTimeOffset, CellDateKind> DataForUsesDateKindFromExtensionWhenSpecified
                => new()
                {
                    { new DateTimeOffset(2021, 2, 3, 4, 5, 6, new TimeSpan(-4, 0, 0)), CellDateKind.Number },
                    { new DateTimeOffset(2021, 2, 3, 4, 5, 6, new TimeSpan(-4, 0, 0)), CellDateKind.Text },
                };

            [Theory]
            [MemberData(nameof(DataForReturnsDateTimeOffsetWritingCellValue))]
            public void ReturnsDateTimeOffsetWritingCellValue(DateTimeOffset value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForReturnsNullableDateTimeOffsetWritingCellValue))]
            public void ReturnsNullableDateTimeOffsetWritingCellValue(DateTimeOffset? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffsetNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [MemberData(nameof(DataForUsesDateKindFromExtensionWhenSpecified))]
            public void UsesDateKindFromExtensionWhenSpecified(DateTimeOffset value, CellDateKind dateKind)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value, dateKind);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(
                    model => model.DateTimeOffset,
                    new DateKindMapOptionsExtension(dateKind));

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void ThrowsInvalidCastExceptionWhenNonDateTimeOffsetTypeIsProvided()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.DateTimeOffset);
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
