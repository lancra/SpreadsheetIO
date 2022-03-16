using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping.Options;

/// <summary>
/// Provides a setting to continue reading a spreadsheet after encountering a resource failure.
/// </summary>
[ExcludeFromCodeCoverage]
public class ContinueOnResourceReadingFailureResourceMapOption : IResourceMapOptionRegistration, IResourceMapOption
{
    internal ContinueOnResourceReadingFailureResourceMapOption()
    {
    }
}
