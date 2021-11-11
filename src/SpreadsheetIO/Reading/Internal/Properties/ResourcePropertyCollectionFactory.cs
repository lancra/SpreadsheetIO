using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

[ExcludeFromCodeCoverage]
internal class ResourcePropertyCollectionFactory : IResourcePropertyCollectionFactory
{
    public IResourcePropertyHeaders<TResource> CreateHeaders<TResource>()
        where TResource : class
        => new ResourcePropertyHeaders<TResource>();

    public IResourcePropertyValues<TResource> CreateValues<TResource>()
        where TResource : class
        => new ResourcePropertyValues<TResource>();
}
