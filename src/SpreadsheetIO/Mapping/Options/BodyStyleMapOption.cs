using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping.Options;

/// <summary>
/// Provides a body style for a resource or a property.
/// </summary>
[ExcludeFromCodeCoverage]
public class BodyStyleMapOption :
    IResourceMapOptionRegistration,
    IPropertyMapOptionRegistration,
    IPropertyMapOption
{
    internal BodyStyleMapOption(IndexerKey key, Style style)
    {
        Key = key;
        Style = style;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

    /// <summary>
    /// Gets the style name.
    /// </summary>
    public string Name => Key.Name;

    /// <summary>
    /// Gets the style.
    /// </summary>
    public Style Style { get; }

    internal IndexerKey Key { get; }
}
