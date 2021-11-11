namespace LanceC.SpreadsheetIO.Mapping.Validation;

/// <summary>
/// Defines the validator for a resource map.
/// </summary>
public interface IResourceMapValidator
{
    /// <summary>
    /// Determines whether this validator can be used for a resource map.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="map">The resource map to validate.</param>
    /// <returns><c>true</c> if this validator can be used for the <paramref name="map"/>; otherwise, <c>false</c>.</returns>
    public bool CanValidate<TResource>(ResourceMap<TResource> map)
        where TResource : class;

    /// <summary>
    /// Validates a resource map.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="map">The resource map to validate.</param>
    /// <returns>The resource map validation result.</returns>
    public ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
        where TResource : class;
}
