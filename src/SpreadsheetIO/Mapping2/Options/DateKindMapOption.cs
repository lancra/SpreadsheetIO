using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Mapping2.Options;

/// <summary>
/// Provides a date format for a resource or a property.
/// </summary>
[ExcludeFromCodeCoverage]
public class DateKindMapOption :
    IResourceMapOptionRegistration,
    IPropertyMapOptionRegistration,
    IPropertyMapOption
{
    internal DateKindMapOption(CellDateKind dateKind)
    {
        DateKind = dateKind;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Type> AllowedTypes
        => new[]
        {
            typeof(DateTime),
            typeof(DateTimeOffset),
        };

    /// <summary>
    /// Gets the date format.
    /// </summary>
    public CellDateKind DateKind { get; }
}
