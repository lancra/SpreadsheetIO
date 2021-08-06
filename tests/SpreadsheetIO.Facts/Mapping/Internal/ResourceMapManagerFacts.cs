using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Internal;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Internal
{
    public class ResourceMapManagerFacts
    {
        private readonly AutoMocker _mocker = new();

        private ResourceMapManager CreateSystemUnderTest()
            => _mocker.CreateInstance<ResourceMapManager>();

        public class TheSingleMethodWithResourceGenericParameter : ResourceMapManagerFacts
        {
            [Fact]
            public void ReturnsMapForSpecifiedResourceType()
            {
                // Arrange
                var expectedResourceMap = new FakeStringResourceMap();
                _mocker.Use<IEnumerable<IResourceMap>>(new[] { expectedResourceMap, });

                var sut = CreateSystemUnderTest();

                // Act
                var actualResourceMap = sut.Single<string>();

                // Assert
                Assert.Equal(expectedResourceMap, actualResourceMap);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenNoMapsAreDefinedForTheSpecifiedResourceType()
            {
                // Arrange
                _mocker.Use<IEnumerable<IResourceMap>>(Array.Empty<IResourceMap>());

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Single<string>());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenMultipleMapsAreDefinedForTheSpecifiedResourceType()
            {
                // Arrange
                var resourceMaps = new IResourceMap[]
                {
                    new FakeStringResourceMap(),
                    new FakeOtherStringResourceMap(),
                };

                _mocker.Use<IEnumerable<IResourceMap>>(resourceMaps);

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Single<string>());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheSingleMethodWithResourceAndResourceMapGenericParameters : ResourceMapManagerFacts
        {
            [Fact]
            public void ReturnsMapForSpecifiedResourceAndMapTypes()
            {
                // Arrange
                var expectedResourceMap = new FakeStringResourceMap();
                _mocker.Use<IEnumerable<IResourceMap>>(new[] { expectedResourceMap, });

                var sut = CreateSystemUnderTest();

                // Act
                var actualResourceMap = sut.Single<string, FakeStringResourceMap>();

                // Assert
                Assert.Equal(expectedResourceMap, actualResourceMap);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenTheSpecifiedMapIsNotDefinedForTheSpecifiedResourceType()
            {
                // Arrange
                _mocker.Use<IEnumerable<IResourceMap>>(new IResourceMap[] { new FakeStringResourceMap(), });

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Single<string, FakeOtherStringResourceMap>());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }
    }
}
