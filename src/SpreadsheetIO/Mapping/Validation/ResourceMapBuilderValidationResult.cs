namespace LanceC.SpreadsheetIO.Mapping.Validation;

/// <summary>
/// Represents the result of validating a resource map builder.
/// </summary>
public class ResourceMapBuilderValidationResult
{
    private ResourceMapBuilderValidationResult(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
    }

    /// <summary>
    /// Gets a value indicating whether the validation passed.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the validation failure message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Creates the result for a successful validation.
    /// </summary>
    /// <returns>The successful validation result.</returns>
    public static ResourceMapBuilderValidationResult Success()
        => new(true, string.Empty);

    /// <summary>
    /// Creates the result for a failed validation.
    /// </summary>
    /// <param name="message">The validation failure message.</param>
    /// <returns>The failed validation result.</returns>
    public static ResourceMapBuilderValidationResult Failure(string message)
        => new(false, message);
}
