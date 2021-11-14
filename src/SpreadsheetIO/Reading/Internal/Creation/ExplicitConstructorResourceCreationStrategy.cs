using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

internal class ExplicitConstructorResourceCreationStrategy : IResourceCreationStrategy
{
    public Func<ResourceMapOptions, bool> ApplicabilityHandler { get; } =
        options => options.HasExtension<ExplicitConstructorResourceMapOptionsExtension>();

    public TResource? Create<TResource>(ResourceMap<TResource> map, IResourcePropertyValues<TResource> values)
        where TResource : class
    {
        var constructorParameterTypes = new List<Type>();
        var constructorParameters = new List<object?>();

        var constructorCreationExtension = map.Options.FindExtension<ExplicitConstructorResourceMapOptionsExtension>()!;
        foreach (var propertyName in constructorCreationExtension.PropertyNames)
        {
            var propertyMap = map.Properties.SingleOrDefault(p => p.Property.Name == propertyName);
            if (propertyMap is null)
            {
                throw new ArgumentException(Messages.MissingResourcePropertyForConstructorParameter(propertyName));
            }

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
