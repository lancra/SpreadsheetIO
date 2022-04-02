using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping;

internal class Cartographer : ICartographer
{
    private readonly IDictionary<Type, ResourceMapResult> _resourceMaps;

    internal Cartographer(IDictionary<Type, ResourceMapResult> resourceMaps)
    {
        _resourceMaps = resourceMaps;
    }

    public ResourceMap GetMap<TResource>()
        where TResource : class
    {
        if (!_resourceMaps.TryGetValue(typeof(TResource), out var resourceMapResult))
        {
            throw new KeyNotFoundException(Messages.MissingMapForResourceType(typeof(TResource)));
        }

        if (!resourceMapResult.IsValid)
        {
            throw new InvalidResourceMapException(typeof(TResource), resourceMapResult.Error!);
        }

        return resourceMapResult.Value!;
    }
}
