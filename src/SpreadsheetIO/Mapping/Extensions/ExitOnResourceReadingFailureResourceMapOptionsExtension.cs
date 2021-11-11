using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Extensions;

/// <summary>
/// Provides an early exit mechanism for a reading failure on a resource.
/// </summary>
[ExcludeFromCodeCoverage]
public class ExitOnResourceReadingFailureResourceMapOptionsExtension : IResourceMapOptionsExtension
{
    internal ExitOnResourceReadingFailureResourceMapOptionsExtension()
    {
    }
}
