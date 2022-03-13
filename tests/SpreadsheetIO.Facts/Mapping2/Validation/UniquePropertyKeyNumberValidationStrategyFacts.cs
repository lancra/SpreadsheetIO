using LanceC.SpreadsheetIO.Facts.Testing.Extensions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Validation;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2.Validation;

public class UniquePropertyKeyNumberValidationStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private UniquePropertyKeyNumberValidationStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<UniquePropertyKeyNumberValidationStrategy>();

    public class TheValidateMethod : UniquePropertyKeyNumberValidationStrategyFacts
    {
        [Fact]
        public void ReturnsSuccessResultWhenNoPropertyKeyNumbersAreDuplicated()
        {
            // Arrange
            var resourceType = typeof(FakeModel);

            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Id),
                            new PropertyMapKey(nameof(FakeModel.Id), 1U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Name),
                            new PropertyMapKey(nameof(FakeModel.Name), 2U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Decimal),
                            new PropertyMapKey(nameof(FakeModel.Decimal), 3U, true)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsFailureResultWhenOneOrMorePropertyKeyNumbersAreDuplicated()
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
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Id),
                            new PropertyMapKey(nameof(FakeModel.Id), 1U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Name),
                            new PropertyMapKey(nameof(FakeModel.Name), 2U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Display),
                            new PropertyMapKey(nameof(FakeModel.Display), 1U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Decimal),
                            new PropertyMapKey(nameof(FakeModel.Decimal), 3U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.DateTime),
                            new PropertyMapKey(nameof(FakeModel.DateTime), 3U, true)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(Messages.DuplicatePropertyMapKeyNumbers(resourceType, "1,3"), validationResult.Message);
        }

        [Fact]
        public void DoesNotConsiderUnspecifiedKeyNumbersWhenValidating()
        {
            // Arrange
            var resourceType = typeof(FakeModel);

            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();
            resourceMapBuilderMock.SetupGet(builder => builder.Properties)
                .Returns(
                    new[]
                    {
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Id),
                            new PropertyMapKey(nameof(FakeModel.Id), 1U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Name),
                            new PropertyMapKey(nameof(FakeModel.Name), 2U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Display),
                            new PropertyMapKey(nameof(FakeModel.Display), default, false)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Decimal),
                            new PropertyMapKey(nameof(FakeModel.Decimal), 3U, true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.DateTime),
                            new PropertyMapKey(nameof(FakeModel.DateTime), default, false)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }
    }
}
