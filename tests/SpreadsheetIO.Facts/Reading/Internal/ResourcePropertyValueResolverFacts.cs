using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal
{
    public class ResourcePropertyValueResolverFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private ResourcePropertyValueResolver CreateSystemUnderTest()
            => _mocker.CreateInstance<ResourcePropertyValueResolver>();

        public class TheTryResolveMethod : ResourcePropertyValueResolverFacts
        {
            [Fact]
            public void ReturnsTrueWhenParseResultKindIsValid()
            {
                // Arrange
                var cellValue = "1";
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);

                var parseResultKind = ResourcePropertyParseResultKind.Success;
                object? parsedValue = 1;
                _mocker.GetMock<IResourcePropertyParser>()
                    .Setup(parser => parser.TryParse(cellValue, map, out parsedValue))
                    .Returns(parseResultKind);

                object? defaultValue = default;
                _mocker.GetMock<IResourcePropertyDefaultValueResolver>()
                    .Setup(defaultValueResolver => defaultValueResolver.TryResolve(map, parseResultKind, out defaultValue))
                    .Returns(false);

                var sut = CreateSystemUnderTest();

                // Act
                var isResolved = sut.TryResolve(cellValue, map, out var actualValue);

                // Assert
                Assert.True(isResolved);
                Assert.Equal(parsedValue, actualValue);
            }

            [Fact]
            public void ReturnsTrueWhenParseResultKindIsInvalidButDefaultValueIsResolved()
            {
                // Arrange
                var cellValue = "foo";
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);

                var parseResultKind = ResourcePropertyParseResultKind.Invalid;
                object? parsedValue = null;
                _mocker.GetMock<IResourcePropertyParser>()
                    .Setup(parser => parser.TryParse(cellValue, map, out parsedValue))
                    .Returns(parseResultKind);

                object? defaultValue = 1;
                _mocker.GetMock<IResourcePropertyDefaultValueResolver>()
                    .Setup(defaultValueResolver => defaultValueResolver.TryResolve(map, parseResultKind, out defaultValue))
                    .Returns(true);

                var sut = CreateSystemUnderTest();

                // Act
                var isResolved = sut.TryResolve(cellValue, map, out var actualValue);

                // Assert
                Assert.True(isResolved);
                Assert.Equal(defaultValue, actualValue);
            }

            [Fact]
            public void ReturnsFalseWhenParseResultKindIsInvalidAndDefaultValueIsNotResolved()
            {
                // Arrange
                var cellValue = "foo";
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);

                var parseResultKind = ResourcePropertyParseResultKind.Invalid;
                object? parsedValue = default;
                _mocker.GetMock<IResourcePropertyParser>()
                    .Setup(parser => parser.TryParse(cellValue, map, out parsedValue))
                    .Returns(parseResultKind);

                object? defaultValue = default;
                _mocker.GetMock<IResourcePropertyDefaultValueResolver>()
                    .Setup(defaultValueResolver => defaultValueResolver.TryResolve(map, parseResultKind, out defaultValue))
                    .Returns(false);

                var sut = CreateSystemUnderTest();

                // Act
                var isResolved = sut.TryResolve(cellValue, map, out var actualValue);

                // Assert
                Assert.False(isResolved);
                Assert.Null(actualValue);
            }
        }
    }
}
