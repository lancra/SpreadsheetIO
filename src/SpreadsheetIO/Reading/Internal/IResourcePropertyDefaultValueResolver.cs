using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Reading.Internal;

/// <summary>
/// Defines the resolver for resource property default values.
/// </summary>
internal interface IResourcePropertyDefaultValueResolver
{
    /// <summary>
    /// Attempts to resolve a default value for the resource property map.
    /// </summary>
    /// <param name="map">The resource property map.</param>
    /// <param name="parseResultKind">The kind of result from parsing the resource property.</param>
    /// <param name="value">The resulting default value, or <c>null</c> if none is resolved.</param>
    /// <returns><c>true</c> if the default value is resolved; otherwise, <c>false</c>.</returns>
    bool TryResolve(PropertyMap map, ResourcePropertyParseResultKind parseResultKind, out object? value);
}
