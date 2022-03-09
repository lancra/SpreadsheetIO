using System.Linq.Expressions;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides the builder for generating a resource map.
/// </summary>
/// <typeparam name="TResource">The type of resource to map.</typeparam>
public class ResourceMapBuilder<TResource> : ResourceMapBuilder
    where TResource : class
{
    internal ResourceMapBuilder()
        : base(typeof(TResource))
    {
    }

    /// <summary>
    /// Specifies that a reading operation will exit if any resource cannot be read.
    /// </summary>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> ExitsOnResourceReadingFailure()
    {
        AddOrUpdateRegistration(new ExitOnResourceReadingFailureResourceMapOption());
        return this;
    }

    /// <summary>
    /// Specifies the default value resolutions to use for reading all properties.
    /// </summary>
    /// <param name="resolutions">The default value resolutions to register.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> HasDefaultPropertyReadingResolutions(
        params ResourcePropertyDefaultReadingResolution[] resolutions)
    {
        Guard.Against.NullOrEmpty(resolutions, nameof(resolutions));
        Guard.Against.NullElements(resolutions, nameof(resolutions));

        AddOrUpdateRegistration(new DefaultPropertyReadingResolutionsResourceMapOptionRegistration(resolutions));
        return this;
    }

    /// <summary>
    /// Specifies the header row number. The default is one.
    /// </summary>
    /// <param name="number">The number of the row which contains the column headers.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> HasHeaderRowNumber(uint number)
    {
        Guard.Against.Zero(number, nameof(number));

        AddOrUpdateRegistration(new HeaderRowNumberResourceMapOption(number));
        return this;
    }

    /// <summary>
    /// Specifies a property for the resource.
    /// </summary>
    /// <typeparam name="TProperty">The type of property.</typeparam>
    /// <param name="propertyExpression">The expression which provides the property in context to the resource.</param>
    /// <returns>The resulting resource map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> Property<TProperty>(Expression<Func<TResource, TProperty>> propertyExpression)
    {
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        var propertyMapBuilder = new PropertyMapBuilder<TResource, TProperty>(propertyExpression);
        AddProperty(propertyMapBuilder);

        return propertyMapBuilder;
    }

    /// <summary>
    /// Specifies a style to use for all property headers.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesHeaderStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for all property headers.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesHeaderStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for all property headers.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesHeaderStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for all property bodies.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesBodyStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for all property bodies.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesBodyStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for all property bodies.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesBodyStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies the kind of date format to use for all properties.
    /// </summary>
    /// <param name="dateKind">The kind of date format.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesDateKind(CellDateKind dateKind)
    {
        Guard.Against.Null(dateKind, nameof(dateKind));

        AddOrUpdateRegistration(new DateKindMapOption(dateKind));
        return this;
    }

    /// <summary>
    /// Specifies that the resource will be created using a constructor with explicitly defined parameters.
    /// </summary>
    /// <param name="propertyNames">The name of the properties to use for the constructor parameters.</param>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesExplicitConstructor(params string[] propertyNames)
    {
        Guard.Against.NullOrEmpty(propertyNames, nameof(propertyNames));
        Guard.Against.NullOrEmptyElements(propertyNames, nameof(propertyNames));

        AddOrUpdateRegistration(new ExplicitConstructorResourceMapOptionRegistration(propertyNames));
        return this;
    }

    /// <summary>
    /// Specifies that the resource will be created using a constructor with parameters that match the order and types of the defined
    /// properties.
    /// </summary>
    /// <returns>The resulting resource map builder.</returns>
    public ResourceMapBuilder<TResource> UsesImplicitConstructor()
    {
        AddOrUpdateRegistration(new ImplicitConstructorResourceMapOptionRegistration());
        return this;
    }
}
