namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

/// <summary>
/// Defines the factory for creating resource property collections.
/// </summary>
internal interface IResourcePropertyCollectionFactory
{
    /// <summary>
    /// Creates a resource property headers collection.
    /// </summary>
    /// <returns>The resource property headers collection.</returns>
    IResourcePropertyHeaders CreateHeaders();

    /// <summary>
    /// Creates a resource property values collection.
    /// </summary>
    /// <returns>The resource property values collection.</returns>
    IResourcePropertyValues CreateValues();
}
