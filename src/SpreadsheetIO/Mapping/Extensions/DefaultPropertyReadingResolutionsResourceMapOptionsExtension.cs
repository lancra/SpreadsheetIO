using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class DefaultPropertyReadingResolutionsResourceMapOptionsExtension : IResourceMapOptionsExtension
    {
        public DefaultPropertyReadingResolutionsResourceMapOptionsExtension(
            params ResourcePropertyDefaultReadingResolution[] resolutions)
        {
            Resolutions = resolutions;
        }

        public IReadOnlyCollection<ResourcePropertyDefaultReadingResolution> Resolutions { get; }
    }
}
