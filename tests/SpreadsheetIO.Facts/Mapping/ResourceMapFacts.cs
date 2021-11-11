using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping;

public class ResourceMapFacts
{
    private readonly AutoMocker _mocker = new();

    private FakeModelMap CreateSystemUnderTest()
        => _mocker.CreateInstance<FakeModelMap>();

    public class TheResourceTypeProperty : ResourceMapFacts
    {
        [Fact]
        public void ReturnsTheGenericParameter()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var resourceType = sut.ResourceType;

            // Assert
            Assert.Equal(typeof(FakeModel), resourceType);
        }
    }

    public class TheOptionsProperty : ResourceMapFacts
    {
        [Fact]
        public void FreezesOptionsOnFirstGet()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var options = sut.Options;

            // Assert
            Assert.True(options.IsFrozen);
        }
    }

    public class TheMapMethodWithPropertyParameter : ResourceMapFacts
    {
        [Fact]
        public void AddsPropertyMap()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            sut.Map(model => model.Name);

            // Assert
            var propertyMap = Assert.Single(sut.Properties);

            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Name)), propertyMap.Property);

            Assert.Equal(nameof(FakeModel.Name), propertyMap.Key.Name);
            Assert.Null(propertyMap.Key.Number);
            Assert.False(propertyMap.Key.IsNameIgnored);

            Assert.Empty(propertyMap.Options.Extensions);
            Assert.True(propertyMap.Options.IsFrozen);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotMemberExpression()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Map(model => true));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotPropertyInfo()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Map(model => model.Field));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsAlreadyMapped()
        {
            // Arrange
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Name);

            // Act
            var exception = Record.Exception(() => sut.Map(model => model.Name));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }

    public class TheMapMethodWithPropertyAndKeyActionParameters : ResourceMapFacts
    {
        [Fact]
        public void AddsPropertyMapWithResultingKey()
        {
            // Arrange
            var keyNumber = 1U;

            var sut = CreateSystemUnderTest();

            // Act
            sut.Map(model => model.Name, keyAction => keyAction.UseNumber(keyNumber));

            // Assert
            var propertyMap = Assert.Single(sut.Properties);

            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Name)), propertyMap.Property);

            Assert.Equal(nameof(FakeModel.Name), propertyMap.Key.Name);
            Assert.Equal(keyNumber, propertyMap.Key.Number);
            Assert.False(propertyMap.Key.IsNameIgnored);

            Assert.Empty(propertyMap.Options.Extensions);
            Assert.True(propertyMap.Options.IsFrozen);
        }

        [Fact]
        public void AddsPropertyMapWithDefaultKeyWhenKeyActionIsNull()
        {
            // Arrange
            var keyAction = default(Action<PropertyMapKeyBuilder>);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Map(model => model.Name, keyAction!);

            // Assert
            var propertyMap = Assert.Single(sut.Properties);

            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Name)), propertyMap.Property);

            Assert.Equal(nameof(FakeModel.Name), propertyMap.Key.Name);
            Assert.Null(propertyMap.Key.Number);
            Assert.False(propertyMap.Key.IsNameIgnored);

            Assert.Empty(propertyMap.Options.Extensions);
            Assert.True(propertyMap.Options.IsFrozen);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotMemberExpression()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Map(model => true, keyAction => keyAction.UseNumber(1U)));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotPropertyInfo()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Map(model => model.Field, keyAction => keyAction.UseNumber(1U)));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsAlreadyMapped()
        {
            // Arrange
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Name);

            // Act
            var exception = Record.Exception(() => sut.Map(model => model.Name, keyAction => keyAction.UseNumber(1U)));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyKeyNameIsAlreadyDefined()
        {
            // Arrange
            var keyName = "Foo";
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Id, keyAction => keyAction.IgnoreName());
            sut.Map(model => model.Display, keyAction => keyAction.OverrideName(keyName));

            // Act
            var exception = Record.Exception(() => sut.Map(model => model.Name, keyAction => keyAction.OverrideName(keyName)));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyKeyNumberIsAlreadyDefined()
        {
            // Arrange
            var keyNumber = 1U;
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Id, keyAction => keyAction.UseNumber(keyNumber));

            // Act
            var exception = Record.Exception(() => sut.Map(model => model.Name, keyAction => keyAction.UseNumber(keyNumber)));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }

    public class TheMapMethodWithPropertyAndOptionsActionParameters : ResourceMapFacts
    {
        [Fact]
        public void AddsPropertyMapWithResultingOptions()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            sut.Map(model => model.Name, optionsAction => optionsAction.MarkAsOptional());

            // Assert
            var propertyMap = Assert.Single(sut.Properties);

            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Name)), propertyMap.Property);

            Assert.Equal(nameof(FakeModel.Name), propertyMap.Key.Name);
            Assert.Null(propertyMap.Key.Number);
            Assert.False(propertyMap.Key.IsNameIgnored);

            Assert.True(propertyMap.Options.HasExtension<OptionalPropertyMapOptionsExtension>());
            Assert.True(propertyMap.Options.IsFrozen);
        }

        [Fact]
        public void AddsPropertyMapWithDefaultOptionsWhenOptionsActionIsNull()
        {
            // Arrange
            var optionsAction = default(Action<PropertyMapOptionsBuilder<FakeModel, string>>);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Map(model => model.Name, optionsAction!);

            // Assert
            var propertyMap = Assert.Single(sut.Properties);

            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Name)), propertyMap.Property);

            Assert.Equal(nameof(FakeModel.Name), propertyMap.Key.Name);
            Assert.Null(propertyMap.Key.Number);
            Assert.False(propertyMap.Key.IsNameIgnored);

            Assert.Empty(propertyMap.Options.Extensions);
            Assert.True(propertyMap.Options.IsFrozen);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotMemberExpression()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Map(model => true, optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotPropertyInfo()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(()
                => sut.Map(model => model.Field, optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsAlreadyMapped()
        {
            // Arrange
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Name);

            // Act
            var exception = Record.Exception(() => sut.Map(model => model.Name, optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }

    public class TheMapMethodWithPropertyAndKeyActionAndOptionsActionParameters : ResourceMapFacts
    {
        [Fact]
        public void AddsPropertyMapWithResultingKeyAndOptions()
        {
            // Arrange
            var keyNumber = 1U;

            var sut = CreateSystemUnderTest();

            // Act
            sut.Map(
                model => model.Name,
                keyAction => keyAction.UseNumber(keyNumber),
                optionsAction => optionsAction.MarkAsOptional());

            // Assert
            var propertyMap = Assert.Single(sut.Properties);

            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Name)), propertyMap.Property);

            Assert.Equal(nameof(FakeModel.Name), propertyMap.Key.Name);
            Assert.Equal(keyNumber, propertyMap.Key.Number);
            Assert.False(propertyMap.Key.IsNameIgnored);

            Assert.True(propertyMap.Options.HasExtension<OptionalPropertyMapOptionsExtension>());
            Assert.True(propertyMap.Options.IsFrozen);
        }

        [Fact]
        public void AddsPropertyMapWithDefaultKeyAndOptionsWhenKeyActionAndOptionsActionAreNull()
        {
            // Arrange
            var keyAction = default(Action<PropertyMapKeyBuilder>);
            var optionsAction = default(Action<PropertyMapOptionsBuilder<FakeModel, string>>);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Map(model => model.Name, keyAction!, optionsAction!);

            // Assert
            var propertyMap = Assert.Single(sut.Properties);

            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Name)), propertyMap.Property);

            Assert.Equal(nameof(FakeModel.Name), propertyMap.Key.Name);
            Assert.Null(propertyMap.Key.Number);
            Assert.False(propertyMap.Key.IsNameIgnored);

            Assert.Empty(propertyMap.Options.Extensions);
            Assert.True(propertyMap.Options.IsFrozen);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotMemberExpression()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(()
                => sut.Map(
                    model => true,
                    keyAction => keyAction.UseNumber(1U),
                    optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotPropertyInfo()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(()
                => sut.Map(
                    model => model.Field,
                    keyAction => keyAction.UseNumber(1U),
                    optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsAlreadyMapped()
        {
            // Arrange
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Name);

            // Act
            var exception = Record.Exception(()
                => sut.Map(
                    model => model.Name,
                    keyAction => keyAction.UseNumber(1U),
                    optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyKeyNameIsAlreadyDefined()
        {
            // Arrange
            var keyName = "Foo";
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Id, keyAction => keyAction.IgnoreName());
            sut.Map(model => model.Display, keyAction => keyAction.OverrideName(keyName));

            // Act
            var exception = Record.Exception(()
                => sut.Map(
                    model => model.Name,
                    keyAction => keyAction.OverrideName(keyName),
                    optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyKeyNumberIsAlreadyDefined()
        {
            // Arrange
            var keyNumber = 1U;
            var sut = CreateSystemUnderTest();
            sut.Map(model => model.Id, keyAction => keyAction.UseNumber(keyNumber));

            // Act
            var exception = Record.Exception(()
                => sut.Map(
                    model => model.Name,
                    keyAction => keyAction.UseNumber(keyNumber),
                    optionsAction => optionsAction.MarkAsOptional()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }
}
