using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

internal class PropertySettersResourceCreationStrategy : IResourceCreationStrategy
{
    public Func<ResourceMap, bool> ApplicabilityHandler { get; } = map => !map.Options.Has<ConstructorResourceMapOption>();

    public TResource? Create<TResource>(ResourceMap map, IResourcePropertyValues values)
        where TResource : class
    {
        var resource = Activator.CreateInstance<TResource>();

        foreach (var resourceProperty in map.Properties)
        {
            var hasValue = values.TryGetValue(resourceProperty, out var value);
            if (hasValue)
            {
                resourceProperty.Property.SetValue(resource, value);
            }
        }

        return resource;
    }
}
