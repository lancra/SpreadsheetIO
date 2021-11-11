using LanceC.SpreadsheetIO.Mapping.Validation;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Validators;

internal class ResourceMapAggregateValidator : IResourceMapAggregateValidator
{
    private readonly IEnumerable<IResourceMapValidator> _validators;

    public ResourceMapAggregateValidator(IEnumerable<IResourceMapValidator> validators)
    {
        _validators = validators;
    }

    public ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
        where TResource : class
    {
        var validationResults = new List<ResourceMapValidationResult>();
        foreach (var validator in _validators)
        {
            if (!validator.CanValidate(map))
            {
                continue;
            }

            var validationResult = validator.Validate(map);
            if (validationResult is null)
            {
                // Validators should not return null but since they can be provided by a consumer, this scenario must be handled.
                // A null validation result is functionally interpreted as `ResourceMapValidatonResult.Success()`.
                continue;
            }

            validationResults.Add(validationResult);
        }

        var aggregateValidationResult = ResourceMapValidationResult.Aggregate(validationResults);
        return aggregateValidationResult;
    }
}
