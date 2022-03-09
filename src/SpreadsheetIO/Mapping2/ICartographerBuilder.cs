using System.Reflection;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Defines the builder for generating a <see cref="ICartographer"/>.
/// </summary>
public interface ICartographerBuilder
{
    /// <summary>
    /// Builds the cartographer.
    /// </summary>
    /// <returns>The cartographer.</returns>
    ICartographer Build();

    /// <summary>
    /// Applies a defined configuration for a resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to apply the configuration to.</typeparam>
    /// <param name="configuration">The configuration to apply.</param>
    /// <returns>The resulting cartographer builder.</returns>
    ICartographerBuilder ApplyConfiguration<TResource>(IResourceMapConfiguration<TResource> configuration)
        where TResource : class;

    /// <summary>
    /// Applies all resource configurations defined in an assembly.
    /// </summary>
    /// <param name="assembly">The assembly to apply configurations from.</param>
    /// <returns>The resulting cartographer builder.</returns>
    ICartographerBuilder ApplyConfigurationsFromAssembly(Assembly assembly);

    /// <summary>
    /// Applies all resource configurations defined in an assembly.
    /// </summary>
    /// <param name="markerType">The type contained within the assembly to apply configurations from.</param>
    /// <returns>The resulting cartographer builder.</returns>
    ICartographerBuilder ApplyConfigurationsFromAssembly(Type markerType);

    /// <summary>
    /// Configures a resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to configure.</typeparam>
    /// <param name="builderAction">The resource map builder configuration.</param>
    /// <returns>The resulting cartographer builder.</returns>
    ICartographerBuilder Configure<TResource>(Action<ResourceMapBuilder<TResource>> builderAction)
        where TResource : class;
}
