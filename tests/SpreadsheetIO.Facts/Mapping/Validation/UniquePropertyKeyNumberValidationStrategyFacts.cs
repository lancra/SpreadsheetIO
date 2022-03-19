using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Extensions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation;

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
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Id), number: 1U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Name),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Name), number: 2U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Decimal),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Decimal), number: 3U, isNameIgnored: true)).Object,
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
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Id), number: 1U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Name),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Name), number: 2U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Display),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Display), number: 1U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Decimal),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Decimal), number: 3U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.DateTime),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.DateTime), number: 3U, isNameIgnored: true)).Object,
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
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Id), number: 1U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Name),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Name), number: 2U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Display),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Display))).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.Decimal),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.Decimal), number: 3U, isNameIgnored: true)).Object,
                        _mocker.GetMockForInternalPropertyMapBuilder(
                            resourceType,
                            nameof(FakeModel.DateTime),
                            PropertyMapKeyCreator.Create(name: nameof(FakeModel.DateTime))).Object,
                    });

            var sut = CreateSystemUnderTest();

            // Act
            var validationResult = sut.Validate(resourceMapBuilderMock.Object);

            // Assert
            Assert.True(validationResult.IsValid);
        }
    }
}
