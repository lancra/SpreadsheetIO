using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping.Options;

/// <summary>
/// Provides an optional setting for a property header or body.
/// </summary>
[ExcludeFromCodeCoverage]
public class OptionalPropertyMapOption : IPropertyMapOptionRegistration, IPropertyMapOption
{
    internal OptionalPropertyMapOption(PropertyElementKind kind)
    {
        Kind = kind;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

    /// <summary>
    /// Gets the optional property element.
    /// </summary>
    public PropertyElementKind Kind { get; }
}
