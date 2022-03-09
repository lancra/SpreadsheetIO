using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping2.Options;

/// <summary>
/// Provides an overridden header row for a resource.
/// </summary>
[ExcludeFromCodeCoverage]
public class HeaderRowNumberResourceMapOption : IResourceMapOptionRegistration, IResourceMapOption
{
    internal HeaderRowNumberResourceMapOption(uint number)
    {
        Number = number;
    }

    /// <summary>
    /// Gets the header row number.
    /// </summary>
    public uint Number { get; }
}
