using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping;

public class ResourceMapOptionsBuilderFacts
{
    public class TheConstructorWithResourceMapOptionsParameter : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void ThrowsArgumentNullExceptionWhenResourceMapOptionsIsNull()
        {
            // Arrange
            var resourceOptions = default(ResourceMapOptions<FakeModel>);

            // Act
            var exception = Record.Exception(() => new ResourceMapOptionsBuilder<FakeModel>(resourceOptions!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheExitOnResourceReadingFailureMethod : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtension()
        {
            // Arrange
            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.ExitOnResourceReadingFailure();

            // Assert
            Assert.True(sut.Options.HasExtension<ExitOnResourceReadingFailureResourceMapOptionsExtension>());
        }
    }

    public class TheHasDefaultPropertyReadingResolutionsMethod : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedResolutions()
        {
            // Arrange
            var expectedResolution = ResourcePropertyDefaultReadingResolution.Invalid;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.HasDefaultPropertyReadingResolutions(expectedResolution);

            // Assert
            var extension = sut.Options.FindExtension<DefaultPropertyReadingResolutionsResourceMapOptionsExtension>();
            Assert.NotNull(extension);

            var actualResolution = Assert.Single(extension!.Resolutions);
            Assert.Equal(expectedResolution, actualResolution);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenResolutionCollectionIsNull()
        {
            // Arrange
            var resolutions = default(ResourcePropertyDefaultReadingResolution[]);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.HasDefaultPropertyReadingResolutions(resolutions!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenAnyResolutionIsNull()
        {
            // Arrange
            var resolutions = new[]
            {
                ResourcePropertyDefaultReadingResolution.Invalid,
                default,
            };

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.HasDefaultPropertyReadingResolutions(resolutions!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheOverrideHeaderRowNumberMethod : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedNumber()
        {
            // Arrange
            var number = 2U;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.OverrideHeaderRowNumber(number);

            // Assert
            var extension = sut.Options.FindExtension<HeaderRowNumberResourceMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(number, extension!.Number);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenNumberIsZero()
        {
            // Arrange
            var number = 0U;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.OverrideHeaderRowNumber(number));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }

    public class TheUseHeaderStyleMethodWithStyleAndNameParameters : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedStyle()
        {
            // Arrange
            var style = Style.Default;
            var name = "foo";

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseHeaderStyle(style, name);

            // Assert
            var extension = sut.Options.FindExtension<HeaderStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(name, extension!.Key.Name);
            Assert.Equal(IndexerKeyKind.Custom, extension!.Key.Kind);
            Assert.Equal(style, extension.Style);
        }

        [Fact]
        public void UsesGuidWhenNameNotProvided()
        {
            // Arrange
            var style = Style.Default;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseHeaderStyle(style);

            // Assert
            var extension = sut.Options.FindExtension<HeaderStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.True(Guid.TryParse(extension!.Key.Name, out var _));
            Assert.Equal(IndexerKeyKind.Custom, extension!.Key.Kind);
            Assert.Equal(style, extension.Style);
        }

        [Fact]
        public void UsesGuidWhenNameIsEmpty()
        {
            // Arrange
            var style = Style.Default;
            var name = string.Empty;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseHeaderStyle(style, name);

            // Assert
            var extension = sut.Options.FindExtension<HeaderStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.True(Guid.TryParse(extension!.Key.Name, out var _));
            Assert.Equal(IndexerKeyKind.Custom, extension!.Key.Kind);
            Assert.Equal(style, extension.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(Style);
            var name = "foo";

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseHeaderStyle(style!, name));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseHeaderStyleMethodWithBuiltInExcelStyleParameter : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedStyle()
        {
            // Arrange
            var style = BuiltInExcelStyle.Normal;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseHeaderStyle(style);

            // Assert
            var extension = sut.Options.FindExtension<HeaderStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(style.IndexerKey, extension!.Key);
            Assert.Equal(style.Style, extension.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInExcelStyle);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseHeaderStyleMethodWithBuiltInPackageStyleParameter : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedStyle()
        {
            // Arrange
            var style = BuiltInPackageStyle.Bold;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseHeaderStyle(style);

            // Assert
            var extension = sut.Options.FindExtension<HeaderStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(style.IndexerKey, extension!.Key);
            Assert.Equal(style.Style, extension.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInPackageStyle);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseBodyStyleMethodWithStyleAndNameParameters : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedStyle()
        {
            // Arrange
            var style = Style.Default;
            var name = "foo";

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseBodyStyle(style, name);

            // Assert
            var extension = sut.Options.FindExtension<BodyStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(name, extension!.Key.Name);
            Assert.Equal(IndexerKeyKind.Custom, extension.Key.Kind);
            Assert.Equal(style, extension.Style);
        }

        [Fact]
        public void UsesGuidWhenNameNotProvided()
        {
            // Arrange
            var style = Style.Default;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseBodyStyle(style);

            // Assert
            var extension = sut.Options.FindExtension<BodyStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.True(Guid.TryParse(extension!.Key.Name, out var _));
            Assert.Equal(IndexerKeyKind.Custom, extension!.Key.Kind);
            Assert.Equal(style, extension.Style);
        }

        [Fact]
        public void UsesGuidWhenNameIsEmpty()
        {
            // Arrange
            var style = Style.Default;
            var name = string.Empty;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseBodyStyle(style, name);

            // Assert
            var extension = sut.Options.FindExtension<BodyStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.True(Guid.TryParse(extension!.Key.Name, out var _));
            Assert.Equal(IndexerKeyKind.Custom, extension!.Key.Kind);
            Assert.Equal(style, extension.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(Style);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseBodyStyleMethodWithBuiltInExcelStyleParameter : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedStyle()
        {
            // Arrange
            var style = BuiltInExcelStyle.Normal;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseBodyStyle(style);

            // Assert
            var extension = sut.Options.FindExtension<BodyStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(style.IndexerKey, extension!.Key);
            Assert.Equal(style.Style, extension.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInExcelStyle);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseBodyStyleMethodWithBuiltInPackageStyleParameter : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedStyle()
        {
            // Arrange
            var style = BuiltInPackageStyle.Bold;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseBodyStyle(style);

            // Assert
            var extension = sut.Options.FindExtension<BodyStyleMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(style.IndexerKey, extension!.Key);
            Assert.Equal(style.Style, extension.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInPackageStyle);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseDateKindMethod : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedDateKind()
        {
            // Arrange
            var dateKind = CellDateKind.Number;

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseDateKind(dateKind);

            // Assert
            var extension = sut.Options.FindExtension<DateKindMapOptionsExtension>();
            Assert.NotNull(extension);
            Assert.Equal(dateKind, extension!.DateKind);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenDateKindIsNull()
        {
            // Arrange
            var dateKind = default(CellDateKind);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseDateKind(dateKind!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseExplicitConstructorMethod : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtensionWithSpecifiedPropertyNames()
        {
            // Arrange
            var expectedPropertyName = "foo";

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseExplicitConstructor(expectedPropertyName);

            // Assert
            var extension = sut.Options.FindExtension<ExplicitConstructorResourceMapOptionsExtension>();
            Assert.NotNull(extension);

            var actualPropertyName = Assert.Single(extension!.PropertyNames);
            Assert.Equal(expectedPropertyName, actualPropertyName);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenAnyPropertyNameIsEmpty()
        {
            // Arrange
            var propertyNames = new[]
            {
                "foo",
                string.Empty,
            };

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseExplicitConstructor(propertyNames));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenPropertyNameCollectionIsNull()
        {
            // Arrange
            var propertyNames = default(string[]);

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseExplicitConstructor(propertyNames!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenAnyPropertyNameIsNull()
        {
            // Arrange
            var propertyNames = new[]
            {
                "foo",
                default,
            };

            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            var exception = Record.Exception(() => sut.UseExplicitConstructor(propertyNames!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUseImplicitConstructorMethod : ResourceMapOptionsBuilderFacts
    {
        [Fact]
        public void AddsOptionsExtension()
        {
            // Arrange
            var resourceOptions = new ResourceMapOptions<FakeModel>();
            var sut = new ResourceMapOptionsBuilder<FakeModel>(resourceOptions);

            // Act
            sut.UseImplicitConstructor();

            // Assert
            Assert.True(sut.Options.HasExtension<ImplicitConstructorResourceMapOptionsExtension>());
        }
    }
}
