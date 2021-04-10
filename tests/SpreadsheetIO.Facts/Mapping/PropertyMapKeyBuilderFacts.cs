using System;
using System.Reflection;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping
{
    public class PropertyMapKeyBuilderFacts
    {
        public static PropertyInfo Property
            => typeof(FakeModel).GetProperty(nameof(FakeModel.Name))!;

        public class TheConstructor : PropertyMapKeyBuilderFacts
        {
            [Fact]
            public void SetsKeyNameFromProvidedProperty()
            {
                // Arrange
                var sut = new PropertyMapKeyBuilder(Property);

                // Act
                var key = sut.Key;

                // Assert
                Assert.Equal(nameof(FakeModel.Name), key.Name);
                Assert.Equal(default, key.Number);
                Assert.Equal(default, key.IsNameIgnored);
            }

            [Fact]
            public void ThrowArgumentNullExceptionWhenPropertyIsNull()
            {
                // Arrange
                var property = default(PropertyInfo);

                // Act
                var exception = Record.Exception(() => new PropertyMapKeyBuilder(property!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheOverrideNameMethod : PropertyMapKeyBuilderFacts
        {
            [Fact]
            public void SetsKeyName()
            {
                // Arrange
                var name = "Bar";
                var sut = new PropertyMapKeyBuilder(Property);

                // Act
                sut.OverrideName(name);

                // Assert
                Assert.Equal(name, sut.Key.Name);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenNameIsEmpty()
            {
                // Arrange
                var name = string.Empty;
                var sut = new PropertyMapKeyBuilder(Property);

                // Act
                var exception = Record.Exception(() => sut.OverrideName(name));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenNameIsNull()
            {
                // Arrange
                var name = default(string);
                var sut = new PropertyMapKeyBuilder(Property);

                // Act
                var exception = Record.Exception(() => sut.OverrideName(name!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheIgnoreNameMethod : PropertyMapKeyBuilderFacts
        {
            [Fact]
            public void SetsKeyNameAsIgnored()
            {
                // Arrange
                var sut = new PropertyMapKeyBuilder(Property);

                // Act
                sut.IgnoreName();

                // Assert
                Assert.True(sut.Key.IsNameIgnored);
                Assert.Equal(nameof(FakeModel.Name), sut.Key.Name);
            }
        }

        public class TheUseNumberMethod : PropertyMapKeyBuilderFacts
        {
            [Fact]
            public void SetsKeyNumber()
            {
                // Arrange
                var number = 1U;
                var sut = new PropertyMapKeyBuilder(Property);

                // Act
                sut.UseNumber(number);

                // Assert
                Assert.Equal(number, sut.Key.Number);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenNumberIsZero()
            {
                // Arrange
                var number = 0U;
                var sut = new PropertyMapKeyBuilder(Property);

                // Act
                var exception = Record.Exception(() => sut.UseNumber(number));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }
        }
    }
}
