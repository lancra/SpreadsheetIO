using System.Linq.Expressions;
using System.Reflection;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;

namespace LanceC.SpreadsheetIO.Facts.Testing.Creators;

internal static class PropertyMapCreator2
{
    public static PropertyMap Create<TResource, TProperty>(
        Expression<Func<TResource, TProperty>> expression,
        Action<PropertyMapKeyBuilder>? keyAction = default,
        params IPropertyMapOption[] options)
        where TResource : class
    {
        var propertyInfo = GetPropertyInfo(expression);

        var keyBuilder = new PropertyMapKeyBuilder(propertyInfo);
        if (keyAction is not null)
        {
            keyAction(keyBuilder);
        }

        var mapOptions = new MapOptions<IPropertyMapOption>(
            (options ?? Array.Empty<IPropertyMapOption>()).ToDictionary(o => o.GetType()));
        var map = new PropertyMap(propertyInfo, keyBuilder.Key, mapOptions);
        return map;
    }

    public static PropertyMap CreateForFakeModel<TProperty>(
        Expression<Func<FakeModel, TProperty>> expression,
        Action<PropertyMapKeyBuilder>? keyAction = default,
        params IPropertyMapOption[] options)
        => Create(expression, keyAction, options);

    public static PropertyMap CreateForFakeConstructionModel<TProperty>(
        Expression<Func<FakeConstructionModel, TProperty>> expression,
        params IPropertyMapOption[] options)
        => Create(expression, default, options);

    public static PropertyMap CreateForFakeResourcePropertyStrategyModel<TProperty>(
        Expression<Func<FakeResourcePropertyStrategyModel, TProperty>> expression,
        params IPropertyMapOption[] options)
        => Create(expression, default, options);

    private static PropertyInfo GetPropertyInfo<TResource, TProperty>(Expression<Func<TResource, TProperty>> property)
        where TResource : class
    {
        if (property.Body is MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo;
            }
        }

        throw new ArgumentException();
    }
}
