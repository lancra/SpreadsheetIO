using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

internal class ImplicitConstructorResourceCreationStrategy : IResourceCreationStrategy
{
    public Func<ResourceMapOptions, bool> ApplicabilityHandler { get; } =
        options => options.HasExtension<ImplicitConstructorResourceMapOptionsExtension>();

    public TResource? Create<TResource>(ResourceMap<TResource> map, IResourcePropertyValues<TResource> values)
        where TResource : class
    {
        var constructorParameterTypes = new List<Type>();
        var constructorParameters = new List<object?>();

        foreach (var propertyMap in map.Properties)
        {
            if (!values.TryGetValue(propertyMap, out var value))
            {
                return default;
            }

            constructorParameterTypes.Add(propertyMap.Property.PropertyType);
            constructorParameters.Add(value);
        }

        // This constructor is validated during resource map resolution and cannot be null here.
        var constructor = typeof(TResource).GetConstructor(constructorParameterTypes.ToArray())!;
        var resource = (TResource)constructor.Invoke(constructorParameters.ToArray());
        return resource;
    }
}
