using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

/// <summary>
/// Defines the collection of values for resource properties.
/// </summary>
internal interface IResourcePropertyValues
{
    /// <summary>
    /// Adds a value for a property map.
    /// </summary>
    /// <param name="map">The resource property map.</param>
    /// <param name="value">The property value.</param>
    void Add(PropertyMap map, object? value);

    /// <summary>
    /// Attempts to retrieve a value for a property map.
    /// </summary>
    /// <param name="map">The resource property map.</param>
    /// <param name="value">The value for the resource property map or <c>null</c> if none is found.</param>
    /// <returns><c>true</c> if a value is defined for the property map; otherwise, <c>false</c>.</returns>
    bool TryGetValue(PropertyMap map, out object? value);
}
