using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

[ExcludeFromCodeCoverage]
internal class AssemblyWrapper : IAssemblyWrapper
{
    private readonly Assembly _assembly;

    public AssemblyWrapper(Assembly assembly)
    {
        Guard.Against.Null(assembly, nameof(assembly));

        _assembly = assembly;
    }

    public IReadOnlyCollection<TypeInfo> GetConstructibleTypes()
        => GetLoadableDefinedTypes()
        .Where(t => !t.IsAbstract)
        .Where(t => !t.IsGenericTypeDefinition)
        .ToArray();

    private IEnumerable<TypeInfo> GetLoadableDefinedTypes()
    {
        try
        {
            return _assembly.DefinedTypes;
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t is not null)
                .Select(IntrospectionExtensions.GetTypeInfo!);
        }
    }
}
