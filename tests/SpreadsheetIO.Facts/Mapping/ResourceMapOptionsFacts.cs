using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping
{
    public class ResourceMapOptionsFacts
    {
        public class TheConstructor : ResourceMapOptionsFacts
        {
            [Fact]
            public void CreatesOptionsWithEmptyExtensionsCollection()
            {
                // Act
                var sut = new ResourceMapOptions<FakeModel>();

                // Assert
                Assert.Empty(sut.Extensions);
            }
        }

        public class TheCopyConstructor : ResourceMapOptionsFacts
        {
            [Fact]
            public void CreatesOptionsWithProvidedExtensionsCollection()
            {
                // Arrange
                var expectedExtension = new FakeResourceMapOptionsExtension();
                var extensions = new Dictionary<Type, IResourceMapOptionsExtension>
                {
                    [typeof(FakeResourceMapOptionsExtension)] = expectedExtension,
                };

                // Act
                var sut = new ResourceMapOptions<FakeModel>(extensions);

                // Assert
                var actualExtension = Assert.Single(sut.Extensions);
                Assert.Equal(expectedExtension, actualExtension);
            }

            [Fact]
            public void CreatesOptionsWithEmptyExtensionsCollectionWhenProvidedCollectionIsNull()
            {
                // Arrange
                var extensions = default(Dictionary<Type, IResourceMapOptionsExtension>);

                // Act
                var sut = new ResourceMapOptions<FakeModel>(extensions!);

                // Assert
                Assert.Empty(sut.Extensions);
            }
        }

        public class TheFindExtensionMethod : ResourceMapOptionsFacts
        {
            [Fact]
            public void ReturnsExtensionWhenFound()
            {
                // Arrange
                var expectedExtension = new FakeResourceMapOptionsExtension();
                var extensions = new Dictionary<Type, IResourceMapOptionsExtension>
                {
                    [typeof(FakeResourceMapOptionsExtension)] = expectedExtension,
                };

                var sut = new ResourceMapOptions<FakeModel>(extensions);

                // Act
                var actualExtension = sut.FindExtension<FakeResourceMapOptionsExtension>();

                // Assert
                Assert.Equal(expectedExtension, actualExtension);
            }

            [Fact]
            public void ReturnsNullWhenExtensionIsNotFound()
            {
                // Arrange
                var sut = new ResourceMapOptions<FakeModel>();

                // Act
                var extension = sut.FindExtension<FakeResourceMapOptionsExtension>();

                // Assert
                Assert.Null(extension);
            }
        }

        public class TheHasExtensionMethod : ResourceMapOptionsFacts
        {
            [Fact]
            public void ReturnsTrueWhenExtensionIsFound()
            {
                // Arrange
                var extensions = new Dictionary<Type, IResourceMapOptionsExtension>
                {
                    [typeof(FakeResourceMapOptionsExtension)] = new FakeResourceMapOptionsExtension(),
                };

                var sut = new ResourceMapOptions<FakeModel>(extensions);

                // Act
                var hasExtension = sut.HasExtension<FakeResourceMapOptionsExtension>();

                // Assert
                Assert.True(hasExtension);
            }

            [Fact]
            public void ReturnsFalseWhenExtensionIsNotFound()
            {
                // Arrange
                var sut = new ResourceMapOptions<FakeModel>();

                // Act
                var hasExtension = sut.HasExtension<FakeResourceMapOptionsExtension>();

                // Assert
                Assert.False(hasExtension);
            }
        }

        public class TheWithExtensionMethod : ResourceMapOptionsFacts
        {
            [Fact]
            public void ReturnsNewOptionsInstanceWithAddedExtension()
            {
                // Arrange
                var expectedExtension = new FakeResourceMapOptionsExtension();
                var sut = new ResourceMapOptions<FakeModel>();

                // Act
                var newOptions = sut.WithExtension(expectedExtension);

                // Assert
                Assert.Empty(sut.Extensions);

                var actualExtension = Assert.Single(newOptions.Extensions);
                Assert.Equal(expectedExtension, actualExtension);
            }

            [Fact]
            public void ReturnsNewOptionsInstanceWithOverriddenExtension()
            {
                // Arrange
                var expectedExistingExtension = new FakeResourceMapOptionsExtension();
                var extensions = new Dictionary<Type, IResourceMapOptionsExtension>
                {
                    [typeof(FakeResourceMapOptionsExtension)] = expectedExistingExtension,
                };

                var sut = new ResourceMapOptions<FakeModel>(extensions);
                var expectedOverriddenExtension = new FakeResourceMapOptionsExtension();

                // Act
                var newOptions = sut.WithExtension(expectedOverriddenExtension);

                // Assert
                var actualExistingExtension = Assert.Single(sut.Extensions);
                Assert.Equal(expectedExistingExtension, actualExistingExtension);

                var actualOverriddenExtension = Assert.Single(newOptions.Extensions);
                Assert.Equal(expectedOverriddenExtension, actualOverriddenExtension);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenExtensionIsNull()
            {
                // Arrange
                var extension = default(FakeResourceMapOptionsExtension);
                var sut = new ResourceMapOptions<FakeModel>();

                // Act
                var exception = Record.Exception(() => sut.WithExtension(extension!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenOptionsAreFrozen()
            {
                // Arrange
                var extension = new FakeResourceMapOptionsExtension();

                var sut = new ResourceMapOptions<FakeModel>();
                sut.Freeze();

                // Act
                var exception = Record.Exception(() => sut.WithExtension(extension));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheFreezeMethod : ResourceMapOptionsFacts
        {
            [Fact]
            public void SetsIsFrozenToTrue()
            {
                // Arrange
                var sut = new ResourceMapOptions<FakeModel>();

                // Act
                sut.Freeze();

                // Assert
                Assert.True(sut.IsFrozen);
            }
        }
    }
}
