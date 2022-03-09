using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using LanceC.SpreadsheetIO.Mapping2.Options;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides a map for a resource property.
/// </summary>
[ExcludeFromCodeCoverage]
public class PropertyMap
{
    internal PropertyMap(PropertyInfo property, PropertyMapKey key, MapOptions<IPropertyMapOption> options)
    {
        Property = property;
        Key = key;
        Options = options;
    }

    /// <summary>
    /// Gets the information about the mapped property.
    /// </summary>
    public PropertyInfo Property { get; }

    /// <summary>
    /// Gets the unique key for the map.
    /// </summary>
    public PropertyMapKey Key { get; }

    /// <summary>
    /// Gets the map options.
    /// </summary>
    public MapOptions<IPropertyMapOption> Options { get; }
}
