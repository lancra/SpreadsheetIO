using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal;

public class ResourcePropertyDefaultValueResolverFacts
{
    private readonly AutoMocker _mocker = new();

    private ResourcePropertyDefaultValueResolver CreateSystemUnderTest()
        => _mocker.CreateInstance<ResourcePropertyDefaultValueResolver>();

    public class TheTryResolveMethod : ResourcePropertyDefaultValueResolverFacts
    {
        [Fact]
        public void ReturnsTrueWhenDefaultValueMapOptionHasCorrespondingResolutionToParseResult()
        {
            // Arrange
            var expectedValue = 1.5M;

            var map = PropertyMapCreator.CreateForFakeModel(
                model => model.Decimal,
                options: new DefaultValuePropertyMapOption(
                    expectedValue,
                    new[]
                    {
                        ResourcePropertyDefaultReadingResolution.Empty,
                        ResourcePropertyDefaultReadingResolution.Missing,
                        ResourcePropertyDefaultReadingResolution.Invalid,
                    }));
            var parseResultKind = ResourcePropertyParseResultKind.Invalid;

            var sut = CreateSystemUnderTest();

            // Act
            var hasDefault = sut.TryResolve(map, parseResultKind, out var actualValue);

            // Assert
            Assert.True(hasDefault);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void ReturnsFalseWhenParseResultIsSuccess()
        {
            // Arrange
            var defaultValue = 1.5M;

            var map = PropertyMapCreator.CreateForFakeModel(
                model => model.Decimal,
                options: new DefaultValuePropertyMapOption(
                    defaultValue,
                    new[]
                    {
                        ResourcePropertyDefaultReadingResolution.Empty,
                        ResourcePropertyDefaultReadingResolution.Missing,
                        ResourcePropertyDefaultReadingResolution.Invalid,
                    }));
            var parseResultKind = ResourcePropertyParseResultKind.Success;

            var sut = CreateSystemUnderTest();

            // Act
            var hasDefault = sut.TryResolve(map, parseResultKind, out var value);

            // Assert
            Assert.False(hasDefault);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsFalseWhenDefaultValueMapOptionIsNotFound()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeModel(model => model.Decimal);
            var parseResultKind = ResourcePropertyParseResultKind.Invalid;

            var sut = CreateSystemUnderTest();

            // Act
            var hasDefault = sut.TryResolve(map, parseResultKind, out var value);

            // Assert
            Assert.False(hasDefault);
            Assert.Null(value);
        }

        [Fact]
        public void ReturnsFalseWhenParseResultDoesNotMatchAnyResolutionsDefinedInDefaultValueMapOption()
        {
            // Arrange
            var defaultValue = 1.5M;

            var map = PropertyMapCreator.CreateForFakeModel(
                model => model.Decimal,
                options: new DefaultValuePropertyMapOption(
                    defaultValue,
                    new[]
                    {
                        ResourcePropertyDefaultReadingResolution.Empty,
                        ResourcePropertyDefaultReadingResolution.Missing,
                    }));
            var parseResultKind = ResourcePropertyParseResultKind.Invalid;

            var sut = CreateSystemUnderTest();

            // Act
            var hasDefault = sut.TryResolve(map, parseResultKind, out var value);

            // Assert
            Assert.False(hasDefault);
            Assert.Null(value);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenMapIsNull()
        {
            // Arrange
            var map = default(PropertyMap);
            var parseResultKind = ResourcePropertyParseResultKind.Invalid;

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.TryResolve(map!, parseResultKind, out var value));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenParseResultKindIsNull()
        {
            // Arrange
            var map = PropertyMapCreator.CreateForFakeModel(model => model.Decimal);
            var parseResultKind = default(ResourcePropertyParseResultKind);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.TryResolve(map, parseResultKind!, out var value));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
