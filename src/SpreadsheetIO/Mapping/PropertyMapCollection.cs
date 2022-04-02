using System.Collections.ObjectModel;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Provides a collection of property maps.
/// </summary>
public class PropertyMapCollection : ReadOnlyCollection<PropertyMap>
{
    internal PropertyMapCollection(IList<PropertyMap> propertyMaps)
        : base(propertyMaps)
    {
    }

    /// <summary>
    /// Finds a property map.
    /// </summary>
    /// <param name="key">The map key.</param>
    /// <returns>The property map if found; otherwise, <c>null</c>.</returns>
    public PropertyMap? Find(PropertyMapKey key)
        => Items.SingleOrDefault(map => map.Key.Equals(key));

    /// <summary>
    /// Finds a property map.
    /// </summary>
    /// <param name="name">The map key name.</param>
    /// <returns>The property map if found; otherwise, <c>null</c>.</returns>
    public PropertyMap? Find(string name)
        => Items.SingleOrDefault(map => map.Key.Name == name && !map.Key.IsNameIgnored)
        ?? Items.SingleOrDefault(map => map.Key.AlternateNames.Contains(name) && !map.Key.IsNameIgnored);

    /// <summary>
    /// Finds a property map.
    /// </summary>
    /// <param name="number">The map key number.</param>
    /// <returns>The property map if found; otherwise, <c>null</c>.</returns>
    public PropertyMap? Find(uint number)
        => Items.SingleOrDefault(map => map.Key.Number == number);
}
