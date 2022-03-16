using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Options;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Provides a map for a resource.
/// </summary>
[ExcludeFromCodeCoverage]
public class ResourceMap
{
    internal ResourceMap(Type resourceType, IReadOnlyCollection<PropertyMap> properties, MapOptions<IResourceMapOption> options)
    {
        ResourceType = resourceType;
        Properties = properties;
        Options = options;
    }

    /// <summary>
    /// Gets the type of the mapped resource.
    /// </summary>
    public Type ResourceType { get; }

    /// <summary>
    /// Gets the property maps for the resource.
    /// </summary>
    public IReadOnlyCollection<PropertyMap> Properties { get; }

    /// <summary>
    /// Gets the map options.
    /// </summary>
    public MapOptions<IResourceMapOption> Options { get; }
}
