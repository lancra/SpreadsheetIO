using System.Linq.Expressions;
using System.Reflection;
using LanceC.SpreadsheetIO.Mapping2.Validation;

namespace LanceC.SpreadsheetIO.Mapping2;

internal class MapBuilderFactory : IMapBuilderFactory
{
    private readonly IResourceMapBuilderValidator _validator;

    public MapBuilderFactory(IResourceMapBuilderValidator validator)
    {
        _validator = validator;
    }

    public IInternalResourceMapBuilder<TResource> CreateForResource<TResource>()
        where TResource : class
        => new ResourceMapBuilder<TResource>(this, _validator);

    public IInternalPropertyMapBuilder<TResource, TProperty> CreateForProperty<TResource, TProperty>(
        Expression<Func<TResource, TProperty>> propertyExpression)
        where TResource : class
        => new PropertyMapBuilder<TResource, TProperty>(propertyExpression, this);

    public IInternalPropertyMapKeyBuilder CreateForPropertyKey(PropertyInfo propertyInfo)
        => new PropertyMapKeyBuilder(propertyInfo);
}
