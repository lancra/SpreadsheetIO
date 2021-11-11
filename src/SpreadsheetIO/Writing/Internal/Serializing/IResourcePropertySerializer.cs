using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

/// <summary>
/// Defines the serializer for generating cell values from resource properties.
/// </summary>
internal interface IResourcePropertySerializer
{
    /// <summary>
    /// Serializes a resource property into a cell value using the provided map.
    /// </summary>
    /// <typeparam name="TResource">The type of resource that the <paramref name="map"/> applies to.</typeparam>
    /// <param name="resource">The resource containing the target property.</param>
    /// <param name="map">The resource property map.</param>
    /// <returns>The serialized cell value.</returns>
    WritingCellValue Serialize<TResource>(TResource resource, PropertyMap<TResource> map)
        where TResource : class;
}
