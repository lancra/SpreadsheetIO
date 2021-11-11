using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

/// <summary>
/// Provides the base validator for a resource map options extension within a resource map.
/// </summary>
public abstract class ResourceMapOptionsExtensionValidator<TResourceMapOptionsExtension> : ResourceMapValidator
    where TResourceMapOptionsExtension : class, IResourceMapOptionsExtension
{
    /// <inheritdoc/>
    public sealed override bool CanValidate<TResource>(ResourceMap<TResource> map)
        where TResource : class
    {
        Guard.Against.Null(map, nameof(map));

        return map.Options.HasExtension<TResourceMapOptionsExtension>();
    }
}
