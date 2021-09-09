using LanceC.SpreadsheetIO.Mapping.Validation;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Validators
{
    /// <summary>
    /// Defines the validator used to aggregate validation results for a resource map.
    /// </summary>
    internal interface IResourceMapAggregateValidator
    {
        /// <summary>
        /// Validates a resource map.
        /// </summary>
        /// <typeparam name="TResource">The resource type.</typeparam>
        /// <param name="map">The resource map to validate.</param>
        /// <returns>The aggregate resource map validation result.</returns>
        ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
            where TResource : class;
    }
}
