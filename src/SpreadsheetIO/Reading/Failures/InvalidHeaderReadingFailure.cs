using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Failures;

/// <summary>
/// Represents a defined header that the spreadsheet does not match.
/// </summary>
[ExcludeFromCodeCoverage]
public record InvalidHeaderReadingFailure(PropertyMapKey NumberMapKey, PropertyMapKey NameMapKey)
{
    /// <summary>
    /// Gets the property map key defined for the spreadsheet column number.
    /// </summary>
    public PropertyMapKey NumberMapKey { get; init; } = NumberMapKey;

    /// <summary>
    /// Gets the property map key defined for the spreadsheet text.
    /// </summary>
    public PropertyMapKey NameMapKey { get; init; } = NameMapKey;
}
