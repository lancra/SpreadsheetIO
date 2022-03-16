using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

internal class ConstructorResourceCreationStrategy : IResourceCreationStrategy
{
    public Func<ResourceMap, bool> ApplicabilityHandler { get; } =
        map => map.Options.Has<ConstructorResourceMapOption>();

    public TResource? Create<TResource>(ResourceMap map, IResourcePropertyValues values)
        where TResource : class
    {
        var constructorOption = map.Options.Find<ConstructorResourceMapOption>();
        var constructorParameters = new List<object?>();
        foreach (var propertyKey in constructorOption!.PropertyKeys)
        {
            var propertyMap = map.Properties.SingleOrDefault(p => p.Key.Equals(propertyKey));
            if (propertyMap is null)
            {
                throw new ArgumentException(Messages.MissingResourcePropertyForConstructorParameter(propertyKey.Name));
            }

            if (!values.TryGetValue(propertyMap, out var value))
            {
                if (propertyMap.Options.Has<OptionalPropertyMapOption>())
                {
                    value = Type.Missing;
                }
                else
                {
                    return default;
                }
            }

            constructorParameters.Add(value);
        }

        var resource = (TResource)constructorOption.Constructor.Invoke(constructorParameters.ToArray());
        return resource;
    }
}
