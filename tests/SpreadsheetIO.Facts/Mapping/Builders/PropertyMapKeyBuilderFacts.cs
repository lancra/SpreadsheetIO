using System.Reflection;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping.Builders;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Builders;

public class PropertyMapKeyBuilderFacts
{
    private readonly AutoMocker _mocker = new();

    private PropertyMapKeyBuilder CreateSystemUnderTest(PropertyInfo propertyInfo)
    {
        _mocker.Use(propertyInfo);
        return _mocker.CreateInstance<PropertyMapKeyBuilder>(true);
    }

    public class TheConstructor : PropertyMapKeyBuilderFacts
    {
        [Fact]
        public void ThrowsArgumentNullExceptionWhenPropertyInfoIsNull()
        {
            // Act
            var exception = Record.Exception(() => CreateSystemUnderTest(default!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheWithNameMethod : PropertyMapKeyBuilderFacts
    {
        [Fact]
        public void OverridesName()
        {
            // Arrange
            var name = "foo";

            var propertyInfo = typeof(FakeModel).GetProperty(nameof(FakeModel.Id))!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            sut.WithName(name);

            // Assert
            Assert.Equal(name, sut.Key.Name);
            Assert.Null(sut.Key.Number);
            Assert.False(sut.Key.IsNameIgnored);
            Assert.Empty(sut.Key.AlternateNames);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenNameIsEmpty()
        {
            // Arrange
            var name = string.Empty;

            var propertyInfo = typeof(FakeModel).GetProperty(nameof(FakeModel.Id))!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            var exception = Record.Exception(() => sut.WithName(name));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenNameIsNull()
        {
            // Arrange
            var name = default(string);

            var propertyInfo = typeof(FakeModel).GetProperty(nameof(FakeModel.Id))!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            var exception = Record.Exception(() => sut.WithName(name!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheWithoutNameMethod : PropertyMapKeyBuilderFacts
    {
        [Fact]
        public void MarksNameAsIgnored()
        {
            // Arrange
            var name = nameof(FakeModel.Id);
            var propertyInfo = typeof(FakeModel).GetProperty(name)!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            sut.WithoutName();

            // Assert
            Assert.Equal(name, sut.Key.Name);
            Assert.Null(sut.Key.Number);
            Assert.True(sut.Key.IsNameIgnored);
            Assert.Empty(sut.Key.AlternateNames);
        }
    }

    public class TheWithNumberMethod : PropertyMapKeyBuilderFacts
    {
        [Fact]
        public void OverridesNumber()
        {
            // Arrange
            var number = 3U;

            var name = nameof(FakeModel.Id);
            var propertyInfo = typeof(FakeModel).GetProperty(name)!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            sut.WithNumber(number);

            // Assert
            Assert.Equal(name, sut.Key.Name);
            Assert.Equal(number, sut.Key.Number);
            Assert.False(sut.Key.IsNameIgnored);
            Assert.Empty(sut.Key.AlternateNames);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenNumberIsZero()
        {
            // Arrange
            var number = 0U;

            var propertyInfo = typeof(FakeModel).GetProperty(nameof(FakeModel.Id))!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            var exception = Record.Exception(() => sut.WithNumber(number));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }

    public class TheWithAlternateNamesMethod : PropertyMapKeyBuilderFacts
    {
        [Fact]
        public void OverridesAlternateNames()
        {
            // Arrange
            var alternateNameOne = "foo";
            var alternateNameTwo = "Bar";
            var alternateNameThree = "baZ";

            var name = nameof(FakeModel.Id);
            var propertyInfo = typeof(FakeModel).GetProperty(name)!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            sut.WithAlternateNames(new[] { alternateNameOne, alternateNameTwo, alternateNameThree, });

            // Assert
            Assert.Equal(name, sut.Key.Name);
            Assert.Null(sut.Key.Number);
            Assert.False(sut.Key.IsNameIgnored);

            Assert.Equal(3, sut.Key.AlternateNames.Count);
            Assert.Single(sut.Key.AlternateNames, alternateName => alternateName == alternateNameOne);
            Assert.Single(sut.Key.AlternateNames, alternateName => alternateName == alternateNameTwo);
            Assert.Single(sut.Key.AlternateNames, alternateName => alternateName == alternateNameThree);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenAnyNameIsEmpty()
        {
            // Arrange
            var names = new[] { "foo", string.Empty, };

            var propertyInfo = typeof(FakeModel).GetProperty(nameof(FakeModel.Id))!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            var exception = Record.Exception(() => sut.WithAlternateNames(names));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenNameCollectionIsNull()
        {
            // Arrange
            var names = default(string[]);

            var propertyInfo = typeof(FakeModel).GetProperty(nameof(FakeModel.Id))!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            var exception = Record.Exception(() => sut.WithAlternateNames(names!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenAnyNameIsNull()
        {
            // Arrange
            var names = new[] { "foo", default, };

            var propertyInfo = typeof(FakeModel).GetProperty(nameof(FakeModel.Id))!;
            var sut = CreateSystemUnderTest(propertyInfo);

            // Act
            var exception = Record.Exception(() => sut.WithAlternateNames(names!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
