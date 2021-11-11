namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

/// <summary>
/// Defines the factory for creating resource property collections.
/// </summary>
internal interface IResourcePropertyCollectionFactory
{
    /// <summary>
    /// Creates a resource property headers collection.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the properties are defined in.</typeparam>
    /// <returns>The resource property headers collection.</returns>
    IResourcePropertyHeaders<TResource> CreateHeaders<TResource>()
        where TResource : class;

    /// <summary>
    /// Creates a resource property values collection.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the properties are defined in.</typeparam>
    /// <returns>The resource property values collection.</returns>
    IResourcePropertyValues<TResource> CreateValues<TResource>()
        where TResource : class;
}
