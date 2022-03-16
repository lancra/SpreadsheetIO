using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

internal class UniquePropertyKeyNameValidationStrategy : IResourceMapBuilderValidationStrategy
{
    public ResourceMapBuilderValidationResult Validate(IInternalResourceMapBuilder resourceMapBuilder)
    {
        var duplicateKeyNames = resourceMapBuilder.Properties.Where(property => !property.KeyBuilder.Key.IsNameIgnored)
            .GroupBy(property => property.KeyBuilder.Key.Name)
            .Where(propertyGrouping => propertyGrouping.Count() > 1)
            .Select(propertyGrouping => propertyGrouping.Key)
            .ToArray();

        if (duplicateKeyNames.Any())
        {
            return ResourceMapBuilderValidationResult.Failure(
                Messages.DuplicatePropertyMapKeyNames(resourceMapBuilder.ResourceType, string.Join(',', duplicateKeyNames)));
        }

        return ResourceMapBuilderValidationResult.Success();
    }
}
