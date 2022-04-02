using LanceC.SpreadsheetIO.Mapping.Builders;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

/// <summary>
/// Defines a strategy for verifying resource map builders.
/// </summary>
internal interface IResourceMapBuilderValidationStrategy
{
    /// <summary>
    /// Validates a resource map builder.
    /// </summary>
    /// <param name="resourceMapBuilder">The builder to validate.</param>
    /// <returns>The validation result.</returns>
    ResourceMapBuilderValidationResult Validate(IInternalResourceMapBuilder resourceMapBuilder);
}
