using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

internal class PropertySettersResourceCreationStrategy : IResourceCreationStrategy
{
    public Func<ResourceMapOptions, bool> ApplicabilityHandler { get; } =
        options => !options.HasExtension<ExplicitConstructorResourceMapOptionsExtension>() &&
            !options.HasExtension<ImplicitConstructorResourceMapOptionsExtension>();

    public TResource? Create<TResource>(ResourceMap<TResource> map, IResourcePropertyValues<TResource> values)
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
