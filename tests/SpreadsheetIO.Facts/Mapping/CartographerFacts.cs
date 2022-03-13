using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping;

public class CartographerFacts
{
    private readonly AutoMocker _mocker = new();

    private Cartographer CreateSystemUnderTest()
        => _mocker.CreateInstance<Cartographer>(true);

    public class TheGetMapMethod : CartographerFacts
    {
        [Fact]
        public void ReturnsResourceMapForSuccessResult()
        {
            // Arrange
            var expectedResourceMap = ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>());

            _mocker.Use<IDictionary<Type, ResourceMapResult>>(
                new Dictionary<Type, ResourceMapResult>
                {
                    [typeof(FakeModel)] = ResourceMapResult.Success(expectedResourceMap),
                });

            var sut = CreateSystemUnderTest();

            // Act
            var actualResourceMap = sut.GetMap<FakeModel>();

            // Assert
            Assert.Equal(expectedResourceMap, actualResourceMap);
        }

        [Fact]
        public void ThrowsInvalidResourceMapExceptionForFailureResult()
        {
            // Arrange
            var resourceType = typeof(FakeModel);
            var resourceMapError = ResourceMapErrorCreator.Create();

            _mocker.Use<IDictionary<Type, ResourceMapResult>>(
                new Dictionary<Type, ResourceMapResult>
                {
                    [resourceType] = ResourceMapResult.Failure(resourceMapError),
                });

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.GetMap<FakeModel>());

            // Assert
            Assert.NotNull(exception);
            var invalidResourceMapException = Assert.IsType<InvalidResourceMapException>(exception);
            Assert.Equal(resourceType, invalidResourceMapException.ResourceType);
            Assert.Equal(resourceMapError, invalidResourceMapException.Error);
        }

        [Fact]
        public void ThrowsKeyNotFoundExceptionWhenResultIsNotFound()
        {
            // Arrange
            _mocker.Use<IDictionary<Type, ResourceMapResult>>(
                new Dictionary<Type, ResourceMapResult>
                {
                    [typeof(FakeModel)] = ResourceMapResult.Success(ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>())),
                });

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.GetMap<FakeConstructionModel>());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }
    }
}
