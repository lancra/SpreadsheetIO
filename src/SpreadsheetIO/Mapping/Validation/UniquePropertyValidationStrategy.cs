using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

internal class UniquePropertyValidationStrategy : IResourceMapBuilderValidationStrategy
{
    public ResourceMapBuilderValidationResult Validate(IInternalResourceMapBuilder resourceMapBuilder)
    {
        var duplicatePropertyNames = resourceMapBuilder.Properties.GroupBy(property => property.PropertyInfo.Name)
            .Where(propertyGrouping => propertyGrouping.Count() > 1)
            .Select(propertyGrouping => propertyGrouping.Key)
            .ToArray();

        if (duplicatePropertyNames.Any())
        {
            return ResourceMapBuilderValidationResult.Failure(
                Messages.DuplicatePropertyMaps(resourceMapBuilder.ResourceType, string.Join(',', duplicatePropertyNames)));
        }

        return ResourceMapBuilderValidationResult.Success();
    }
}
