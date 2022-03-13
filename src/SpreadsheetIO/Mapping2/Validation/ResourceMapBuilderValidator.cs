namespace LanceC.SpreadsheetIO.Mapping2.Validation;

internal class ResourceMapBuilderValidator : IResourceMapBuilderValidator
{
    private readonly IEnumerable<IResourceMapBuilderValidationStrategy> _strategies;

    public ResourceMapBuilderValidator(IEnumerable<IResourceMapBuilderValidationStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IReadOnlyCollection<ResourceMapBuilderValidationResult> Validate(IInternalResourceMapBuilder resourceMapBuilder)
    {
        var failedValidationResults = new List<ResourceMapBuilderValidationResult>();

        foreach (var strategy in _strategies)
        {
            var validationResult = strategy.Validate(resourceMapBuilder);
            if (!validationResult.IsValid)
            {
                failedValidationResults.Add(validationResult);
            }
        }

        return failedValidationResults;
    }
}
