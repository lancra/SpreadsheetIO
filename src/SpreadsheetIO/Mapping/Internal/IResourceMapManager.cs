using System;
using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Mapping.Internal
{
    /// <summary>
    /// Defines the internal manager for resource maps.
    /// </summary>
    internal interface IResourceMapManager
    {
        /// <summary>
        /// Returns the map defined for the resource.
        /// </summary>
        /// <typeparam name="TResource">The resource type.</typeparam>
        /// <returns>The map defined for the resource.</returns>
        /// <exception cref="InvalidOperationException">Thrown when multiple maps are defined for the resource.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when no maps are defined for the resource.</exception>
        ResourceMap<TResource> Single<TResource>()
            where TResource : class;

        /// <summary>
        /// Returns the specified map defined for the resource.
        /// </summary>
        /// <typeparam name="TResource">The resource type.</typeparam>
        /// <typeparam name="TResourceMap">The map type.</typeparam>
        /// <returns>The specified map defined for the resource.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the specified map is not defined for the resource.</exception>
        ResourceMap<TResource> Single<TResource, TResourceMap>()
            where TResource : class
            where TResourceMap : ResourceMap<TResource>;
    }
}
