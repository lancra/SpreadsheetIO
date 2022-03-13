using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping.Options;

/// <summary>
/// Provides an early exit mechanism for a reading failure on a resource.
/// </summary>
[ExcludeFromCodeCoverage]
public class ExitOnResourceReadingFailureResourceMapOption : IResourceMapOptionRegistration, IResourceMapOption
{
    internal ExitOnResourceReadingFailureResourceMapOption()
    {
    }
}
