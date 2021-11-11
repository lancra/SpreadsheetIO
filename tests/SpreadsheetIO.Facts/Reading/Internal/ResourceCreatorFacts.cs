using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal;

public class ResourceCreatorFacts
{
    private readonly AutoMocker _mocker = new();

    private ResourceCreator CreateSystemUnderTest()
        => _mocker.CreateInstance<ResourceCreator>();

    public class TheCreateMethod : ResourceCreatorFacts
    {
        [Fact]
        public void ReturnsResourceFromExplicitConstructorWhenAssociatedOptionsExtensionIsFound()
        {
            // Arrange
            var expectedModel = new FakeConstructionModel(1, "foo", 2.5M);

            var resourceMap = new FakeConstructionModelMap(
                optionsBuilder => optionsBuilder.UseExplicitConstructor(
                    nameof(FakeConstructionModel.Id),
                    nameof(FakeConstructionModel.Name),
                    nameof(FakeConstructionModel.Amount)))
                .Map(model => model.Id)
                .Map(model => model.Name)
                .Map(model => model.Amount);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));
            var amountPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Amount));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, expectedModel.Id);
            values.Add(namePropertyMap, expectedModel.Name);
            values.Add(amountPropertyMap, expectedModel.Amount);

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create(resourceMap, values);

            // Assert
            Assert.NotNull(actualModel);
            Assert.Equal(expectedModel.Id, actualModel!.Id);
            Assert.Equal(expectedModel.Name, actualModel.Name);
            Assert.Equal(expectedModel.Amount, actualModel.Amount);
        }

        [Fact]
        public void ReturnsResourceFromImplicitConstructorWhenAssociatedOptionsExtensionIsFound()
        {
            // Arrange
            var expectedModel = new FakeConstructionModel(1, "foo", 2.5M);

            var resourceMap = new FakeConstructionModelMap(optionsBuilder => optionsBuilder.UseImplicitConstructor())
                .Map(model => model.Id)
                .Map(model => model.Name)
                .Map(model => model.Amount);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));
            var amountPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Amount));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, expectedModel.Id);
            values.Add(namePropertyMap, expectedModel.Name);
            values.Add(amountPropertyMap, expectedModel.Amount);

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create(resourceMap, values);

            // Assert
            Assert.NotNull(actualModel);
            Assert.Equal(expectedModel.Id, actualModel!.Id);
            Assert.Equal(expectedModel.Name, actualModel.Name);
            Assert.Equal(expectedModel.Amount, actualModel.Amount);
        }

        [Fact]
        public void ReturnsResourceFromPropertySettersWhenNoAssociatedOptionsExtensionsAreFound()
        {
            // Arrange
            var expectedModel = new FakeConstructionModel(1, "foo", 2.5M);

            var resourceMap = new FakeConstructionModelMap()
                .Map(model => model.Id)
                .Map(model => model.Name)
                .Map(model => model.Amount);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));
            var amountPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Amount));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, expectedModel.Id);
            values.Add(namePropertyMap, expectedModel.Name);
            values.Add(amountPropertyMap, expectedModel.Amount);

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create(resourceMap, values);

            // Assert
            Assert.NotNull(actualModel);
            Assert.Equal(expectedModel.Id, actualModel!.Id);
            Assert.Equal(expectedModel.Name, actualModel.Name);
            Assert.Equal(expectedModel.Amount, actualModel.Amount);
        }

        [Fact]
        public void ReturnsNullWhenPropertyNameIsSpecifiedForExplicitConstructorAndNoPropertyValueIsFound()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(
                optionsBuilder => optionsBuilder.UseExplicitConstructor(
                    nameof(FakeConstructionModel.Id),
                    nameof(FakeConstructionModel.Name),
                    nameof(FakeConstructionModel.Amount)))
                .Map(model => model.Id)
                .Map(model => model.Name)
                .Map(model => model.Amount);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));
            var amountPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Amount));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, 1);
            values.Add(namePropertyMap, "foo");

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create(resourceMap, values);

            // Assert
            Assert.Null(actualModel);
        }

        [Fact]
        public void ReturnsNullWhenMappedPropertyForImplicitConstructorHasNoPropertyValue()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(optionsBuilder => optionsBuilder.UseImplicitConstructor())
                .Map(model => model.Id)
                .Map(model => model.Name)
                .Map(model => model.Amount);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, 1);
            values.Add(namePropertyMap, "foo");

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create(resourceMap, values);

            // Assert
            Assert.Null(actualModel);
        }

        [Fact]
        public void ReturnsPartiallyConstructedResourceFromPropertySettersWhenSomeMappedPropertiesDoNotHaveValues()
        {
            // Arrange
            var expectedModel = new FakeConstructionModel(1, "foo", default);

            var resourceMap = new FakeConstructionModelMap()
                .Map(model => model.Id)
                .Map(model => model.Name)
                .Map(model => model.Amount);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, expectedModel.Id);
            values.Add(namePropertyMap, expectedModel.Name);

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create(resourceMap, values);

            // Assert
            Assert.NotNull(actualModel);
            Assert.Equal(expectedModel.Id, actualModel!.Id);
            Assert.Equal(expectedModel.Name, actualModel.Name);
            Assert.Equal(expectedModel.Amount, actualModel.Amount);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyNameIsSpecifiedForExplicitConstructorAndThePropertyMapIsNotDefined()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(
                optionsBuilder => optionsBuilder.UseExplicitConstructor(
                    nameof(FakeConstructionModel.Id),
                    nameof(FakeConstructionModel.Name),
                    nameof(FakeConstructionModel.Amount)))
                .Map(model => model.Id)
                .Map(model => model.Name);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, 1);
            values.Add(namePropertyMap, "foo");

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Create(resourceMap, values));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenExplicitConstructorCannotBeFoundFromSpecifiedPropertyNames()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(
                optionsBuilder => optionsBuilder.UseExplicitConstructor(
                    nameof(FakeConstructionModel.Amount),
                    nameof(FakeConstructionModel.Name),
                    nameof(FakeConstructionModel.Id)))
                .Map(model => model.Id)
                .Map(model => model.Name)
                .Map(model => model.Amount);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));
            var amountPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Amount));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, 1);
            values.Add(namePropertyMap, "foo");
            values.Add(amountPropertyMap, 2.5M);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Create(resourceMap, values));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenImplicitConstructorCannotBeFoundFromMappedProperties()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(optionsBuilder => optionsBuilder.UseImplicitConstructor())
                .Map(model => model.Amount)
                .Map(model => model.Name)
                .Map(model => model.Id);

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));
            var amountPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Amount));

            var values = new ResourcePropertyValues<FakeConstructionModel>();
            values.Add(idPropertyMap, 1);
            values.Add(namePropertyMap, "foo");
            values.Add(amountPropertyMap, 2.5M);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Create(resourceMap, values));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }
}
