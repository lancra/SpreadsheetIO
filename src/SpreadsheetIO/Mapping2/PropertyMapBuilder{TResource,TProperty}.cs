using System.Linq.Expressions;
using System.Reflection;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides the builder for generating a property map.
/// </summary>
/// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
/// <typeparam name="TProperty">The type of property to map.</typeparam>
public class PropertyMapBuilder<TResource, TProperty> : PropertyMapBuilder<TResource>
    where TResource : class
{
    internal PropertyMapBuilder(Expression<Func<TResource, TProperty>> propertyExpression)
        : base(GetPropertyInfo(propertyExpression))
    {
    }

    /// <summary>
    /// Specifies the unique key used when reading and writing the property.
    /// </summary>
    /// <param name="key">The action for modifying the key builder.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> HasKey(Action<PropertyMapKeyBuilder> key)
    {
        Guard.Against.Null(key, nameof(key));

        key(KeyBuilder);
        return this;
    }

    /// <summary>
    /// Specifies a default value to use when a property cannot be read.
    /// </summary>
    /// <param name="value">The default value.</param>
    /// <param name="resolutions">The default value resolutions to register.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> HasDefault(
        TProperty value,
        params ResourcePropertyDefaultReadingResolution[] resolutions)
    {
        Guard.Against.Null(resolutions, nameof(resolutions));
        Guard.Against.NullElements(resolutions, nameof(resolutions));

        AddOrUpdateRegistration(new DefaultValuePropertyMapOptionRegistration(value!, resolutions));
        return this;
    }

    /// <summary>
    /// Specifies that the property is optional for reading.
    /// </summary>
    /// <param name="kind">The kind of property element to designate as optional.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> IsOptional(PropertyElementKind? kind = default)
    {
        AddOrUpdateRegistration(new OptionalPropertyMapOption(kind ?? PropertyElementKind.All));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for the property header.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for the property header.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for the property header.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for the property body.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <param name="name">The unique name of the style to use. A random identifier is used when no name is provided.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> UsesBodyStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for the property body.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> UsesBodyStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies a style to use for the property body.
    /// </summary>
    /// <param name="style">The style to use.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> UsesBodyStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    /// <summary>
    /// Specifies the kind of date format to use.
    /// </summary>
    /// <param name="dateKind">The kind of date format.</param>
    /// <returns>The resulting property map builder.</returns>
    public PropertyMapBuilder<TResource, TProperty> UsesDateKind(CellDateKind dateKind)
    {
        Guard.Against.Null(dateKind, nameof(dateKind));

        AddOrUpdateRegistration(new DateKindMapOption(dateKind));
        return this;
    }

    private static PropertyInfo GetPropertyInfo(Expression<Func<TResource, TProperty>> propertyExpression)
    {
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        if (propertyExpression.Body is MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo;
            }
        }

        throw new ArgumentException(Messages.InvalidResourcePropertyExpression, nameof(propertyExpression));
    }
}
