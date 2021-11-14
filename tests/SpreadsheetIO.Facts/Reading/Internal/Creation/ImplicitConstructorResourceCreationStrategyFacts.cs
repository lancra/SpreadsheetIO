using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Reading.Internal.Creation;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Creation;

public class ImplicitConstructorResourceCreationStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private ImplicitConstructorResourceCreationStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<ImplicitConstructorResourceCreationStrategy>();

    public class TheApplicabilityHandlerProperty : ImplicitConstructorResourceCreationStrategyFacts
    {
        [Fact]
        public void ReturnsTrueWhenOptionsContainsImplicitConstructorExtension()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap(optionsBuilder => optionsBuilder.UseImplicitConstructor());
            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap.Options);

            // Assert
            Assert.True(isApplicable);
        }

        [Fact]
        public void ReturnsFalseWhenOptionsDoesNotContainImplicitConstructorExtension()
        {
            // Arrange
            var resourceMap = new FakeConstructionModelMap();
            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap.Options);

            // Assert
            Assert.False(isApplicable);
        }
    }

    public class TheCreateMethod : ImplicitConstructorResourceCreationStrategyFacts
    {
        [Fact]
        public void ReturnsResource()
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
        public void ReturnsNullWhenMappPropertyHasNoPropertyValue()
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
    }
}
