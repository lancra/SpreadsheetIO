using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

internal class ExplicitConstructorResourceMapOptionConversionStrategy :
    ResourceMapOptionConversionStrategy<ExplicitConstructorResourceMapOptionRegistration>
{
    public override MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        ExplicitConstructorResourceMapOptionRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder)
    {
        Guard.Against.Null(registration, nameof(registration));
        Guard.Against.Null(resourceMapBuilder, nameof(resourceMapBuilder));

        var constructorParameters = new List<(Type Type, PropertyMapKey Key)>();
        var unmappedPropertyNames = new List<string>();
        foreach (var propertyName in registration.PropertyNames)
        {
            var propertyMapBuilder = resourceMapBuilder.Properties.SingleOrDefault(pb => pb.PropertyInfo.Name == propertyName);
            if (propertyMapBuilder is null)
            {
                unmappedPropertyNames.Add(propertyName);
                continue;
            }

            constructorParameters.Add((propertyMapBuilder.PropertyInfo.PropertyType, propertyMapBuilder.KeyBuilder.Key));
        }

        if (unmappedPropertyNames.Any())
        {
            return MapOptionConversionResult.Failure<IResourceMapOption>(
                registration,
                Messages.MissingMapForResourceProperties(
                    resourceMapBuilder.ResourceType.Name,
                    string.Join(',', unmappedPropertyNames)));
        }

        var constructor = resourceMapBuilder.ResourceType.GetConstructor(
            constructorParameters.Select(parameter => parameter.Type).ToArray());
        if (constructor is null)
        {
            return MapOptionConversionResult.Failure<IResourceMapOption>(
                registration,
                Messages.MissingResourceConstructor(resourceMapBuilder.ResourceType.Name));
        }

        var option = new ConstructorResourceMapOption(constructor, constructorParameters.Select(parameter => parameter.Key).ToArray());
        return MapOptionConversionResult.Success<IResourceMapOption>(registration, option);
    }
}
