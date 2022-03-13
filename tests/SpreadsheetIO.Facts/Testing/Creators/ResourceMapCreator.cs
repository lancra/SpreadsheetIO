using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;

namespace LanceC.SpreadsheetIO.Facts.Testing.Creators;

public static class ResourceMapCreator
{
    public static ResourceMap Create<TResource>(IReadOnlyCollection<PropertyMap> properties, params IResourceMapOption[] options)
        where TResource : class
    {
        var mapOptions = new MapOptions<IResourceMapOption>(
            (options ?? Array.Empty<IResourceMapOption>()).ToDictionary(o => o.GetType()));
        var map = new ResourceMap(typeof(TResource), properties, mapOptions);
        return map;
    }
}
