using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Represents a map to Excel for a resource property.
/// </summary>
/// <typeparam name="TResource">The type of resource to map.</typeparam>
[ExcludeFromCodeCoverage]
public class PropertyMap<TResource>
    where TResource : class
{
    internal PropertyMap(PropertyInfo property, PropertyMapKey key, PropertyMapOptions<TResource> options)
    {
        Property = property;
        Key = key;
        Options = options;
    }

    /// <summary>
    /// Gets the underlying resource property.
    /// </summary>
    public PropertyInfo Property { get; }

    /// <summary>
    /// Gets the property identification key.
    /// </summary>
    public PropertyMapKey Key { get; }

    /// <summary>
    /// Gets the property map customization options.
    /// </summary>
    public PropertyMapOptions<TResource> Options { get; }
}
