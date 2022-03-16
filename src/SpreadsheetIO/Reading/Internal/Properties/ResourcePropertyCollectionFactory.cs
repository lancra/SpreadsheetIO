using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

[ExcludeFromCodeCoverage]
internal class ResourcePropertyCollectionFactory : IResourcePropertyCollectionFactory
{
    public IResourcePropertyHeaders CreateHeaders()
        => new ResourcePropertyHeaders();

    public IResourcePropertyValues CreateValues()
        => new ResourcePropertyValues();
}
