using System.Linq.Expressions;
using System.Reflection;

namespace LanceC.SpreadsheetIO.Mapping.Builders;

/// <summary>
/// Defines the factory for generating map builders.
/// </summary>
internal interface IMapBuilderFactory
{
    /// <summary>
    /// Creates a map builder for a resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to map.</typeparam>
    /// <returns>The internal resource map builder.</returns>
    IInternalResourceMapBuilder<TResource> CreateForResource<TResource>()
        where TResource : class;

    /// <summary>
    /// Creates a map builder for a resource property.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
    /// <typeparam name="TProperty">The type of property to map.</typeparam>
    /// <param name="propertyExpression">The expression which defines the property within the resource.</param>
    /// <returns>The internal property map builder.</returns>
    IInternalPropertyMapBuilder<TResource, TProperty> CreateForProperty<TResource, TProperty>(
        Expression<Func<TResource, TProperty>> propertyExpression)
        where TResource : class;

    /// <summary>
    /// Creates a builder for a resource property key.
    /// </summary>
    /// <param name="propertyInfo">The information about the resource property.</param>
    /// <returns>The internal property map key builder.</returns>
    IInternalPropertyMapKeyBuilder CreateForPropertyKey(PropertyInfo propertyInfo);
}
