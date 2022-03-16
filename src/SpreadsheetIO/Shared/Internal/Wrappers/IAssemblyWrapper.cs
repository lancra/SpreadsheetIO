using System.Reflection;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

/// <summary>
/// Defines a wrapper for an assembly.
/// </summary>
internal interface IAssemblyWrapper
{
    /// <summary>
    /// Gets types that can be constructed via reflection.
    /// </summary>
    /// <returns>The constructible types.</returns>
    IReadOnlyCollection<TypeInfo> GetConstructibleTypes();
}
