using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

[ExcludeFromCodeCoverage]
internal class AssemblyWrapperFactory : IAssemblyWrapperFactory
{
    public IAssemblyWrapper Create(Assembly assembly)
        => new AssemblyWrapper(assembly);

    public IAssemblyWrapper Create(Type markerType)
    {
        var assembly = Assembly.GetAssembly(markerType);
        if (assembly is null)
        {
            throw new InvalidOperationException(Messages.CannotGetAssemblyFromType(markerType.Name));
        }

        return new AssemblyWrapper(assembly);
    }
}
