using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

internal class UniquePropertyKeyNameValidationStrategy : IResourceMapBuilderValidationStrategy
{
    public ResourceMapBuilderValidationResult Validate(IInternalResourceMapBuilder resourceMapBuilder)
    {
        var namedProperties = resourceMapBuilder.Properties.Where(property => !property.KeyBuilder.Key.IsNameIgnored)
            .ToArray();
        var duplicateKeyNames = namedProperties.Select(property => property.KeyBuilder.Key.Name)
            .Concat(namedProperties.SelectMany(property => property.KeyBuilder.Key.AlternateNames))
            .GroupBy(keyName => keyName)
            .Where(keyNameGrouping => keyNameGrouping.Count() > 1)
            .Select(keyNameGrouping => keyNameGrouping.Key)
            .ToArray();

        if (duplicateKeyNames.Any())
        {
            return ResourceMapBuilderValidationResult.Failure(
                Messages.DuplicatePropertyMapKeyNames(resourceMapBuilder.ResourceType, string.Join(',', duplicateKeyNames)));
        }

        return ResourceMapBuilderValidationResult.Success();
    }
}
