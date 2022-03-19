using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
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
        public void ReturnsTrueWhenOptionsDoesNotContainConstructorOption()
        {
            // Arrange
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(Array.Empty<PropertyMap>());
            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap);

            // Assert
            Assert.True(isApplicable);
        }

        [Fact]
        public void ReturnsFalseWhenOptionsContainsConstructorOption()
        {
            // Arrange
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                Array.Empty<PropertyMap>(),
                new ConstructorResourceMapOption(
                    typeof(FakeConstructionModel).GetConstructor(new[] { typeof(int), typeof(string), typeof(decimal), })!,
                    new[]
                    {
                        PropertyMapKeyCreator.Create(name: nameof(FakeConstructionModel.Id)),
                        PropertyMapKeyCreator.Create(name: nameof(FakeConstructionModel.Name)),
                        PropertyMapKeyCreator.Create(name: nameof(FakeConstructionModel.Amount)),
                    }));

            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap);

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

            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                new[]
                {
                    PropertyMapCreator.CreateForFakeConstructionModel(model => model.Id),
                    PropertyMapCreator.CreateForFakeConstructionModel(model => model.Name),
                    PropertyMapCreator.CreateForFakeConstructionModel(model => model.Amount),
                });

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));
            var amountPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Amount));

            var values = new ResourcePropertyValues();
            values.Add(idPropertyMap, expectedModel.Id);
            values.Add(namePropertyMap, expectedModel.Name);
            values.Add(amountPropertyMap, expectedModel.Amount);

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create<FakeConstructionModel>(resourceMap, values);

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

            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                new[]
                {
                    PropertyMapCreator.CreateForFakeConstructionModel(model => model.Id),
                    PropertyMapCreator.CreateForFakeConstructionModel(model => model.Name),
                    PropertyMapCreator.CreateForFakeConstructionModel(model => model.Amount),
                });

            var idPropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Id));
            var namePropertyMap = resourceMap.Properties.Single(p => p.Key.Name == nameof(FakeConstructionModel.Name));

            var values = new ResourcePropertyValues();
            values.Add(idPropertyMap, expectedModel.Id);
            values.Add(namePropertyMap, expectedModel.Name);

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create<FakeConstructionModel>(resourceMap, values);

            // Assert
            Assert.NotNull(actualModel);
            Assert.Equal(expectedModel.Id, actualModel!.Id);
            Assert.Equal(expectedModel.Name, actualModel.Name);
            Assert.Equal(expectedModel.Amount, actualModel.Amount);
        }
    }
}
