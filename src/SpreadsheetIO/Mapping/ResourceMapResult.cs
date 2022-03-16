namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Represents the result from building a <see cref="ResourceMap"/>.
/// </summary>
public class ResourceMapResult
{
    private ResourceMapResult(bool isValid, ResourceMap? value, ResourceMapError? error)
    {
        IsValid = isValid;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Gets the value that determines whether the map is valid.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the resource map or <c>null</c> if the map is not valid.
    /// </summary>
    public ResourceMap? Value { get; }

    /// <summary>
    /// Gets the error for a failed map or <c>null</c> if the map is valid.
    /// </summary>
    public ResourceMapError? Error { get; }

    /// <summary>
    /// Creates the result for a successful map.
    /// </summary>
    /// <param name="value">The resource map.</param>
    /// <returns>The successful resource map result.</returns>
    public static ResourceMapResult Success(ResourceMap value)
        => new(true, value, default);

    /// <summary>
    /// Creates the result for a failed map.
    /// </summary>
    /// <param name="error">The resource map error.</param>
    /// <returns>The failed resource map result.</returns>
    public static ResourceMapResult Failure(ResourceMapError error)
        => new(false, default, error);
}
