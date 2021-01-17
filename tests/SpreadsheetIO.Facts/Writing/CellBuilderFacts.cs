using System;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing
{
    public class CellBuilderFacts
    {
        public class TheWithValueMethodWithIntegerParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithIntegerValue()
            {
                // Arrange
                var value = 21;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("21", builderImpl.Cell.CellValue.Text);
            }
        }

        public class TheWithValueMethodWithNullableIntegerParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithIntegerValueWhenValueIsNotNull()
            {
                // Arrange
                int? value = 21;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("21", builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(int?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }

        public class TheWithValueMethodWithDoubleParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDoubleValue()
            {
                // Arrange
                var value = 21.34D;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("21.34", builderImpl.Cell.CellValue.Text);
            }
        }

        public class TheWithValueMethodWithNullableDoubleParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDoubleValueWhenValueIsNotNull()
            {
                // Arrange
                double? value = 21.34D;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("21.34", builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(double?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }

        public class TheWithValueMethodWithBooleanParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithBooleanValue()
            {
                // Arrange
                var value = true;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("1", builderImpl.Cell.CellValue.Text);
            }
        }

        public class TheWithValueMethodWithNullableBooleanParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithBooleanValueWhenValueIsNotNull()
            {
                // Arrange
                bool? value = true;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("1", builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(bool?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }

        public class TheWithValueMethodWithDecimalParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDecimalValue()
            {
                // Arrange
                var value = 21.34M;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("21.34", builderImpl.Cell.CellValue.Text);
            }
        }

        public class TheWithValueMethodWithNullableDecimalParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDecimalValueWhenValueIsNotNull()
            {
                // Arrange
                decimal? value = 21.34M;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("21.34", builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(decimal?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }

        public class TheWithValueMethodWithDateTimeParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDateTimeValue()
            {
                // Arrange
                var value = new DateTime(2021, 1, 2, 3, 4, 5, 678);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("44198.12784349537", builderImpl.Cell.CellValue.Text);
            }
        }

        public class TheWithValueMethodWithNullableDateTimeParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDateTimeValueWhenValueIsNotNull()
            {
                // Arrange
                DateTime? value = new DateTime(2021, 1, 2, 3, 4, 5, 678);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("44198.12784349537", builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(DateTime?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }

        public class TheWithValueMethodWithDateTimeOffsetParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDateTimeOffsetValue()
            {
                // Arrange
                var value = new DateTimeOffset(
                    new DateTime(2021, 1, 2, 3, 4, 5, 678),
                    new TimeSpan(-5, 0, 0));

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("44198.12784349537", builderImpl.Cell.CellValue.Text);
            }
        }

        public class TheWithValueMethodWithNullableDateTimeOffsetParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithDateTimeOffsetValueWhenValueIsNotNull()
            {
                // Arrange
                DateTimeOffset? value = new DateTimeOffset(
                    new DateTime(2021, 1, 2, 3, 4, 5, 678),
                    new TimeSpan(-5, 0, 0));

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("44198.12784349537", builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(DateTimeOffset?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }

        public class TheWithValueMethodWithCharacterParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithCharacterValue()
            {
                // Arrange
                var value = 'L';

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("L", builderImpl.Cell.CellValue.Text);
            }
        }

        public class TheWithValueMethodWithNullableCharacterParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithCharacterValueWhenValueIsNotNull()
            {
                // Arrange
                char? value = 'L';

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal("L", builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(char?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }

        public class TheWithValueMethodWithStringParameter
        {
            [Fact]
            public void ReturnsCellValueBuilderWhichContainsCellWithStringValueWhenValueIsNotNull()
            {
                // Arrange
                var value = "foo";

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Equal(value, builderImpl.Cell.CellValue.Text);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsEmpty()
            {
                // Arrange
                var value = string.Empty;

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }

            [Fact]
            public void ReturnsCellValueBuilderWhichContainsEmptyCellWhenValueIsNull()
            {
                // Arrange
                var value = default(string?);

                // Act
                var builder = CellBuilder.WithValue(value);

                // Assert
                var builderImpl = Assert.IsType<CellBuilderImpl>(builder);
                Assert.Null(builderImpl.Cell.CellValue);
            }
        }
    }
}
