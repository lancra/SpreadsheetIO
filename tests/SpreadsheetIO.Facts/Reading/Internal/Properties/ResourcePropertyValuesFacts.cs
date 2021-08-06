using System;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Properties
{
    public class ResourcePropertyValuesFacts
    {
        private readonly AutoMocker _mocker = new();

        private ResourcePropertyValues<FakeModel> CreateSystemUnderTest()
            => _mocker.CreateInstance<ResourcePropertyValues<FakeModel>>();

        public class TheAddMethod : ResourcePropertyValuesFacts
        {
            [Fact]
            public void AddsValueForMap()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var expectedValue = 1;

                var sut = CreateSystemUnderTest();

                // Act
                sut.Add(map, expectedValue);

                // Assert
                var hasMap = sut.TryGetValue(map, out var actualValue);
                Assert.True(hasMap);
                Assert.Equal(expectedValue, actualValue);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenMapIsNull()
            {
                // Arrange
                var map = default(PropertyMap<FakeModel>);
                var value = 1;

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Add(map!, value));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheTryGetValueMethod : ResourcePropertyValuesFacts
        {
            [Fact]
            public void ReturnsTrueWhenValueIsFoundForMap()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);
                var expectedValue = 1;

                var sut = CreateSystemUnderTest();
                sut.Add(map, expectedValue);

                // Act
                var hasMap = sut.TryGetValue(map, out var actualValue);

                // Assert
                Assert.True(hasMap);
                Assert.Equal(expectedValue, actualValue);
            }

            [Fact]
            public void ReturnsFalseWhenValueIsNotFoundForMap()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Id);

                var sut = CreateSystemUnderTest();

                // Act
                var hasMap = sut.TryGetValue(map, out var value);

                // Assert
                Assert.False(hasMap);
                Assert.Null(value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenMapIsNull()
            {
                // Arrange
                var map = default(PropertyMap<FakeModel>);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.TryGetValue(map!, out var value));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }
    }
}
