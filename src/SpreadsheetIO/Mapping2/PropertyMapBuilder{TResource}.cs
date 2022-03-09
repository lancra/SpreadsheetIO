using System.Reflection;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides the builder for generating a property map.
/// </summary>
/// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
public abstract class PropertyMapBuilder<TResource> : PropertyMapBuilder
    where TResource : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyMapBuilder{TResource}"/> class.
    /// </summary>
    /// <param name="propertyInfo">The information about the resource property.</param>
    protected PropertyMapBuilder(PropertyInfo propertyInfo)
        : base(propertyInfo)
    {
    }
}
