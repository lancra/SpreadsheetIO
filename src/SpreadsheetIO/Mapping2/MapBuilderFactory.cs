using System.Linq.Expressions;
using System.Reflection;

namespace LanceC.SpreadsheetIO.Mapping2;

internal class MapBuilderFactory : IMapBuilderFactory
{
    public IInternalResourceMapBuilder<TResource> CreateForResource<TResource>()
        where TResource : class
        => new ResourceMapBuilder<TResource>(this);

    public IInternalPropertyMapBuilder<TResource, TProperty> CreateForProperty<TResource, TProperty>(
        Expression<Func<TResource, TProperty>> propertyExpression)
        where TResource : class
        => new PropertyMapBuilder<TResource, TProperty>(propertyExpression, this);

    public IInternalPropertyMapKeyBuilder CreateForPropertyKey(PropertyInfo propertyInfo)
        => new PropertyMapKeyBuilder(propertyInfo);
}
