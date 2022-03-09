using System.Reflection;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

/// <summary>
/// Defines a factory for generating assembly wrappers.
/// </summary>
internal interface IAssemblyWrapperFactory
{
    /// <summary>
    /// Creates an assembly wrapper.
    /// </summary>
    /// <param name="assembly">The assembly to create from.</param>
    /// <returns>The created assembly wrapper.</returns>
    IAssemblyWrapper Create(Assembly assembly);

    /// <summary>
    /// Creates an assembly wrapper.
    /// </summary>
    /// <param name="markerType">The marker type used to determine the assembly to create from.</param>
    /// <returns>The created assembly wrapper.</returns>
    IAssemblyWrapper Create(Type markerType);
}
