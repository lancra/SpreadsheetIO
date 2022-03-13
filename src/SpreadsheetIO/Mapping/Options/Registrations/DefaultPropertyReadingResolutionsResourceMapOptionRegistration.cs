using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Reading;

namespace LanceC.SpreadsheetIO.Mapping.Options;

/// <summary>
/// Provides the resolutions used when reading default properties from a resource.
/// </summary>
[ExcludeFromCodeCoverage]
internal class DefaultPropertyReadingResolutionsResourceMapOptionRegistration : IResourceMapOptionRegistration
{
    public DefaultPropertyReadingResolutionsResourceMapOptionRegistration(
        params ResourcePropertyDefaultReadingResolution[] resolutions)
    {
        Resolutions = resolutions;
    }

    /// <summary>
    /// Gets the property default reading resolutions.
    /// </summary>
    public IReadOnlyCollection<ResourcePropertyDefaultReadingResolution> Resolutions { get; }
}
