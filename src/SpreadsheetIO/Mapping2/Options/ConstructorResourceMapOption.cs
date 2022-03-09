using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LanceC.SpreadsheetIO.Mapping2.Options;

/// <summary>
/// Provides a constructor for instantiating a resource.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConstructorResourceMapOption : IResourceMapOption
{
    internal ConstructorResourceMapOption(ConstructorInfo constructor, IReadOnlyCollection<PropertyMapKey> propertyKeys)
    {
        Constructor = constructor;
        PropertyKeys = propertyKeys;
    }

    /// <summary>
    /// Gets the resource constructor information.
    /// </summary>
    public ConstructorInfo Constructor { get; }

    /// <summary>
    /// Gets the keys for the property maps used in the constructor.
    /// </summary>
    public IReadOnlyCollection<PropertyMapKey> PropertyKeys { get; }
}
