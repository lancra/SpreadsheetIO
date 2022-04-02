using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

internal class PropertySetterCreationValidationStrategy : IResourceMapBuilderValidationStrategy
{
    public ResourceMapBuilderValidationResult Validate(IInternalResourceMapBuilder resourceMapBuilder)
    {
        if (resourceMapBuilder.TryGetRegistration<ImplicitConstructorResourceMapOptionRegistration>(out var _) ||
            resourceMapBuilder.TryGetRegistration<ExplicitConstructorResourceMapOptionRegistration>(out var _))
        {
            return ResourceMapBuilderValidationResult.Success();
        }

        var invalidPropertyNames = new List<string>();
        foreach (var propertyMapBuilder in resourceMapBuilder.Properties)
        {
            if (propertyMapBuilder.PropertyInfo.GetSetMethod() is null)
            {
                invalidPropertyNames.Add(propertyMapBuilder.PropertyInfo.Name);
            }
        }

        if (invalidPropertyNames.Any())
        {
            return ResourceMapBuilderValidationResult
                .Failure(Messages.InvalidPropertiesForSetterCreation(
                    resourceMapBuilder.ResourceType,
                    string.Join(',', invalidPropertyNames)));
        }

        return ResourceMapBuilderValidationResult.Success();
    }
}
