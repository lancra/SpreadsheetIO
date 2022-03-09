namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Represents the result from building a <see cref="PropertyMap"/>.
/// </summary>
public class PropertyMapResult
{
    private PropertyMapResult(bool isValid, PropertyMap? value, PropertyMapError? error)
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
    /// Gets the property map or <c>null</c> if the map is not valid.
    /// </summary>
    public PropertyMap? Value { get; }

    /// <summary>
    /// Gets the error for a failed map or <c>null</c> if the map is valid.
    /// </summary>
    public PropertyMapError? Error { get; }

    /// <summary>
    /// Creates the result for a successful map.
    /// </summary>
    /// <param name="value">The property map.</param>
    /// <returns>The successful property map result.</returns>
    public static PropertyMapResult Success(PropertyMap value)
        => new(true, value, default);

    /// <summary>
    /// Creates the result for a failed map.
    /// </summary>
    /// <param name="error">The property map error.</param>
    /// <returns>The failed property map result.</returns>
    public static PropertyMapResult Failure(PropertyMapError error)
        => new(false, default, error);
}
