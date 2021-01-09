using System;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Shared;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared
{
    public class EnumerationFacts
    {
        public static TheoryData<FakeEnumeration?, FakeEnumeration?> DataForOperatorEqualityAndInequalityWhereValuesAreNotEqual
            => new TheoryData<FakeEnumeration?, FakeEnumeration?>
            {
                {
                    default,
                    FakeEnumeration.Foo
                },
                {
                    FakeEnumeration.Foo,
                    default
                },
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Bar
                },
                {
                    FakeEnumeration.Bar,
                    FakeEnumeration.Foo
                },
            };

        public static TheoryData<FakeEnumeration?, FakeEnumeration?> DataForOperatorEqualityAndInequalityWhereValuesAreEqual
            => new TheoryData<FakeEnumeration?, FakeEnumeration?>
            {
                {
                    default,
                    default
                },
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Foo
                },
            };

        public static TheoryData<FakeEnumeration?, FakeEnumeration?> DataForOperatorComparisonWhereLeftValueLessThanRightValue
            => new TheoryData<FakeEnumeration?, FakeEnumeration?>
            {
                {
                    default,
                    FakeEnumeration.Foo
                },
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Bar
                },
            };

        public static TheoryData<FakeEnumeration?, FakeEnumeration?>
            DataForOperatorComparisonWhereLeftValueGreaterThanOrEqualToRightValue
            => new TheoryData<FakeEnumeration?, FakeEnumeration?>
            {
                {
                    default,
                    default
                },
                {
                    FakeEnumeration.Foo,
                    default
                },
                {
                    FakeEnumeration.Bar,
                    FakeEnumeration.Foo
                },
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Foo
                },
            };

        public static TheoryData<FakeEnumeration?, FakeEnumeration?> DataForOperatorComparisonWhereLeftValueLessThanOrEqualToRightValue
            => new TheoryData<FakeEnumeration?, FakeEnumeration?>
            {
                {
                    default,
                    default
                },
                {
                    default,
                    FakeEnumeration.Foo
                },
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Bar
                },
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Foo
                },
            };

        public static TheoryData<FakeEnumeration?, FakeEnumeration?> DataForOperatorComparisonWhereLeftValueGreaterThanRightValue
            => new TheoryData<FakeEnumeration?, FakeEnumeration?>
            {
                {
                    FakeEnumeration.Foo,
                    default
                },
                {
                    FakeEnumeration.Bar,
                    FakeEnumeration.Foo
                },
            };

        public class TheEqualityOperator : EnumerationFacts
        {
            [Theory]
            [MemberData(nameof(DataForOperatorEqualityAndInequalityWhereValuesAreNotEqual))]
            public void ReturnsFalseWhenValuesAreNotEqual(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration == other;

                // Assert
                Assert.False(result);
            }

            [Theory]
            [MemberData(nameof(DataForOperatorEqualityAndInequalityWhereValuesAreEqual))]
            public void ReturnsTrueWhenValuesAreEqual(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration == other;

                // Assert
                Assert.True(result);
            }
        }

        public class TheInequalityOperator : EnumerationFacts
        {
            [Theory]
            [MemberData(nameof(DataForOperatorEqualityAndInequalityWhereValuesAreNotEqual))]
            public void ReturnsTrueWhenValuesAreNotEqual(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration != other;

                // Assert
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(DataForOperatorEqualityAndInequalityWhereValuesAreEqual))]
            public void ReturnsFalseWhenValuesAreEqual(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration != other;

                // Assert
                Assert.False(result);
            }
        }

        public class TheLessThanOperator : EnumerationFacts
        {
            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueLessThanRightValue))]
            public void ReturnsTrueWhenLeftValueLessThanRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration < other;

                // Assert
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueGreaterThanOrEqualToRightValue))]
            public void ReturnsFalseWhenLeftValueGreaterThanOrEqualToRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration < other;

                // Assert
                Assert.False(result);
            }
        }

        public class TheLessThanOrEqualOperator : EnumerationFacts
        {
            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueLessThanOrEqualToRightValue))]
            public void ReturnsTrueWhenLeftValueLessThanOrEqualToRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration <= other;

                // Assert
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueGreaterThanRightValue))]
            public void ReturnsFalseWhenLeftValueGreaterThanRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration <= other;

                // Assert
                Assert.False(result);
            }
        }

        public class TheGreaterThanOperator : EnumerationFacts
        {
            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueGreaterThanRightValue))]
            public void ReturnsTrueWhenLeftValueGreaterThanRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration > other;

                // Assert
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueLessThanOrEqualToRightValue))]
            public void ReturnsFalseWhenLeftValueLessThanOrEqualToRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration > other;

                // Assert
                Assert.False(result);
            }
        }

        public class TheGreaterThanOrEqualOperator : EnumerationFacts
        {
            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueGreaterThanOrEqualToRightValue))]
            public void ReturnsTrueWhenLeftValueGreaterThanOrEqualToRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration >= other;

                // Assert
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(DataForOperatorComparisonWhereLeftValueLessThanRightValue))]
            public void ReturnsFalseWhenLeftValueLessThanRightValue(
                FakeEnumeration enumeration,
                FakeEnumeration other)
            {
                // Act
                var result = enumeration >= other;

                // Assert
                Assert.False(result);
            }
        }

        public class TheFromIdMethod : EnumerationFacts
        {
            [Fact]
            public void ReturnsEnumerationInstanceForValidValue()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                var actual = Enumeration.FromId<FakeEnumeration>(expected.Id);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ThrowsNotSupportedExceptionForInvalidValue()
            {
                // Arrange
                var value = int.MaxValue;

                // Act
                var exception = Record.Exception(
                    () => Enumeration.FromId<FakeEnumeration>(value));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<NotSupportedException>(exception);
            }

            [Fact]
            public void ThrowsNotSupportedExceptionForDefaultValue()
            {
                // Act
                var exception = Record.Exception(() => Enumeration.FromId<FakeEnumeration>(default));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<NotSupportedException>(exception);
            }
        }

        public class TheFromNameMethod : EnumerationFacts
        {
            [Fact]
            public void ReturnsEnumerationInstanceForValidValue()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                var actual = Enumeration.FromName<FakeEnumeration>(expected.Name);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsEnumerationInstanceForValidValueOfDifferentCase()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                var actual = Enumeration.FromName<FakeEnumeration>(expected.Name.ToUpper());

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ThrowsNotSupportedExceptionForNullValue()
            {
                // Act
                var exception = Record.Exception(() => Enumeration.FromName<FakeEnumeration>(default));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<NotSupportedException>(exception);
            }

            [Fact]
            public void ThrowsNotSupportedExceptionForInvalidValue()
            {
                // Arrange
                var value = "qux";

                // Act
                var exception = Record.Exception(
                    () => Enumeration.FromName<FakeEnumeration>(value));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<NotSupportedException>(exception);
            }
        }

        public class TheTryFromIdMethod : EnumerationFacts
        {
            [Fact]
            public void ReturnsTrueForValidValue()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                var success = Enumeration.TryFromId<FakeEnumeration>(expected.Id, out var _);

                // Assert
                Assert.True(success);
            }

            [Fact]
            public void PassesEnumerationOutForValidValue()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                Enumeration.TryFromId<FakeEnumeration>(expected.Id, out var actual);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsFalseForInvalidValue()
            {
                // Arrange
                var value = int.MaxValue;

                // Act
                var success = Enumeration.TryFromId<FakeEnumeration>(value, out var _);

                // Assert
                Assert.False(success);
            }

            [Fact]
            public void PassesNullEnumerationOutForInvalidValue()
            {
                // Arrange
                var value = int.MaxValue;

                // Act
                Enumeration.TryFromId<FakeEnumeration>(value, out var enumeration);

                // Assert
                Assert.Null(enumeration);
            }
        }

        public class TheTryFromNameMethod : EnumerationFacts
        {
            [Fact]
            public void ReturnsTrueForValidValue()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                var success = Enumeration.TryFromName<FakeEnumeration>(expected.Name, out var _);

                // Assert
                Assert.True(success);
            }

            [Fact]
            public void PassesEnumerationOutForValidValue()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                Enumeration.TryFromName<FakeEnumeration>(expected.Name, out var actual);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void PassesEnumerationOutForValidValueOfDifferentCase()
            {
                // Arrange
                var expected = FakeEnumeration.Foo;

                // Act
                Enumeration.TryFromName<FakeEnumeration>(expected.Name.ToUpper(), out var actual);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsFalseForInvalidValue()
            {
                // Arrange
                var value = "qux";

                // Act
                var success = Enumeration.TryFromName<FakeEnumeration>(value, out var _);

                // Assert
                Assert.False(success);
            }

            [Fact]
            public void PassesNullEnumerationOutForInvalidValue()
            {
                // Arrange
                var value = "qux";

                // Act
                Enumeration.TryFromName<FakeEnumeration>(value, out var enumeration);

                // Assert
                Assert.Null(enumeration);
            }
        }

        public class TheGetAllMethodWithGenericType : EnumerationFacts
        {
            [Fact]
            public void ReturnsAllValues()
            {
                // Arrange
                var expecteds = new[]
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Bar,
                    FakeEnumeration.Baz,
                };

                // Act
                var actuals = Enumeration.GetAll<FakeEnumeration>();

                // Assert
                Assert.Equal(expecteds, actuals);
            }
        }

        public class TheGetAllMethodWithTypeParameter : EnumerationFacts
        {
            [Fact]
            public void ReturnsAllValues()
            {
                // Arrange
                var expecteds = new[]
                {
                    FakeEnumeration.Foo,
                    FakeEnumeration.Bar,
                    FakeEnumeration.Baz,
                };

                // Act
                var actuals = Enumeration.GetAll(typeof(FakeEnumeration));

                // Assert
                Assert.Equal(expecteds, actuals);
            }
        }

        public class TheCompareToMethod : EnumerationFacts
        {
            public static TheoryData<FakeEnumeration, FakeEnumeration, int> ReturnsComparisonResult_Data
                => new TheoryData<FakeEnumeration, FakeEnumeration, int>
                {
                    {
                        FakeEnumeration.Foo,
                        FakeEnumeration.Bar,
                        -1
                    },
                    {
                        FakeEnumeration.Foo,
                        FakeEnumeration.Foo,
                        0
                    },
                    {
                        FakeEnumeration.Bar,
                        FakeEnumeration.Foo,
                        1
                    },
                };

            [Theory]
            [MemberData(nameof(ReturnsComparisonResult_Data))]
            public void ReturnsComparisonResult(
                FakeEnumeration enumeration,
                FakeEnumeration other,
                int expected)
            {
                // Act
                var actual = enumeration.CompareTo(other);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsNullValuePrecedingEnumeration()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                var other = default(FakeEnumeration);

                // Act
                var result = enumeration.CompareTo(other);

                // Assert
                Assert.Equal(1, result);
            }
        }

        public class TheEqualsMethod : EnumerationFacts
        {
            [Fact]
            public void ReturnsFalseWhenOtherIsNull()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                var other = default(FakeEnumeration);

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseWhenTypesDoNotMatch()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                var other = FakeOtherEnumeration.Foo;

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseWhenValuesDoNotMatch()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                var other = FakeEnumeration.Bar;

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueWhenTypesAndValuesMatch()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                var other = FakeEnumeration.Foo;

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseWhenOtherIsObjectAndNull()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                object? other = default(FakeEnumeration);

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseWhenOtherIsObjectAndTypesDoNotMatch()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                object other = FakeOtherEnumeration.Foo;

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseWhenOtherIsObjectAndValuesDoNotMatch()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                object other = FakeEnumeration.Bar;

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueWhenOtherIsObjectAndTypesAndValuesMatch()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;
                object other = FakeEnumeration.Foo;

                // Act
                var result = enumeration.Equals(other);

                // Assert
                Assert.True(result);
            }
        }

        public class TheGetHashCodeMethod : EnumerationFacts
        {
            [Fact]
            public void ReturnsId()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;

                // Act
                var result = enumeration.GetHashCode();

                // Assert
                Assert.Equal(enumeration.Id, result);
            }
        }

        public class TheToStringMethod : EnumerationFacts
        {
            [Fact]
            public void ReturnsName()
            {
                // Arrange
                var enumeration = FakeEnumeration.Foo;

                // Act
                var result = enumeration.ToString();

                // Assert
                Assert.Equal(enumeration.Name, result);
            }
        }
    }
}
