using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Creation;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Creation;

public class ConstructorResourceCreationStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private ConstructorResourceCreationStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<ConstructorResourceCreationStrategy>();

    public class TheApplicabilityHandlerProperty : ConstructorResourceCreationStrategyFacts
    {
        [Fact]
        public void ReturnsTrueWhenOptionsContainsConstructorOption()
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
            Assert.True(isApplicable);
        }

        [Fact]
        public void ReturnsFalseWhenOptionsDoesNotContainConstructorOption()
        {
            // Arrange
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(Array.Empty<PropertyMap>());
            var sut = CreateSystemUnderTest();

            // Act
            var isApplicable = sut.ApplicabilityHandler(resourceMap);

            // Assert
            Assert.False(isApplicable);
        }
    }

    public class TheCreateMethod : ConstructorResourceCreationStrategyFacts
    {
        [Fact]
        public void ReturnsResource()
        {
            // Arrange
            var expectedModel = new FakeConstructionModel(1, "foo", 2.5M);

            var idPropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Id);
            var namePropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Name);
            var amountPropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(
                model => model.Amount,
                new OptionalPropertyMapOption(PropertyElementKind.All));
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                new[]
                {
                    idPropertyMap,
                    namePropertyMap,
                    amountPropertyMap,
                },
                new ConstructorResourceMapOption(
                    typeof(FakeConstructionModel).GetConstructor(new[] { typeof(int), typeof(string), typeof(decimal), })!,
                    new[]
                    {
                        idPropertyMap.Key,
                        namePropertyMap.Key,
                        amountPropertyMap.Key,
                    }));

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
        public void UsesMissingValueWhenOptionalPropertyHasNoProvidedValue()
        {
            // Arrange
            var expectedModel = new FakeConstructionModel(1, "foo");

            var idPropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Id);
            var namePropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Name);
            var amountPropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(
                model => model.Amount,
                new OptionalPropertyMapOption(PropertyElementKind.All));
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                new[]
                {
                    idPropertyMap,
                    namePropertyMap,
                    amountPropertyMap,
                },
                new ConstructorResourceMapOption(
                    typeof(FakeConstructionModel).GetConstructor(new[] { typeof(int), typeof(string), typeof(decimal), })!,
                    new[]
                    {
                        idPropertyMap.Key,
                        namePropertyMap.Key,
                        amountPropertyMap.Key,
                    }));

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

        [Fact]
        public void ReturnsNullWhenRequiredPropertySpecifiedForConstructorHasNoValue()
        {
            // Arrange
            var idPropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Id);
            var namePropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Name);
            var amountPropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(
                model => model.Amount,
                new OptionalPropertyMapOption(PropertyElementKind.All));
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                new[]
                {
                    idPropertyMap,
                    namePropertyMap,
                    amountPropertyMap,
                },
                new ConstructorResourceMapOption(
                    typeof(FakeConstructionModel).GetConstructor(new[] { typeof(int), typeof(string), typeof(decimal), })!,
                    new[]
                    {
                        idPropertyMap.Key,
                        namePropertyMap.Key,
                        amountPropertyMap.Key,
                    }));

            var values = new ResourcePropertyValues();
            values.Add(idPropertyMap, 1);

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create<FakeConstructionModel>(resourceMap, values);

            // Assert
            Assert.Null(actualModel);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertySpecifiedForConstructorHasNoMap()
        {
            // Arrange
            var expectedModel = new FakeConstructionModel(1, "foo");

            var idPropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Id);
            var namePropertyMap = PropertyMapCreator.CreateForFakeConstructionModel(model => model.Name);
            var amountPropertyKey = PropertyMapKeyCreator.Create(name: nameof(FakeConstructionModel.Amount));
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                new[]
                {
                    idPropertyMap,
                    namePropertyMap,
                },
                new ConstructorResourceMapOption(
                    typeof(FakeConstructionModel).GetConstructor(new[] { typeof(int), typeof(string), typeof(decimal), })!,
                    new[]
                    {
                        idPropertyMap.Key,
                        namePropertyMap.Key,
                        amountPropertyKey,
                    }));

            var values = new ResourcePropertyValues();
            values.Add(idPropertyMap, expectedModel.Id);
            values.Add(namePropertyMap, expectedModel.Name);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Create<FakeConstructionModel>(resourceMap, values));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(Messages.MissingResourcePropertyForConstructorParameter(amountPropertyKey.Name), exception.Message);
        }
    }
}
