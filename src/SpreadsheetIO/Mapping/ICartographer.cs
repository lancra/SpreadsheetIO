namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Defines the manager for maps.
/// </summary>
public interface ICartographer
{
    /// <summary>
    /// Gets the map for a resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource that is mapped.</typeparam>
    /// <returns>The resource map.</returns>
    /// <exception cref="InvalidResourceMapException">
    /// Thrown when the requested resource map could not be retrieved due to configuration issues.
    /// </exception>
    /// <exception cref="KeyNotFoundException">Thrown when the requested resource has not been mapped.</exception>
    ResourceMap GetMap<TResource>()
        where TResource : class;
}
