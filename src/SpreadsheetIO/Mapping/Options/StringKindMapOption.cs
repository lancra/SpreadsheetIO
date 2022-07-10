using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Mapping.Options;

/// <summary>
/// Provides a string format for a resource or a property.
/// </summary>
[ExcludeFromCodeCoverage]
public class StringKindMapOption :
    IResourceMapOptionRegistration,
    IPropertyMapOptionRegistration,
    IPropertyMapOption
{
    internal StringKindMapOption(CellStringKind stringKind)
    {
        StringKind = stringKind;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Type> AllowedTypes
        => new[]
        {
            typeof(char),
            typeof(string),
        };

    /// <summary>
    /// Gets the string format.
    /// </summary>
    public CellStringKind StringKind { get; }
}
