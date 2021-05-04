using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping
{
    public class PropertyMapOptionsBuilderFacts
    {
        public class TheConstructorWithResourceMapOptionsParameter : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenResourceMapOptionsIsNull()
            {
                // Arrange
                var resourceOptions = default(ResourceMapOptions<FakeModel>);

                // Act
                var exception = Record.Exception(() => new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheConstructorWithResourceMapOptionsAndPropertyMapOptionsParameters : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenResourceMapOptionsIsNull()
            {
                // Arrange
                var resourceOptions = default(ResourceMapOptions<FakeModel>);
                var propertyOptions = new PropertyMapOptions<FakeModel, string>();

                // Act
                var exception = Record.Exception(()
                    => new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions!, propertyOptions));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPropertyMapOptionsIsNull()
            {
                // Arrange
                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var propertyOptions = default(PropertyMapOptions<FakeModel, string>);

                // Act
                var exception = Record.Exception(()
                    => new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions, propertyOptions!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheHasDefaultMethodWithValueParameter : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithAllResolutions()
            {
                // Arrange
                var value = "foo";

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                sut.HasDefault(value);

                // Assert
                var extension = sut.Options.FindExtension<DefaultValuePropertyMapOptionsExtension>();
                Assert.NotNull(extension);
                Assert.Equal(value, extension!.Value);
                Assert.Equal(Enumeration.GetAll<ResourcePropertyDefaultReadingResolution>().Count, extension.Resolutions.Count);
            }

            [Fact]
            public void AddsOptionsExtensionWithResolutionsFromResourceMapOptionsExtension()
            {
                // Arrange
                var value = "foo";
                var expectedResolution = ResourcePropertyDefaultReadingResolution.Invalid;

                var resourceExtension = new DefaultPropertyReadingResolutionsResourceMapOptionsExtension(expectedResolution);
                var resourceExtensions = new Dictionary<Type, IResourceMapOptionsExtension>
                {
                    [typeof(DefaultPropertyReadingResolutionsResourceMapOptionsExtension)] = resourceExtension,
                };
                var resourceOptions = new ResourceMapOptions<FakeModel>(resourceExtensions);

                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                sut.HasDefault(value);

                // Assert
                var extension = sut.Options.FindExtension<DefaultValuePropertyMapOptionsExtension>();
                Assert.NotNull(extension);
                Assert.Equal(value, extension!.Value);

                var actualResolution = Assert.Single(extension.Resolutions);
                Assert.Equal(expectedResolution, actualResolution);
            }
        }

        public class TheHasDefaultMethodWithValueAndResolutionsParameters : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedResolutions()
            {
                // Arrange
                var value = "foo";
                var expectedResolution = ResourcePropertyDefaultReadingResolution.Invalid;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                sut.HasDefault(value, expectedResolution);

                // Assert
                var extension = sut.Options.FindExtension<DefaultValuePropertyMapOptionsExtension>();
                Assert.NotNull(extension);
                Assert.Equal(value, extension!.Value);

                var actualResolution = Assert.Single(extension.Resolutions);
                Assert.Equal(expectedResolution, actualResolution);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenResolutionCollectionIsNull()
            {
                // Arrange
                var value = "foo";
                var resolutions = default(ResourcePropertyDefaultReadingResolution[]);

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.HasDefault(value, resolutions!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenAnyResolutionIsNull()
            {
                // Arrange
                var value = "foo";
                var resolutions = new[]
                {
                    ResourcePropertyDefaultReadingResolution.Invalid,
                    default,
                };

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.HasDefault(value, resolutions!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheMarkHeaderAsOptionalMethod : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtension()
            {
                // Arrange
                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                sut.MarkHeaderAsOptional();

                // Assert
                Assert.True(sut.Options.HasExtension<OptionalHeaderPropertyMapOptionsExtension>());
            }
        }

        public class TheMarkValueAsOptionalMethod : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtension()
            {
                // Arrange
                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                sut.MarkValueAsOptional();

                // Assert
                Assert.True(sut.Options.HasExtension<OptionalValuePropertyMapOptionsExtension>());
            }
        }

        public class TheUseHeaderStyleMethodWithStyleAndNameParameters : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedStyle()
            {
                // Arrange
                var style = new Style(Border.Default, Fill.Default, Font.Default, NumericFormat.Default);
                var name = "foo";

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                sut.UseHeaderStyle(style, name);

                // Assert
                var extension = sut.Options.FindExtension<HeaderStyleMapOptionsExtension>();
                Assert.NotNull(extension);
                Assert.Equal(name, extension!.Key.Name);
                Assert.Equal(IndexerKeyKind.Custom, extension.Key.Kind);
                Assert.Equal(style, extension.Style);
            }

            [Fact]
            public void UsesGuidWhenNameNotProvided()
            {
                // Arrange
                var style = new Style(Border.Default, Fill.Default, Font.Default, NumericFormat.Default);

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var style = new Style(Border.Default, Fill.Default, Font.Default, NumericFormat.Default);
                var name = string.Empty;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseHeaderStyle(style!, name));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheUseHeaderStyleMethodWithBuiltInExcelStyleParameter : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedStyle()
            {
                // Arrange
                var style = BuiltInExcelStyle.Normal;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseHeaderStyle(style!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheUseHeaderStyleMethodWithBuiltInPackageStyleParameter : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedStyle()
            {
                // Arrange
                var style = BuiltInPackageStyle.Bold;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseHeaderStyle(style!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheUseBodyStyleMethodWithStyleAndNameParameters : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedStyle()
            {
                // Arrange
                var style = new Style(Border.Default, Fill.Default, Font.Default, NumericFormat.Default);
                var name = "foo";

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var style = new Style(Border.Default, Fill.Default, Font.Default, NumericFormat.Default);

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var style = new Style(Border.Default, Fill.Default, Font.Default, NumericFormat.Default);
                var name = string.Empty;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var name = "foo";

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseBodyStyle(style!, name));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheUseBodyStyleMethodWithBuiltInExcelStyleParameter : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedStyle()
            {
                // Arrange
                var style = BuiltInExcelStyle.Normal;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseBodyStyle(style!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheUseBodyStyleMethodWithBuiltInPackageStyleParameter : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedStyle()
            {
                // Arrange
                var style = BuiltInPackageStyle.Bold;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

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
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseBodyStyle(style!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheUseDateKindMethod : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsOptionsExtensionWithSpecifiedDateKind()
            {
                // Arrange
                var dateKind = CellDateKind.Number;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, DateTime>(resourceOptions);

                // Act
                sut.UseDateKind(dateKind);

                // Assert
                var extension = sut.Options.FindExtension<DateKindMapOptionsExtension>();
                Assert.NotNull(extension);
                Assert.Equal(dateKind, extension!.DateKind);
            }

            [Fact]
            public void AllowsNullableDateTime()
            {
                // Arrange
                var dateKind = CellDateKind.Text;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, DateTime?>(resourceOptions);

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
                var sut = new PropertyMapOptionsBuilder<FakeModel, DateTime>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseDateKind(dateKind!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenPropertyTypeIsNotDateTimeType()
            {
                // Arrange
                var dateKind = CellDateKind.Number;

                var resourceOptions = new ResourceMapOptions<FakeModel>();
                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                var exception = Record.Exception(() => sut.UseDateKind(dateKind!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheApplyResourceMapOptionsMethod : PropertyMapOptionsBuilderFacts
        {
            [Fact]
            public void AddsSupportedExtensionsFromResourceMapOptionsToPropertyMapOptions()
            {
                // Arrange
                var expectedExtension = new FakeMapOptionsExtension();
                var resourceExtensions = new Dictionary<Type, IResourceMapOptionsExtension>
                {
                    [typeof(FakeResourceMapOptionsExtension)] = new FakeResourceMapOptionsExtension(),
                    [typeof(FakeMapOptionsExtension)] = expectedExtension,
                };
                var resourceOptions = new ResourceMapOptions<FakeModel>(resourceExtensions);

                var sut = new PropertyMapOptionsBuilder<FakeModel, string>(resourceOptions);

                // Act
                sut.ApplyResourceMapOptions();

                // Assert
                var actualExtension = Assert.Single(sut.Options.Extensions);
                Assert.Equal(expectedExtension, actualExtension);
            }
        }
    }
}
