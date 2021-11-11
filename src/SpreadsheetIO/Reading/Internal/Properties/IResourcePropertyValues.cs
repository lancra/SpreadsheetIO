using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

/// <summary>
/// Defines the collection of values for resource properties.
/// </summary>
/// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
internal interface IResourcePropertyValues<TResource>
    where TResource : class
{
    /// <summary>
    /// Adds a value for a property map.
    /// </summary>
    /// <param name="map">The resource property map.</param>
    /// <param name="value">The property value.</param>
    void Add(PropertyMap<TResource> map, object? value);

    /// <summary>
    /// Attempts to retrieve a value for a property map.
    /// </summary>
    /// <param name="map">The resource property map.</param>
    /// <param name="value">The value for the resource property map or <c>null</c> if none is found.</param>
    /// <returns><c>true</c> if a value is defined for the property map; otherwise, <c>false</c>.</returns>
    bool TryGetValue(PropertyMap<TResource> map, out object? value);
}
