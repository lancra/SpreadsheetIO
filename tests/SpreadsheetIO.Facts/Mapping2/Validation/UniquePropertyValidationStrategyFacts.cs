using LanceC.SpreadsheetIO.Facts.Testing.Extensions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Validation;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Validation;

public class UniquePropertyValidationStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private UniquePropertyValidationStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<UniquePropertyValidationStrategy>();

    public class TheValidateMethod : UniquePropertyValidationStrategyFacts
    {
        [Fact]
        public void ReturnsSuccessResultWhenNoPropertiesAreDuplicated()
        {
            // Arrange
            var resourceType = typeof(FakeModel);

            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Name)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Decimal)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsFailureResultWhenOneOrMorePropertiesAreDuplicated()
        {
            // Arrange
            var resourceType = typeof(FakeModel);

            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.ResourceType)
                .Returns(resourceType);
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Id)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Name)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Decimal)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Decimal)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(Messages.DuplicatePropertyMaps(resourceType, "Id,Decimal"), validationResult.Message);
        }
    }
}
