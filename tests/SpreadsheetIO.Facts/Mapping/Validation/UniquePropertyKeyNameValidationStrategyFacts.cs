using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Extensions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation;

public class UniquePropertyKeyNameValidationStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private UniquePropertyKeyNameValidationStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<UniquePropertyKeyNameValidationStrategy>();

    public class TheValidateMethod : UniquePropertyKeyNameValidationStrategyFacts
    {
        [Fact]
        public void ReturnsSuccessResultWhenNoPropertyKeyNamesAreDuplicated()
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
        public void ReturnsFailureResultWhenOneOrMorePropertyKeyNamesAreDuplicated()
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
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Name)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Display),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Id))).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Decimal)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.DateTime),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Decimal))).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(Messages.DuplicatePropertyMapKeyNames(resourceType, "Id,Decimal"), validationResult.Message);
        }

        [Fact]
        public void DoesNotConsiderIgnoredKeyNamesWhenValidating()
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
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Display),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Id), isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(resourceType, nameof(FakeModel.Decimal)).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }
    }
}
