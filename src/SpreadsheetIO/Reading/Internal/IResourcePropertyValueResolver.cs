using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal;

/// <summary>
/// Defines the resolver for resource property values.
/// </summary>
internal interface IResourcePropertyValueResolver
{
    /// <summary>
    /// Attempts to resolve a value for the resource property map.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the property resides in.</typeparam>
    /// <param name="cellValue">The cell value from a spreadsheet.</param>
    /// <param name="map">The resource property map.</param>
    /// <param name="value">The resolved value.</param>
    /// <returns><c>true</c> if a valid value is parsed or a default value is resolved; otherwise, <c>false</c>.</returns>
    bool TryResolve<TResource>(string cellValue, PropertyMap<TResource> map, out object? value)
        where TResource : class;
}
