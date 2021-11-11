using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Provides a builder for generating resource map options.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the map is defined for.</typeparam>
    public class ResourceMapOptionsBuilder<TResource>
        where TResource : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMapOptionsBuilder{TResource}"/> class.
        /// </summary>
        public ResourceMapOptionsBuilder()
            : this(new ResourceMapOptions<TResource>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMapOptionsBuilder{TResource}"/> class.
        /// </summary>
        /// <param name="options">The resource map options to copy.</param>
        public ResourceMapOptionsBuilder(ResourceMapOptions<TResource> options)
        {
            Guard.Against.Null(options, nameof(options));

            Options = options;
        }

        /// <summary>
        /// Gets the current resource map options.
        /// </summary>
        public ResourceMapOptions<TResource> Options { get; private set; }

        /// <summary>
        /// Specifies that a reading operation will exit if any resource cannot be read.
        /// </summary>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> ExitOnResourceReadingFailure()
            => WithOption(new ExitOnResourceReadingFailureResourceMapOptionsExtension());

        /// <summary>
        /// Specifies the default value resolutions to use for resource properties.
        /// </summary>
        /// <param name="resolutions">The default value resolutions to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> HasDefaultPropertyReadingResolutions(
            params ResourcePropertyDefaultReadingResolution[] resolutions)
        {
            Guard.Against.Null(resolutions, nameof(resolutions));
            foreach (var resolution in resolutions)
            {
                Guard.Against.Null(resolution, nameof(resolution));
            }

            return WithOption(new DefaultPropertyReadingResolutionsResourceMapOptionsExtension(resolutions));
        }

        /// <summary>
        /// Overrides the default header row number.
        /// </summary>
        /// <param name="number">The number of the row which contains the headers.</param>
        /// <returns>The resulting resource map options builder.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="number"/> is zero.</exception>
        public ResourceMapOptionsBuilder<TResource> OverrideHeaderRowNumber(uint number)
        {
            Guard.Against.Zero(number, nameof(number));

            return WithOption(new HeaderRowNumberResourceMapOptionsExtension(number));
        }

        /// <summary>
        /// Specifies a style to use for the header of all properties.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <param name="name">The unique name of the style to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseHeaderStyle(Style style, string name = "")
        {
            Guard.Against.Null(style, nameof(style));

            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }

            return WithOption(new HeaderStyleMapOptionsExtension(new IndexerKey(name, IndexerKeyKind.Custom), style));
        }

        /// <summary>
        /// Specifies a style to use for the header of all properties.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseHeaderStyle(BuiltInExcelStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new HeaderStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies a style to use for the header of all properties.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseHeaderStyle(BuiltInPackageStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new HeaderStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies a style to use for the body of all properties.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <param name="name">The unique name of the style to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseBodyStyle(Style style, string name = "")
        {
            Guard.Against.Null(style, nameof(style));

            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }

            return WithOption(new BodyStyleMapOptionsExtension(new IndexerKey(name, IndexerKeyKind.Custom), style));
        }

        /// <summary>
        /// Specifies a style to use for the body of all properties.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseBodyStyle(BuiltInExcelStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new BodyStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies a style to use for the body of all properties.
        /// </summary>
        /// <param name="style">The style to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseBodyStyle(BuiltInPackageStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            return WithOption(new BodyStyleMapOptionsExtension(style.IndexerKey, style.Style));
        }

        /// <summary>
        /// Specifies the kind of date to use for all properties.
        /// </summary>
        /// <param name="dateKind">The kind of date to use.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseDateKind(CellDateKind dateKind)
        {
            Guard.Against.Null(dateKind, nameof(dateKind));

            return WithOption(new DateKindMapOptionsExtension(dateKind));
        }

        /// <summary>
        /// Specifies that the resource will be created using a constructor with explicitly defined parameters.
        /// </summary>
        /// <param name="propertyNames">The names of the properties to use for the constructor parameters.</param>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseExplicitConstructor(params string[] propertyNames)
        {
            Guard.Against.Null(propertyNames, nameof(propertyNames));
            foreach (var propertyName in propertyNames)
            {
                Guard.Against.NullOrEmpty(propertyName, nameof(propertyName));
            }

            return WithOption(new ExplicitConstructorResourceMapOptionsExtension(propertyNames));
        }

        /// <summary>
        /// Specifies that the resource will be created using a constructor with parameters that match the order of the defined
        /// resource properties.
        /// </summary>
        /// <returns>The resulting resource map options builder.</returns>
        public ResourceMapOptionsBuilder<TResource> UseImplicitConstructor()
            => WithOption(new ImplicitConstructorResourceMapOptionsExtension());

        private ResourceMapOptionsBuilder<TResource> WithOption<TExtension>(TExtension extension)
            where TExtension : class, IResourceMapOptionsExtension
        {
            Guard.Against.Null(extension, nameof(extension));

            Options = (ResourceMapOptions<TResource>)Options.WithExtension(extension);
            return this;
        }
    }
}
