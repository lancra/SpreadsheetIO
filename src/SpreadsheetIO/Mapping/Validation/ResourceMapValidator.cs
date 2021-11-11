namespace LanceC.SpreadsheetIO.Mapping.Validation;

/// <summary>
/// Provides the base validator for a resource map.
/// </summary>
public abstract class ResourceMapValidator : IResourceMapValidator
{
    /// <inheritdoc/>
    public virtual bool CanValidate<TResource>(ResourceMap<TResource> map)
        where TResource : class
        => true;

    /// <inheritdoc/>
    public abstract ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
        where TResource : class;
}
