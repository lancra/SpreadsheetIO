using System;
using System.Linq;
using Ardalis.GuardClauses;
using Ardalis.SmartEnum;
using LanceC.SpreadsheetIO.Mapping.Internal.Extensions;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Provides a builder for generating property map options.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    public class PropertyMapOptionsBuilder<TResource, TProperty>
        where TResource : class
    {
        private readonly ResourceMapOptions<TResource> _resourceOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapOptionsBuilder{TResource, TProperty}"/> class.
        /// </summary>
        /// <param name="resourceOptions">The options for the resource map.</param>
        public PropertyMapOptionsBuilder(ResourceMapOptions<TResource> resourceOptions)
            : this(resourceOptions, new PropertyMapOptions<TResource, TProperty>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapOptionsBuilder{TResource, TProperty}"/> class.
        /// </summary>
        /// <param name="resourceOptions">The options for the resource map.</param>
        /// <param name="options">The property map options to copy.</param>
        public PropertyMapOptionsBuilder(
            ResourceMapOptions<TResource> resourceOptions,
            PropertyMapOptions<TResource, TProperty> options)
        {
            Guard.Against.Null(resourceOptions, nameof(resourceOptions));
            Guard.Against.Null(options, nameof(options));

            _resourceOptions = resourceOptions;
            Options = options;
        }

        /// <summary>
        /// Gets the current property map options.
        /// </summary>
        public PropertyMapOptions<TResource, TProperty> Options { get; private set; }

        /// <summary>
        /// Specifies a default value to use when a property cannot be parsed.
        /// </summary>
        /// <param name="value">The default property value.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> HasDefault(TProperty value)
        {
            var resolutionsExtension = _resourceOptions.FindExtension<DefaultPropertyReadingResolutionsResourceMapOptionsExtension>();
            var resolutions = resolutionsExtension?.Resolutions ?? SmartEnum<ResourcePropertyDefaultReadingResolution>.List;

            return HasDefault(value, resolutions.ToArray());
        }

        /// <summary>
        /// Specifies a default value to use for a specified property parse result.
        /// </summary>
        /// <param name="value">The default property value.</param>
        /// <param name="resolutions">The default value resolutions to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> HasDefault(
            TProperty value,
            params ResourcePropertyDefaultReadingResolution[] resolutions)
        {
            Guard.Against.Null(resolutions, nameof(resolutions));
            foreach (var resolution in resolutions)
            {
                Guard.Against.Null(resolution, nameof(resolution));
            }

            return WithOption(new DefaultValuePropertyMapOptionsExtension(value!, resolutions));
        }

        /// <summary>
        /// Marks the property as optional for reading.
        /// </summary>
        /// <param name="kind">The kind of property element.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> MarkAsOptional(PropertyElementKind kind = default!)
            => WithOption(new OptionalPropertyMapOptionsExtension(kind ?? PropertyElementKind.All));

        /// <summary>
        /// Marks the property map header as optional for reading.
        /// </summary>
        /// <returns>The resulting property map options builder.</returns>
        [Obsolete("This method is obsolete. Use MarkElementAsOptional(PropertyElementKind.Header) instead.")]
        public PropertyMapOptionsBuilder<TResource, TProperty> MarkHeaderAsOptional()
            => WithOption(new OptionalPropertyMapOptionsExtension(PropertyElementKind.Header));

        /// <summary>
        /// Marks the property map value as optional for reading.
        /// </summary>
        /// <returns>The resulting property map options builder.</returns>
        [Obsolete("This method is obsolete. Use MarkElementAsOptional(PropertyElementKind.Body) instead.")]
        public PropertyMapOptionsBuilder<TResource, TProperty> MarkValueAsOptional()
            => WithOption(new OptionalPropertyMapOptionsExtension(PropertyElementKind.Body));

        /// <summary>
        /// Specifies a style to use for the property header.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <param name="name">The unique name of the style to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> UseHeaderStyle(Style style, string name = "")
        {
            Guard.Against.Null(style, nameof(style));

            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }

            return WithOption(new HeaderStyleMapOptionsExtension(new IndexerKey(name, IndexerKeyKind.Custom), style));
        }

        /// <summary>
        /// Specifies a style to use for the property header.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> UseHeaderStyle(BuiltInExcelStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new HeaderStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies a style to use for the property header.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> UseHeaderStyle(BuiltInPackageStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new HeaderStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies a style to use for the property body.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <param name="name">The unique name of the style to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> UseBodyStyle(Style style, string name = "")
        {
            Guard.Against.Null(style, nameof(style));

            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }

            return WithOption(new BodyStyleMapOptionsExtension(new IndexerKey(name, IndexerKeyKind.Custom), style));
        }

        /// <summary>
        /// Specifies a style to use for the property body.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> UseBodyStyle(BuiltInExcelStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new BodyStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies a style to use for the property body.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> UseBodyStyle(BuiltInPackageStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new BodyStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies the kind of date to use for a property.
        /// </summary>
        /// <param name="dateKind">The kind of date to use.</param>
        /// <returns>The resulting property map options builder.</returns>
        public PropertyMapOptionsBuilder<TResource, TProperty> UseDateKind(CellDateKind dateKind)
        {
            Guard.Against.Null(dateKind, nameof(dateKind));

            return WithOption(new DateKindMapOptionsExtension(dateKind));
        }

        internal PropertyMapOptionsBuilder<TResource, TProperty> ApplyResourceMapOptions()
        {
            foreach (var resourceExtension in _resourceOptions.Extensions)
            {
                if (resourceExtension is IPropertyMapOptionsExtension propertyExtension)
                {
                    Options = (PropertyMapOptions<TResource, TProperty>)Options.WithExtensionInternal(propertyExtension);
                }
            }

            return this;
        }

        private PropertyMapOptionsBuilder<TResource, TProperty> WithOption<TExtension>(TExtension extension)
            where TExtension : class, IPropertyMapOptionsExtension
        {
            Guard.Against.Null(extension, nameof(extension));

            Options = (PropertyMapOptions<TResource, TProperty>)Options.WithExtension(extension);
            return this;
        }
    }
}
