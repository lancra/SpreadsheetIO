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

internal class PropertyMapBuilder<TResource, TProperty> : PropertyMapBuilder, IInternalPropertyMapBuilder<TResource, TProperty>
    where TResource : class
{
    public PropertyMapBuilder(Expression<Func<TResource, TProperty>> propertyExpression, IMapBuilderFactory mapBuilderFactory)
        : base(GetPropertyInfo(propertyExpression), mapBuilderFactory)
    {
    }

    public IPropertyMapBuilder<TResource, TProperty> HasKey(Action<IPropertyMapKeyBuilder> key)
    {
        Guard.Against.Null(key, nameof(key));

        key(KeyBuilder);
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> HasDefault(
        TProperty value,
        params ResourcePropertyDefaultReadingResolution[] resolutions)
    {
        Guard.Against.Null(resolutions, nameof(resolutions));
        Guard.Against.NullElements(resolutions, nameof(resolutions));

        AddOrUpdateRegistration(new DefaultValuePropertyMapOptionRegistration(value!, resolutions));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> IsOptional(PropertyElementKind? kind = default)
    {
        AddOrUpdateRegistration(new OptionalPropertyMapOption(kind ?? PropertyElementKind.All));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> UsesHeaderStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new HeaderStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> UsesBodyStyle(Style style, string name = "")
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(
            new IndexerKey(!string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString(), IndexerKeyKind.Custom),
            style));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> UsesBodyStyle(BuiltInExcelStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> UsesBodyStyle(BuiltInPackageStyle style)
    {
        Guard.Against.Null(style, nameof(style));

        AddOrUpdateRegistration(new BodyStyleMapOption(style.IndexerKey, style.Style));
        return this;
    }

    public IPropertyMapBuilder<TResource, TProperty> UsesDateKind(CellDateKind dateKind)
    {
        Guard.Against.Null(dateKind, nameof(dateKind));

        AddOrUpdateRegistration(new DateKindMapOption(dateKind));
        return this;
    }

    private static PropertyInfo GetPropertyInfo(Expression<Func<TResource, TProperty>> propertyExpression)
    {
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        if (propertyExpression.Body is MemberExpression memberExpression &&
            memberExpression.Member is PropertyInfo propertyInfo)
        {
            return propertyInfo;
        }

        throw new ArgumentException(Messages.InvalidResourcePropertyExpression, nameof(propertyExpression));
    }
}
