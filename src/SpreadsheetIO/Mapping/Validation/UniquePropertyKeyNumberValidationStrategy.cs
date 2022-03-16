using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

internal class UniquePropertyKeyNumberValidationStrategy : IResourceMapBuilderValidationStrategy
{
    public ResourceMapBuilderValidationResult Validate(IInternalResourceMapBuilder resourceMapBuilder)
    {
        var duplicateKeyNumbers = resourceMapBuilder.Properties.Where(property => property.KeyBuilder.Key.Number.HasValue)
            .GroupBy(property => property.KeyBuilder.Key.Number)
            .Where(propertyGrouping => propertyGrouping.Count() > 1)
            .Select(propertyGrouping => propertyGrouping.Key!.Value)
            .ToArray();

        if (duplicateKeyNumbers.Any())
        {
            return ResourceMapBuilderValidationResult.Failure(
                Messages.DuplicatePropertyMapKeyNumbers(resourceMapBuilder.ResourceType, string.Join(',', duplicateKeyNumbers)));
        }

        return ResourceMapBuilderValidationResult.Success();
    }
}
