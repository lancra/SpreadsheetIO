using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Defines the builder for generating a property map.
/// </summary>
/// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
/// <typeparam name="TProperty">The type of property to map.</typeparam>
public interface IPropertyMapBuilder<TResource, TProperty>
    where TResource : class
{
    /// <summary>
    /// Specifies the unique key used when reading and writing the property.
    /// </summary>
    /// <param name="key">The action for modifying the key builder.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> HasKey(Action<IPropertyMapKeyBuilder> key);

    /// <summary>
    /// Specifies a default value to use when a property cannot be read.
    /// </summary>
    /// <param name="value">The default value.</param>
    /// <param name="resolutions">The default value resolutions to register.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> HasDefault(
        TProperty value,
        params ResourcePropertyDefaultReadingResolution[] resolutions);

    /// <summary>
    /// Specifies that the property is optional for reading.
    /// </summary>
    /// <param name="kind">The kind of property element to designate as optional.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> IsOptional(PropertyElementKind? kind = default);

    /// <summary>
    /// Specifies a style to use for the property header.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(Style style, string name = "");

    /// <summary>
    /// Specifies a style to use for the property header.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(BuiltInExcelStyle style);

    /// <summary>
    /// Specifies a style to use for the property header.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(BuiltInPackageStyle style);

    /// <summary>
    /// Specifies a style to use for the property body.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesBodyStyle(Style style, string name = "");

    /// <summary>
    /// Specifies a style to use for the property body.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesBodyStyle(BuiltInExcelStyle style);

    /// <summary>
    /// Specifies a style to use for the property body.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesBodyStyle(BuiltInPackageStyle style);

    /// <summary>
    /// Specifies the kind of date format to use.
    /// </summary>
    /// <param name="dateKind">The kind of date format.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesDateKind(CellDateKind dateKind);

    /// <summary>
    /// Specifies the kind of string format to use.
    /// </summary>
    /// <param name="stringKind">The kind of string format.</param>
    /// <returns>The resulting property map builder.</returns>
    IPropertyMapBuilder<TResource, TProperty> UsesStringKind(CellStringKind stringKind);
}
