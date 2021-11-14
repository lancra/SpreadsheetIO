using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Properties;
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
            var resourceMap = new FakeConstructionModelMap(options => options.ExitOnResourceReadingFailure());
            var fakeValues = _mocker.GetMock<IResourcePropertyValues<FakeConstructionModel>>();
            var expectedModel = new FakeConstructionModel(1, "One", 1.11M);

            var matchingStrategy = new Mock<IResourceCreationStrategy>();
            matchingStrategy.SetupGet(strategy => strategy.ApplicabilityHandler)
                .Returns(options => options.HasExtension<ExitOnResourceReadingFailureResourceMapOptionsExtension>());
            matchingStrategy.Setup(strategy => strategy.Create(resourceMap, fakeValues.Object))
                .Returns(expectedModel);

            var unmatchingStrategy = new Mock<IResourceCreationStrategy>();
            unmatchingStrategy.SetupGet(strategy => strategy.ApplicabilityHandler)
                .Returns(options => !options.HasExtension<ExitOnResourceReadingFailureResourceMapOptionsExtension>());

            _mocker.Use<IEnumerable<IResourceCreationStrategy>>(
                new[]
                {
                    matchingStrategy.Object,
                    unmatchingStrategy.Object,
                });

            var sut = CreateSystemUnderTest();

            // Act
            var actualModel = sut.Create(resourceMap, fakeValues.Object);

            // Assert
            Assert.Equal(expectedModel, actualModel);
        }
    }
}
