using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Represents the options for a property map.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    public class PropertyMapOptions<TResource, TProperty> : PropertyMapOptions<TResource>
        where TResource : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapOptions{TResource, TProperty}"/> class.
        /// </summary>
        public PropertyMapOptions()
            : base(new Dictionary<Type, IPropertyMapOptionsExtension>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapOptions{TResource, TProperty}"/> class.
        /// </summary>
        /// <param name="extensions">The options extensions to copy.</param>
        public PropertyMapOptions(IReadOnlyDictionary<Type, IPropertyMapOptionsExtension> extensions)
            : base(extensions)
        {
        }

        /// <summary>
        /// Adds an extension to a new instance of the options.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to add.</typeparam>
        /// <param name="extension">The extension to add.</param>
        /// <returns>The new options instance with the given extension added.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the property type is not allowed or when the options are frozen.
        /// </exception>
        public override PropertyMapOptions<TResource> WithExtension<TExtension>(TExtension extension)
        {
            Guard.Against.Null(extension, nameof(extension));

            ThrowIfFrozen();

            if (!IsExtensionAllowed(extension))
            {
                throw new InvalidOperationException(
                    Messages.OptionsExtensionNotAllowedForType(extension.GetType().Name, typeof(TProperty).Name));
            }

            var extensions = Extensions.ToDictionary(e => e.GetType(), e => e);
            extensions[typeof(TExtension)] = extension;

            return new PropertyMapOptions<TResource, TProperty>(extensions);
        }

        internal PropertyMapOptions<TResource> WithExtensionInternal(IPropertyMapOptionsExtension extension)
        {
            Guard.Against.Null(extension, nameof(extension));

            ThrowIfFrozen();

            if (!IsExtensionAllowed(extension))
            {
                return this;
            }

            var extensions = Extensions.ToDictionary(e => e.GetType(), e => e);
            if (extensions.ContainsKey(extension.GetType()))
            {
                return this;
            }

            extensions[extension.GetType()] = extension;
            return new PropertyMapOptions<TResource, TProperty>(extensions);
        }

        private static bool IsExtensionAllowed(IPropertyMapOptionsExtension extension)
            => !extension.AllowedTypes.Any() ||
            extension.AllowedTypes.Any(t => t == typeof(TProperty) || t == Nullable.GetUnderlyingType(typeof(TProperty)));

        private void ThrowIfFrozen()
        {
            if (IsFrozen)
            {
                throw new InvalidOperationException(Messages.FrozenMapOptions);
            }
        }
    }
}
