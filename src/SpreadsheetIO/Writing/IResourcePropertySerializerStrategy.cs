using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing;

/// <summary>
/// Defines a strategy for serializing a resource property.
/// </summary>
public interface IResourcePropertySerializerStrategy
{
    /// <summary>
    /// Gets the property types that can be serialized with this strategy.
    /// </summary>
    IReadOnlyCollection<Type> PropertyTypes { get; }

    /// <summary>
    /// Serializes a resource property into a cell value using the provided map.
    /// </summary>
    /// <typeparam name="TResource">The type of resource that the <paramref name="map"/> applies to.</typeparam>
    /// <param name="value">The value of the resource property.</param>
    /// <param name="map">The resource property map.</param>
    /// <returns>The serialized cell value.</returns>
    WritingCellValue Serialize<TResource>(object? value, PropertyMap<TResource> map)
        where TResource : class;
}
