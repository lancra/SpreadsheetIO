using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Reading.Internal.Creation;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Creation;

public class ResourceCreatorFacts
{
    private readonly AutoMocker _mocker = new();

    private ResourceCreator CreateSystemUnderTest()
        => _mocker.CreateInstance<ResourceCreator>();

    public class TheCreateMethod : ResourceCreatorFacts
    {
        [Fact]
        public void ReturnsMatchingStrategyResult()
        {
            // Arrange
            var resourceMap = ResourceMapCreator.Create<FakeConstructionModel>(
                Array.Empty<PropertyMap>(),
                new ContinueOnResourceReadingFailureResourceMapOption());
            var fakeValues = _mocker.GetMock<IResourcePropertyValues>();
            var expectedModel = new FakeConstructionModel(1, "One", 1.11M);

            var matchingStrategy = new Mock<IResourceCreationStrategy>();
            matchingStrategy.SetupGet(strategy => strategy.ApplicabilityHandler)
                .Returns(map => map.Options.Has<ContinueOnResourceReadingFailureResourceMapOption>());
            matchingStrategy.Setup(strategy => strategy.Create<FakeConstructionModel>(resourceMap, fakeValues.Object))
                .Returns(expectedModel);

            var unmatchingStrategy = new Mock<IResourceCreationStrategy>();
            unmatchingStrategy.SetupGet(strategy => strategy.ApplicabilityHandler)
                .Returns(map => !map.Options.Has<ContinueOnResourceReadingFailureResourceMapOption>());

            _mocker.Use<IEnumerable<IResourceCreationStrategy>>(
                new[]
                {
                    matchingStrategy.Object,
                    unmatchingStrategy.Object,
                });

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create<FakeConstructionModel>(resourceMap, fakeValues.Object);

            // Assert
            Assert.Equal(expectedModel, actualModel);
        }
    }
}
