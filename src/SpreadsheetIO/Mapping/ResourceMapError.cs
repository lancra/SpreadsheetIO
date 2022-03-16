using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Validation;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Represents an error encountered when building a <see cref="ResourceMap"/>.
/// </summary>
public class ResourceMapError
{
    internal ResourceMapError(
        IReadOnlyCollection<MapOptionConversionResult> conversions,
        IReadOnlyCollection<ResourceMapBuilderValidationResult> validations)
    {
        Conversions = conversions;
        Validations = validations;
    }

    /// <summary>
    /// Gets the failed map option conversions.
    /// </summary>
    public IReadOnlyCollection<MapOptionConversionResult> Conversions { get; }

    /// <summary>
    /// Gets the failed map builder validations.
    /// </summary>
    public IReadOnlyCollection<ResourceMapBuilderValidationResult> Validations { get; }
}
