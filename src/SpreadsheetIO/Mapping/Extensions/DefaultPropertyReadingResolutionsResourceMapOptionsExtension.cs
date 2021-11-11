using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    /// <summary>
    /// Provides the resolutions used when reading default properties from a resource.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DefaultPropertyReadingResolutionsResourceMapOptionsExtension : IResourceMapOptionsExtension
    {
        internal DefaultPropertyReadingResolutionsResourceMapOptionsExtension(
            params ResourcePropertyDefaultReadingResolution[] resolutions)
        {
            Resolutions = resolutions;
        }

        /// <summary>
        /// Gets the property default reading resolutions.
        /// </summary>
        public IReadOnlyCollection<ResourcePropertyDefaultReadingResolution> Resolutions { get; }
    }
}
