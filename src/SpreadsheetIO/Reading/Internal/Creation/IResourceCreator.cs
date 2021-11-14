using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

/// <summary>
/// Defines a creator for instantiating new resources.
/// </summary>
internal interface IResourceCreator
{
    /// <summary>
    /// Creates a new resource from the provided values.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to create.</typeparam>
    /// <param name="map">The resource map.</param>
    /// <param name="values">The resource property values.</param>
    /// <returns>The new resource if successful; otherwise, <c>null</c>.</returns>
    TResource? Create<TResource>(ResourceMap<TResource> map, IResourcePropertyValues<TResource> values)
        where TResource : class;
}
