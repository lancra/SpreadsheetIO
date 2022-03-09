using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading;

namespace LanceC.SpreadsheetIO.Mapping2.Options;

/// <summary>
/// Provides a default value for a property.
/// </summary>
[ExcludeFromCodeCoverage]
public class DefaultValuePropertyMapOption : IPropertyMapOption
{
    internal DefaultValuePropertyMapOption(object value, IReadOnlyCollection<ResourcePropertyDefaultReadingResolution> resolutions)
    {
        Value = value;
        Resolutions = resolutions;
    }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Gets the property default reading resolutions.
    /// </summary>
    public IReadOnlyCollection<ResourcePropertyDefaultReadingResolution> Resolutions { get; }
}
