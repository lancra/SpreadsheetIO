using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping;

public class PropertyMapOptionsFacts
{
    public class TheConstructor : PropertyMapOptionsFacts
    {
        [Fact]
        public void CreatesOptionsWithEmptyExtensionsCollection()
        {
            // Act
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Assert
            Assert.Empty(sut.Extensions);
        }
    }

    public class TheCopyConstructor : PropertyMapOptionsFacts
    {
        [Fact]
        public void CreatesOptionsWithProvidedExtensionsCollection()
        {
            // Arrange
            var expectedExtension = new FakePropertyMapOptionsExtension();
            var extensions = new Dictionary<Type, IPropertyMapOptionsExtension>
            {
                [typeof(FakePropertyMapOptionsExtension)] = expectedExtension,
            };

            // Act
            var sut = new PropertyMapOptions<FakeModel, string>(extensions);

            // Assert
            var actualExtension = Assert.Single(sut.Extensions);
            Assert.Equal(expectedExtension, actualExtension);
        }

        [Fact]
        public void CreatesOptionsWithEmptyExtensionsCollectionWhenProvidedCollectionIsNull()
        {
            // Arrange
            var extensions = default(Dictionary<Type, IPropertyMapOptionsExtension>);

            // Act
            var sut = new PropertyMapOptions<FakeModel, string>(extensions!);

            // Assert
            Assert.Empty(sut.Extensions);
        }
    }

    public class TheFindExtensionMethod : PropertyMapOptionsFacts
    {
        [Fact]
        public void ReturnsExtensionWhenFound()
        {
            // Arrange
            var expectedExtension = new FakePropertyMapOptionsExtension();
            var extensions = new Dictionary<Type, IPropertyMapOptionsExtension>
            {
                [typeof(FakePropertyMapOptionsExtension)] = expectedExtension,
            };

            var sut = new PropertyMapOptions<FakeModel, string>(extensions);

            // Act
            var actualExtension = sut.FindExtension<FakePropertyMapOptionsExtension>();

            // Assert
            Assert.Equal(expectedExtension, actualExtension);
        }

        [Fact]
        public void ReturnsNullWhenExtensionIsNotFound()
        {
            // Arrange
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Act
            var extension = sut.FindExtension<FakePropertyMapOptionsExtension>();

            // Assert
            Assert.Null(extension);
        }
    }

    public class TheHasExtensionMethod : PropertyMapOptionsFacts
    {
        [Fact]
        public void ReturnsTrueWhenExtensionIsFound()
        {
            // Arrange
            var extensions = new Dictionary<Type, IPropertyMapOptionsExtension>
            {
                [typeof(FakePropertyMapOptionsExtension)] = new FakePropertyMapOptionsExtension(),
            };

            var sut = new PropertyMapOptions<FakeModel, string>(extensions);

            // Act
            var hasExtension = sut.HasExtension<FakePropertyMapOptionsExtension>();

            // Assert
            Assert.True(hasExtension);
        }

        [Fact]
        public void ReturnsFalseWhenExtensionIsNotFound()
        {
            // Arrange
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Act
            var hasExtension = sut.HasExtension<FakePropertyMapOptionsExtension>();

            // Assert
            Assert.False(hasExtension);
        }
    }

    public class TheWithExtensionMethod : PropertyMapOptionsFacts
    {
        [Fact]
        public void ReturnsNewOptionsInstanceWithAddedExtension()
        {
            // Arrange
            var expectedExtension = new FakePropertyMapOptionsExtension();
            var sut = new PropertyMapOptions<FakeModel, string>();

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
            var expectedExistingExtension = new FakePropertyMapOptionsExtension();
            var extensions = new Dictionary<Type, IPropertyMapOptionsExtension>
            {
                [typeof(FakePropertyMapOptionsExtension)] = expectedExistingExtension,
            };

            var sut = new PropertyMapOptions<FakeModel, string>(extensions);
            var expectedOverriddenExtension = new FakePropertyMapOptionsExtension();

            // Act
            var newOptions = sut.WithExtension(expectedOverriddenExtension);

            // Assert
            var actualExistingExtension = Assert.Single(sut.Extensions);
            Assert.Equal(expectedExistingExtension, actualExistingExtension);

            var actualOverriddenExtension = Assert.Single(newOptions.Extensions);
            Assert.Equal(expectedOverriddenExtension, actualOverriddenExtension);
        }

        [Fact]
        public void DoesNotThrowExceptionForNullableAllowedType()
        {
            // Arrange
            var extension = new FakeDateTimePropertyMapOptionsExtension();
            var sut = new PropertyMapOptions<FakeModel, DateTime?>();

            // Act
            var exception = Record.Exception(() => sut.WithExtension(extension));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenExtensionIsNull()
        {
            // Arrange
            var extension = default(FakePropertyMapOptionsExtension);
            var sut = new PropertyMapOptions<FakeModel, string>();

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
            var extension = new FakePropertyMapOptionsExtension();

            var sut = new PropertyMapOptions<FakeModel, string>();
            sut.Freeze();

            // Act
            var exception = Record.Exception(() => sut.WithExtension(extension));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenExtensionIsNotAllowedForPropertyType()
        {
            // Arrange
            var extension = new FakeDateTimePropertyMapOptionsExtension();
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Act
            var exception = Record.Exception(() => sut.WithExtension(extension));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    public class TheWithExtensionNoOverwriteMethod : PropertyMapOptionsFacts
    {
        [Fact]
        public void ReturnsNewOptionsInstanceWithAddedExtension()
        {
            // Arrange
            var expectedExtension = new FakePropertyMapOptionsExtension();
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Act
            var newOptions = sut.WithExtensionNoOverwrite(expectedExtension);

            // Assert
            Assert.Empty(sut.Extensions);

            var actualExtension = Assert.Single(newOptions.Extensions);
            Assert.Equal(expectedExtension, actualExtension);
        }

        [Fact]
        public void ReturnsSameOptionsInstanceWhenNewExtensionIsAlreadyPresent()
        {
            // Arrange
            var expectedExistingExtension = new FakePropertyMapOptionsExtension();
            var extensions = new Dictionary<Type, IPropertyMapOptionsExtension>
            {
                [typeof(FakePropertyMapOptionsExtension)] = expectedExistingExtension,
            };

            var sut = new PropertyMapOptions<FakeModel, string>(extensions);
            var overriddenExtension = new FakePropertyMapOptionsExtension();

            // Act
            var newOptions = sut.WithExtensionNoOverwrite(overriddenExtension);

            // Assert
            Assert.Equal(sut, newOptions);

            var actualExistingExtension = Assert.Single(newOptions.Extensions);
            Assert.Equal(expectedExistingExtension, actualExistingExtension);
        }

        [Fact]
        public void ReturnsSameOptionsInstanceWhenNewExtensionIsNotAllowedForPropertyType()
        {
            // Arrange
            var extension = new FakeDateTimePropertyMapOptionsExtension();
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Act
            var newOptions = sut.WithExtensionNoOverwrite(extension);

            // Assert
            Assert.Equal(sut, newOptions);
            Assert.Empty(newOptions.Extensions);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenExtensionIsNull()
        {
            // Arrange
            var extension = default(FakePropertyMapOptionsExtension);
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Act
            var exception = Record.Exception(() => sut.WithExtensionNoOverwrite(extension!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenOptionsAreFrozen()
        {
            // Arrange
            var extension = new FakePropertyMapOptionsExtension();

            var sut = new PropertyMapOptions<FakeModel, string>();
            sut.Freeze();

            // Act
            var exception = Record.Exception(() => sut.WithExtensionNoOverwrite(extension));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    public class TheFreezeMethod : PropertyMapOptionsFacts
    {
        [Fact]
        public void SetsIsFrozenToTrue()
        {
            // Arrange
            var sut = new PropertyMapOptions<FakeModel, string>();

            // Act
            sut.Freeze();

            // Assert
            Assert.True(sut.IsFrozen);
        }
    }
}
