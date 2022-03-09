namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Defines the configuration of a resource map.
/// </summary>
/// <typeparam name="TResource">The type of resource to map.</typeparam>
public interface IResourceMapConfiguration<TResource>
    where TResource : class
{
    /// <summary>
    /// Configures the map of resource type <typeparamref name="TResource"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the resource map.</param>
    void Configure(ResourceMapBuilder<TResource> builder);
}
