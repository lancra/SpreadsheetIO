using System.Linq.Expressions;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Defines the builder for generating a resource map.
/// </summary>
/// <typeparam name="TResource">The type of resource to map.</typeparam>
public interface IResourceMapBuilder<TResource>
    where TResource : class
{
    /// <summary>
    /// Specifies that a reading operation will continue if any resource cannot be read.
    /// </summary>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> ContinuesOnResourceReadingFailure();

    /// <summary>
    /// Specifies the default value resolutions to use for reading all properties.
    /// </summary>
    /// <param name="resolutions">The default value resolutions to register.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> HasDefaultPropertyReadingResolutions(params ResourcePropertyDefaultReadingResolution[] resolutions);

    /// <summary>
    /// Specifies the header row number. The default is one.
    /// </summary>
    /// <param name="number">The number of the row which contains the column headers.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> HasHeaderRowNumber(uint number);

    /// <summary>
    /// Specifies a property for the resource.
    /// </summary>
    /// <typeparam name="TProperty">The type of property.</typeparam>
    /// <param name="propertyExpression">The expression which provides the property in context to the resource.</param>
    /// <returns>The resulting resource map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> Property<TProperty>(Expression<Func<TResource, TProperty>> propertyExpression);

    /// <summary>
    /// Specifies a style to use for all property headers.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesHeaderStyle(Style style, string name = "");

    /// <summary>
    /// Specifies a style to use for all property headers.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesHeaderStyle(BuiltInExcelStyle style);

    /// <summary>
    /// Specifies a style to use for all property headers.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesHeaderStyle(BuiltInPackageStyle style);

    /// <summary>
    /// Specifies a style to use for all property bodies.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesBodyStyle(Style style, string name = "");

    /// <summary>
    /// Specifies a style to use for all property bodies.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesBodyStyle(BuiltInExcelStyle style);

    /// <summary>
    /// Specifies a style to use for all property bodies.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesBodyStyle(BuiltInPackageStyle style);

    /// <summary>
    /// Specifies the kind of date format to use for all properties.
    /// </summary>
    /// <param name="dateKind">The kind of date format.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesDateKind(CellDateKind dateKind);

    /// <summary>
    /// Specifies the kind of string format to use for all properties.
    /// </summary>
    /// <param name="stringKind">The kind of string format.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesStringKind(CellStringKind stringKind);

    /// <summary>
    /// Specifies that the resource will be created using a constructor with explicitly defined parameters.
    /// </summary>
    /// <param name="propertyNames">The name of the properties to use for the constructor parameters.</param>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesExplicitConstructor(params string[] propertyNames);

    /// <summary>
    /// Specifies that the resource will be created using a constructor with parameters that match the order and types of the defined
    /// properties.
    /// </summary>
    /// <returns>The resulting resource map builder.</returns>
    IResourceMapBuilder<TResource> UsesImplicitConstructor();
}
