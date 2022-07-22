using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Writing;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Writing;

public class WritingCellValueFacts
{
    public static readonly TheoryData<DateTime, string> DataForHandles1900Dates =
            new()
            {
                { new DateTime(1900, 3, 1), "61" },
                { new DateTime(1900, 2, 28), "59" },
                { new DateTime(1900, 2, 27), "58" },
                { new DateTime(1900, 1, 2), "2" },
                { new DateTime(1900, 1, 1), "1" },
                { new DateTime(1899, 12, 31), "-1" },
                { new DateTime(1899, 12, 30), "-2" },
            };

    private readonly OpenXml.Cell _cell = new();

    public class TheConstructorWithIntegerParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierProperty()
        {
            // Arrange
            var value = 12;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableIntegerParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhenValueIsNotNull()
        {
            // Arrange
            int? value = 12;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(int?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithUnsignedIntegerParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierProperty()
        {
            // Arrange
            var value = 12U;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableUnsignedIntegerParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhenValueIsNotNull()
        {
            // Arrange
            uint? value = 12U;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(uint?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithLongParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierProperty()
        {
            // Arrange
            var value = 12L;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableLongParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhenValueIsNotNull()
        {
            // Arrange
            long? value = 12L;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(long?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithUnsignedLongParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierProperty()
        {
            // Arrange
            var value = 12UL;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableUnsignedLongParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhenValueIsNotNull()
        {
            // Arrange
            ulong? value = 12UL;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(ulong?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithDoubleParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierProperty()
        {
            // Arrange
            var value = 12.34D;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12.34", _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableDoubleParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhenValueIsNotNull()
        {
            // Arrange
            double? value = 12.34D;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12.34", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(double?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithBooleanParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierProperty()
        {
            // Arrange
            var value = true;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("1", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.Boolean, _cell.DataType.Value);
        }
    }

    public class TheConstructorWithNullableBooleanParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhenValueIsNotNull()
        {
            // Arrange
            bool? value = true;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("1", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.Boolean, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(bool?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithDecimalParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierProperty()
        {
            // Arrange
            var value = 12.34M;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12.34", _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableDecimalParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhenValueIsNotNull()
        {
            // Arrange
            decimal? value = 12.34M;

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("12.34", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(decimal?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithDateTimeParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDate()
        {
            // Arrange
            var value = new DateTime(2020, 1, 1, 2, 3, 4, 567);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900Dates(DateTime value, string expectedCellValue)
        {
            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithDateTimeAndCellDateKindParameters : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDateWhenDateKindIsNumber()
        {
            // Arrange
            var value = new DateTime(2020, 1, 1, 2, 3, 4, 567);

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyWhichWritesTextDateWhenDateKindIsText()
        {
            // Arrange
            var value = new DateTime(2020, 1, 1, 2, 3, 4, 567);

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Text);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("2020-01-01T02:03:04.567", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900DatesWhenDateKindIsNumber(DateTime value, string expectedCellValue)
        {
            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableDateTimeParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDateWhenValueIsNotNull()
        {
            // Arrange
            DateTime? value = new DateTime(2020, 1, 1, 2, 3, 4, 567);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(DateTime?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900Dates(DateTime value, string expectedCellValue)
        {
            // Arrange
            DateTime? nullableValue = value;

            // Act
            var cellValue = new WritingCellValue(nullableValue);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableDateTimeAndCellDateKindParameters : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDateWhenDateKindIsNumberAndValueIsNotNull()
        {
            // Arrange
            DateTime? value = new DateTime(2020, 1, 1, 2, 3, 4, 567);

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyWhichWritesTextDateWhenDateKindIsTextAndValueIsNotNull()
        {
            // Arrange
            DateTime? value = new DateTime(2020, 1, 1, 2, 3, 4, 567);

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Text);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("2020-01-01T02:03:04.567", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(DateTime?);

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Text);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900DatesWhenDateKindIsNumber(DateTime value, string expectedCellValue)
        {
            // Arrange
            DateTime? nullableValue = value;

            // Act
            var cellValue = new WritingCellValue(nullableValue, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithDateTimeOffsetParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDate()
        {
            // Arrange
            var value = new DateTimeOffset(new DateTime(2020, 1, 1, 2, 3, 4, 567), new TimeSpan(-5, 0, 0));

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900Dates(DateTime value, string expectedCellValue)
        {
            // Arrange
            var dateTimeOffsetValue = new DateTimeOffset(value);

            // Act
            var cellValue = new WritingCellValue(dateTimeOffsetValue);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithDateTimeOffsetAndCellDateKindParameters : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDateWhenDateKindIsNumber()
        {
            // Arrange
            var value = new DateTimeOffset(new DateTime(2020, 1, 1, 2, 3, 4, 567), new TimeSpan(-5, 0, 0));

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyWhichWritesTextDateWhenDateKindIsText()
        {
            // Arrange
            var value = new DateTimeOffset(new DateTime(2020, 1, 1, 2, 3, 4, 567), new TimeSpan(-5, 0, 0));

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Text);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("2020-01-01T02:03:04.567-05:00", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900DatesWhenDateKindIsNumber(DateTime value, string expectedCellValue)
        {
            // Arrange
            var dateTimeOffsetValue = new DateTimeOffset(value);

            // Act
            var cellValue = new WritingCellValue(dateTimeOffsetValue, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableDateTimeOffsetParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDateWhenValueIsNotNull()
        {
            // Arrange
            DateTimeOffset? value = new DateTimeOffset(new DateTime(2020, 1, 1, 2, 3, 4, 567), new TimeSpan(-5, 0, 0));

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(DateTimeOffset?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900Dates(DateTime value, string expectedCellValue)
        {
            // Arrange
            DateTimeOffset? dateTimeOffsetValue = new(value);

            // Act
            var cellValue = new WritingCellValue(dateTimeOffsetValue);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithNullableDateTimeOffsetAndCellDateKindParameters : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesNumericDateWhenDateKindIsNumberAndValueIsNotNull()
        {
            // Arrange
            DateTimeOffset? value = new DateTimeOffset(new DateTime(2020, 1, 1, 2, 3, 4, 567), new TimeSpan(-5, 0, 0));

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("43831.08546952546", _cell.CellValue.Text);
        }

        [Fact]
        public void SetsCellModifierPropertyWhichWritesTextDateWhenDateKindIsTextAndValueIsNotNull()
        {
            // Arrange
            DateTimeOffset? value = new DateTimeOffset(new DateTime(2020, 1, 1, 2, 3, 4, 567), new TimeSpan(-5, 0, 0));

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Text);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("2020-01-01T02:03:04.567-05:00", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(DateTimeOffset?);

            // Act
            var cellValue = new WritingCellValue(value, CellDateKind.Text);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }

        [Theory]
        [MemberData(nameof(DataForHandles1900Dates))]
        public void Handles1900DatesWhenDateKindIsNumber(DateTime value, string expectedCellValue)
        {
            // Arrange
            DateTimeOffset? dateTimeOffsetValue = new(value);

            // Act
            var cellValue = new WritingCellValue(dateTimeOffsetValue, CellDateKind.Number);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal(expectedCellValue, _cell.CellValue.Text);
        }
    }

    public class TheConstructorWithCharacterParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesSharedString()
        {
            // Arrange
            var value = 'L';

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("L", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }
    }

    public class TheConstructorWithCharacterAndCellStringKindParameters : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesSharedStringWhenStringKindIsSharedString()
        {
            // Arrange
            var value = 'L';

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.SharedString);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("L", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyWhichWritesInlineStringWhenStringKindIsInlineString()
        {
            // Arrange
            var value = 'L';

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.InlineString);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("L", _cell.InlineString.Text.Text);
            Assert.Equal(OpenXml.CellValues.InlineString, _cell.DataType.Value);
        }
    }

    public class TheConstructorWithNullableCharacterParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesSharedStringWhenValueIsNotNull()
        {
            // Arrange
            char? value = 'L';

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("L", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(char?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithNullableCharacterAndCellStringKindParameters : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesSharedStringWhenStringKindIsSharedStringAndValueIsNotNull()
        {
            // Arrange
            char? value = 'L';

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.SharedString);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("L", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyWhichWritesInlineStringWhenStringKindIsInlineStringAndValueIsNotNull()
        {
            // Arrange
            char? value = 'L';

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.InlineString);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("L", _cell.InlineString.Text.Text);
            Assert.Equal(OpenXml.CellValues.InlineString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(char?);

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.InlineString);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithStringParameter : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesSharedStringWhenValueIsNotNull()
        {
            // Arrange
            string? value = "Test";

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("Test", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(string?);

            // Act
            var cellValue = new WritingCellValue(value);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }

    public class TheConstructorWithStringAndCellStringKindParameters : WritingCellValueFacts
    {
        [Fact]
        public void SetsCellModifierPropertyWhichWritesSharedStringWhenStringKindIsSharedString()
        {
            // Arrange
            string? value = "Test";

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.SharedString);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("Test", _cell.CellValue.Text);
            Assert.Equal(OpenXml.CellValues.SharedString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyWhichWritesInlineStringWhenStringKindIsInlineString()
        {
            // Arrange
            string? value = "Test";

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.InlineString);

            // Assert
            Assert.NotNull(cellValue.CellModifier);
            cellValue.CellModifier!(_cell);
            Assert.Equal("Test", _cell.InlineString.Text.Text);
            Assert.Equal(OpenXml.CellValues.InlineString, _cell.DataType.Value);
        }

        [Fact]
        public void SetsCellModifierPropertyToNullWhenValueIsNull()
        {
            // Arrange
            var value = default(string?);

            // Act
            var cellValue = new WritingCellValue(value, CellStringKind.InlineString);

            // Assert
            Assert.Null(cellValue.CellModifier);
        }
    }
}
