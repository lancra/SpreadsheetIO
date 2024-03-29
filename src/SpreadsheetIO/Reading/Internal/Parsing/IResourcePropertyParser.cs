using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

/// <summary>
/// Defines the parser for generating resource properties from cell values.
/// </summary>
internal interface IResourcePropertyParser
{
    /// <summary>
    /// Parses a cell value into a resource property using the provided map.
    /// </summary>
    /// <param name="cellValue">The cell value from a spreadsheet.</param>
    /// <param name="map">The resource property map.</param>
    /// <param name="value">The parsed value.</param>
    /// <returns>The kind of result from parsing the value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no strategy is defined for the property type defined in the <paramref name="map"/>.
    /// </exception>
    ResourcePropertyParseResultKind TryParse(string cellValue, PropertyMap map, out object? value);
}
