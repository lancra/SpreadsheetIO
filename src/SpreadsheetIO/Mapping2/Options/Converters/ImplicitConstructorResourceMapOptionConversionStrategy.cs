using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

internal class ImplicitConstructorResourceMapOptionConversionStrategy :
    ResourceMapOptionConversionStrategy<ImplicitConstructorResourceMapOptionRegistration>
{
    public override MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        ImplicitConstructorResourceMapOptionRegistration registration,
        ResourceMapBuilder resourceMapBuilder)
    {
        Guard.Against.Null(registration, nameof(registration));
        Guard.Against.Null(resourceMapBuilder, nameof(resourceMapBuilder));

        var constructorParameters = resourceMapBuilder.Properties
            .Select(pb => (pb.PropertyInfo.PropertyType, pb.KeyBuilder.Key))
            .ToArray();

        var constructor = resourceMapBuilder.ResourceType.GetConstructor(
            constructorParameters.Select(parameter => parameter.PropertyType).ToArray());
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
