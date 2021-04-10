using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Represents the options for a resource map.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the map is defined for.</typeparam>
    public class ResourceMapOptions<TResource> : ResourceMapOptions
        where TResource : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMapOptions{TResource}"/> class.
        /// </summary>
        public ResourceMapOptions()
            : base(new Dictionary<Type, IResourceMapOptionsExtension>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMapOptions{TResource}"/> class.
        /// </summary>
        /// <param name="extensions">The options extensions to copy.</param>
        public ResourceMapOptions(IReadOnlyDictionary<Type, IResourceMapOptionsExtension> extensions)
            : base(extensions)
        {
        }

        /// <summary>
        /// Adds an extension to a new instance of the options.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to add.</typeparam>
        /// <param name="extension">The extension to add.</param>
        /// <returns>The new options instance with the given extension added.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the options are frozen.</exception>
        public override ResourceMapOptions WithExtension<TExtension>(TExtension extension)
        {
            Guard.Against.Null(extension, nameof(extension));

            ThrowIfFrozen();

            var extensions = Extensions.ToDictionary(e => e.GetType(), e => e);
            extensions[typeof(TExtension)] = extension;

            return new ResourceMapOptions<TResource>(extensions);
        }

        private void ThrowIfFrozen()
        {
            if (IsFrozen)
            {
                throw new InvalidOperationException("The resource map options are frozen and cannot be modified.");
            }
        }
    }
}
