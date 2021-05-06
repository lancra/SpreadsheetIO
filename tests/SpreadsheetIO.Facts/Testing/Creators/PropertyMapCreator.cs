using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Creators
{
    public static class PropertyMapCreator
    {
        public static PropertyMap<FakeModel> CreateForFakeModel<TProperty>(
            Expression<Func<FakeModel, TProperty>> expression,
            params IPropertyMapOptionsExtension[] extensions)
        {
            var propertyInfo = GetPropertyInfo(expression);
            var extensionsLookup = extensions.ToDictionary(extension => extension.GetType(), extension => extension);

            var map = new PropertyMap<FakeModel>(
                propertyInfo,
                new PropertyMapKey(propertyInfo.Name, default, false),
                new PropertyMapOptions<FakeModel, TProperty>(extensionsLookup));
            return map;
        }

        public static PropertyMap<FakeResourcePropertyStrategyModel> CreateForFakeResourcePropertyStrategyModel<TProperty>(
            Expression<Func<FakeResourcePropertyStrategyModel, TProperty>> expression,
            params IPropertyMapOptionsExtension[] extensions)
        {
            var propertyInfo = GetPropertyInfo(expression);
            var extensionsLookup = extensions.ToDictionary(extension => extension.GetType(), extension => extension);

            var map = new PropertyMap<FakeResourcePropertyStrategyModel>(
                propertyInfo,
                new PropertyMapKey(propertyInfo.Name, default, false),
                new PropertyMapOptions<FakeResourcePropertyStrategyModel, TProperty>(extensionsLookup));
            return map;
        }

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
}
