using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping.Validation;

/// <summary>
/// Provides the base validator for a property map options extension within a resource map.
/// </summary>
public abstract class PropertyMapOptionsExtensionValidator<TPropertyMapOptionsExtension> : ResourceMapValidator
    where TPropertyMapOptionsExtension : class, IPropertyMapOptionsExtension
{
    /// <inheritdoc/>
    public sealed override bool CanValidate<TResource>(ResourceMap<TResource> map)
        where TResource : class
    {
        Guard.Against.Null(map, nameof(map));

        return map.Properties.Any(propertyMap => propertyMap.Options.HasExtension<TPropertyMapOptionsExtension>());
    }
}
