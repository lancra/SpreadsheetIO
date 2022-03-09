using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Reading;

/// <summary>
/// Defines a strategy for parsing a resource property.
/// </summary>
public interface IResourcePropertyParserStrategy
{
    /// <summary>
    /// Gets the property type that can be parsed with this strategy.
    /// </summary>
    Type PropertyType { get; }

    /// <summary>
    /// Parses a cell value into a resource property using the provided map.
    /// </summary>
    /// <param name="cellValue">The cell value from a spreadsheet.</param>
    /// <param name="map">The resource property map.</param>
    /// <param name="value">The parsed value.</param>
    /// <returns>The kind of result from parsing the value.</returns>
    ResourcePropertyParseResultKind TryParse(string cellValue, PropertyMap map, out object? value);
}
