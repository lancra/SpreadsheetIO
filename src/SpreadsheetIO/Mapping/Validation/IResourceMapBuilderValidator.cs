namespace LanceC.SpreadsheetIO.Mapping.Validation;

/// <summary>
/// Defines the validator for verifying resource map builders.
/// </summary>
internal interface IResourceMapBuilderValidator
{
    /// <summary>
    /// Validates a resource map builder.
    /// </summary>
    /// <param name="resourceMapBuilder">The builder to validate.</param>
    /// <returns>The failed validation results.</returns>
    IReadOnlyCollection<ResourceMapBuilderValidationResult> Validate(IInternalResourceMapBuilder resourceMapBuilder);
}
