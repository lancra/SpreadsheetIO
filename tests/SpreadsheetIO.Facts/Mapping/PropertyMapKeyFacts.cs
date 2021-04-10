using LanceC.SpreadsheetIO.Mapping;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping
{
    public class PropertyMapKeyFacts
    {
        public static TheoryData<PropertyMapKey?, PropertyMapKey?> DataForWhenOneKeyIsNull
            => new()
            {
                {
                    default,
                    new PropertyMapKey(default, default, default)
                },
                {
                    new PropertyMapKey(default, default, default),
                    default
                },
            };

        public static TheoryData<PropertyMapKey, PropertyMapKey> DataForWhenNameAndNumberAreNotEqual
            => new()
            {
                {
                    new PropertyMapKey("foo", 1U, true),
                    new PropertyMapKey("foo", 2U, false)
                },
                {
                    new PropertyMapKey("foo", 1U, true),
                    new PropertyMapKey("bar", 1U, false)
                },
                {
                    new PropertyMapKey("foo", 1U, true),
                    new PropertyMapKey("bar", 2U, false)
                },
                {
                    new PropertyMapKey(default, 1U, true),
                    new PropertyMapKey("foo", 1U, false)
                },
                {
                    new PropertyMapKey("foo", default, true),
                    new PropertyMapKey("foo", 1U, false)
                },
            };

        public class TheEqualityOperator : PropertyMapKeyFacts
        {
            [Fact]
            public void ReturnsTrueWhenBothKeysAreNull()
            {
                // Arrange
                var firstKey = default(PropertyMapKey);
                var secondKey = default(PropertyMapKey);

                // Act
                var result = firstKey! == secondKey!;

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsTrueWhenNameAndNumberAreEqual()
            {
                // Arrange
                var name = "foo";
                var number = 1U;

                var firstKey = new PropertyMapKey(name, number, true);
                var secondKey = new PropertyMapKey(name, number, false);

                // Act
                var result = firstKey == secondKey;

                // Assert
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(DataForWhenOneKeyIsNull))]
            public void ReturnsFalseWhenOneKeyIsNull(PropertyMapKey firstKey, PropertyMapKey secondKey)
            {
                // Act
                var result = firstKey == secondKey;

                // Assert
                Assert.False(result);
            }

            [Theory]
            [MemberData(nameof(DataForWhenNameAndNumberAreNotEqual))]
            public void ReturnsFalseWhenNameAndNumberAreNotEqual(PropertyMapKey firstKey, PropertyMapKey secondKey)
            {
                // Act
                var result = firstKey == secondKey;

                // Assert
                Assert.False(result);
            }
        }

        public class TheInequalityOperator : PropertyMapKeyFacts
        {
            [Theory]
            [MemberData(nameof(DataForWhenOneKeyIsNull))]
            public void ReturnsTrueWhenOneKeyIsNull(PropertyMapKey firstKey, PropertyMapKey secondKey)
            {
                // Act
                var result = firstKey != secondKey;

                // Assert
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(DataForWhenNameAndNumberAreNotEqual))]
            public void ReturnsTrueWhenNameAndNumberAreNotEqual(PropertyMapKey firstKey, PropertyMapKey secondKey)
            {
                // Act
                var result = firstKey != secondKey;

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseWhenBothKeysAreNull()
            {
                // Arrange
                var firstKey = default(PropertyMapKey);
                var secondKey = default(PropertyMapKey);

                // Act
                var result = firstKey! != secondKey!;

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseWhenNameAndNumberAreEqual()
            {
                // Arrange
                var name = "foo";
                var number = 1U;

                var firstKey = new PropertyMapKey(name, number, true);
                var secondKey = new PropertyMapKey(name, number, false);

                // Act
                var result = firstKey != secondKey;

                // Assert
                Assert.False(result);
            }
        }

        public class TheEqualsMethod : PropertyMapKeyFacts
        {
            [Fact]
            public void ReturnsTrueWhenNameAndNumberAreEqual()
            {
                // Arrange
                var name = "foo";
                var number = 1U;

                var key = new PropertyMapKey(name, number, true);
                var otherKey = new PropertyMapKey(name, number, false);

                // Act
                var result = key.Equals(otherKey);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseWhenOtherKeyIsIncorrectType()
            {
                // Arrange
                var key = new PropertyMapKey("foo", 1U, true);
                var otherKey = "foo";

                // Act
                var result = key.Equals(otherKey);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseWhenOtherKeyIsNull()
            {
                // Arrange
                var key = new PropertyMapKey("foo", 1U, true);
                var otherKey = default(PropertyMapKey);

                // Act
                var result = key.Equals(otherKey);

                // Assert
                Assert.False(result);
            }

            [Theory]
            [MemberData(nameof(DataForWhenNameAndNumberAreNotEqual))]
            public void ReturnsFalseWhenNameAndNumberAreNotEqual(PropertyMapKey firstKey, PropertyMapKey secondKey)
            {
                // Act
                var result = firstKey.Equals(secondKey);

                // Assert
                Assert.False(result);
            }
        }
    }
}
