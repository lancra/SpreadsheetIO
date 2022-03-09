using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

/// <summary>
/// Defines the collection of resource property headers from a spreadsheet.
/// </summary>
internal interface IResourcePropertyHeaders
{
    /// <summary>
    /// Gets the column numbers that the headers are found in.
    /// </summary>
    IEnumerable<uint> ColumnNumbers { get; }

    /// <summary>
    /// Adds a property map to the collection by column number.
    /// </summary>
    /// <param name="map">The property map which represents a header.</param>
    /// <param name="columnNumber">The column number where the header is defined.</param>
    void Add(PropertyMap map, uint columnNumber);

    /// <summary>
    /// Determines whether a property map is identified as a header.
    /// </summary>
    /// <param name="map">The property map to check existence of.</param>
    /// <returns><c>true</c> if the property map is a header; otherwise, <c>false</c>.</returns>
    bool ContainsMap(PropertyMap map);

    /// <summary>
    /// Creates a tracker used to determine whether a header has a value within a body row.
    /// </summary>
    /// <returns>The header usage tracker.</returns>
    IResourcePropertyHeaderUsageTracker CreateUsageTracker();

    /// <summary>
    /// Gets the property map for a column number.
    /// </summary>
    /// <param name="columnNumber">The column number for the header.</param>
    /// <returns>The property map for the provided column number.</returns>
    PropertyMap GetMap(uint columnNumber);

    /// <summary>
    /// Attempts to get a property map for a column number.
    /// </summary>
    /// <param name="columnNumber">The column number for the header.</param>
    /// <param name="map">The property map for the provided column number or <c>null</c> if none exists.</param>
    /// <returns><c>true</c> if a property map is found; otherwise, <c>false</c>.</returns>
    bool TryGetMap(uint columnNumber, out PropertyMap? map);
}
