using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Reading.Internal.Creation;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Creation;

public class PropertySettersResourceCreationStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private PropertySettersResourceCreationStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<PropertySettersResourceCreationStrategy>();

    public class TheApplicabilityHandlerProperty : PropertySettersResourceCreationStrategyFacts
    {
        [Fact]
        public void ReturnsTrueWhenOptionsDoesNotContainConstructorExtension()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap();
            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap.Options);

            // Assert
            Assert.True(isApplicable);
        }

        [Fact]
        public void ReturnsFalseWhenOptionsContainsExplicitConstructorExtension()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(
                optionsBuilder => optionsBuilder.UseExplicitConstructor(
                    nameof(FakeConstructionModel.Id),
                    nameof(FakeConstructionModel.Name),
                    nameof(FakeConstructionModel.Amount)));

            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap.Options);

            // Assert
            Assert.False(isApplicable);
        }

        [Fact]
        public void ReturnsFalseWhenOptionsContainsImplicitConstructorExtension()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(optionsBuilder => optionsBuilder.UseImplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap.Options);

            // Assert
            Assert.False(isApplicable);
        }
    }

    public class TheCreateMethod : PropertySettersResourceCreationStrategyFacts
    {
        [Fact]
        public void ReturnsResource()
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
        public void ReturnsPartiallyCreatedResourceFromPropertySettersWhenSomeMappedPropertiesDoNotHaveValues()
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
    }
}
