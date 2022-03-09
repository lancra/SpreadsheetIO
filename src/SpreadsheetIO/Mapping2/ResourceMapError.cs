using LanceC.SpreadsheetIO.Mapping2.Options.Converters;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Represents an error encountered when building a <see cref="ResourceMap"/>.
/// </summary>
public class ResourceMapError
{
    internal ResourceMapError(IReadOnlyCollection<MapOptionConversionResult> conversions)
    {
        Conversions = conversions;
    }

    /// <summary>
    /// Gets the failed map option conversions.
    /// </summary>
    public IReadOnlyCollection<MapOptionConversionResult> Conversions { get; }
}
