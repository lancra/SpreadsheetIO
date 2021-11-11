using System.Linq.Expressions;
using System.Reflection;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Represents a map to Excel for a resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to map.</typeparam>
    public abstract class ResourceMap<TResource> : IResourceMap
        where TResource : class
    {
        private readonly List<PropertyMap<TResource>> _properties = new();
        private ResourceMapOptions<TResource>? _options;

        /// <summary>
        /// Gets the property maps.
        /// </summary>
        public IReadOnlyCollection<PropertyMap<TResource>> Properties
            => _properties;

        /// <inheritdoc/>
        public Type ResourceType { get; } = typeof(TResource);

        internal ResourceMapOptions<TResource> Options
        {
            get
            {
                if (_options is null)
                {
                    var optionsBuilder = new ResourceMapOptionsBuilder<TResource>();
                    Configure(optionsBuilder);

                    _options = optionsBuilder.Options;
                    _options.Freeze();
                }

                return _options;
            }
        }

        /// <summary>
        /// Defines a property map within the resource.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to map.</typeparam>
        /// <param name="property">The property to map.</param>
        /// <returns>The resulting resource map.</returns>
        protected ResourceMap<TResource> Map<TProperty>(Expression<Func<TResource, TProperty>> property)
            => Map(property, key => { }, options => { });

        /// <summary>
        /// Defines a property map within the resource.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to map.</typeparam>
        /// <param name="property">The property to map.</param>
        /// <param name="key">The callback used to modify the map key.</param>
        /// <returns>The resulting resource map.</returns>
        protected ResourceMap<TResource> Map<TProperty>(
            Expression<Func<TResource, TProperty>> property,
            Action<PropertyMapKeyBuilder> key)
            => Map(property, key, options => { });

        /// <summary>
        /// Defines a property map within the resource.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to map.</typeparam>
        /// <param name="property">The property to map.</param>
        /// <param name="options">The callback used to modify the map options.</param>
        /// <returns>The resulting resource map.</returns>
        protected ResourceMap<TResource> Map<TProperty>(
            Expression<Func<TResource, TProperty>> property,
            Action<PropertyMapOptionsBuilder<TResource, TProperty>> options)
            => Map(property, key => { }, options);

        /// <summary>
        /// Defines a property map within the resource.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to map.</typeparam>
        /// <param name="property">The property to map.</param>
        /// <param name="key">The callback used to modify the map key.</param>
        /// <param name="options">The callback used to modify the map options.</param>
        /// <returns>The resulting resource map.</returns>
        protected ResourceMap<TResource> Map<TProperty>(
            Expression<Func<TResource, TProperty>> property,
            Action<PropertyMapKeyBuilder> key,
            Action<PropertyMapOptionsBuilder<TResource, TProperty>> options)
        {
            Guard.Against.Null(property, nameof(property));

            var propertyInfo = GetPropertyInfo(property);

            var keyBuilder = new PropertyMapKeyBuilder(propertyInfo);
            key?.Invoke(keyBuilder);

            var optionsBuilder = new PropertyMapOptionsBuilder<TResource, TProperty>(Options)
                .ApplyResourceMapOptions();
            options?.Invoke(optionsBuilder);

            optionsBuilder.Options.Freeze();

            ValidatePropertyUniqueness(propertyInfo);
            ValidatePropertyKeyUniqueness(keyBuilder.Key);

            var propertyMap = new PropertyMap<TResource>(propertyInfo, keyBuilder.Key, optionsBuilder.Options);
            _properties.Add(propertyMap);

            return this;
        }

        /// <summary>
        /// Configures the map options.
        /// </summary>
        /// <param name="optionsBuilder">The builder for generating map options.</param>
        protected virtual void Configure(ResourceMapOptionsBuilder<TResource> optionsBuilder)
        {
        }

        private static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TResource, TProperty>> property)
        {
            if (property.Body is MemberExpression memberExpression)
            {
                if (memberExpression.Member is PropertyInfo propertyInfo)
                {
                    return propertyInfo;
                }
            }

            throw new ArgumentException(Messages.InvalidResourcePropertyExpression, nameof(property));
        }

        private void ValidatePropertyUniqueness(PropertyInfo property)
        {
            var isDuplicate = _properties.Any(p => p.Property == property);
            if (isDuplicate)
            {
                throw new ArgumentException(Messages.DuplicatePropertyMap(property.Name, GetType().Name), nameof(property));
            }
        }

        private void ValidatePropertyKeyUniqueness(PropertyMapKey key)
        {
            if (!key.IsNameIgnored)
            {
                var isDuplicate = _properties.Any(p => !p.Key.IsNameIgnored && p.Key.Name == key.Name);
                if (isDuplicate)
                {
                    throw new ArgumentException(Messages.DuplicatePropertyMapKeyName(key.Name, GetType().Name), nameof(key));
                }
            }

            if (key.Number.HasValue)
            {
                var isDuplicate = _properties.Any(p => p.Key.Number == key.Number);
                if (isDuplicate)
                {
                    throw new ArgumentException(Messages.DuplicatePropertyMapKeyNumber(key.Number, GetType().Name), nameof(key));
                }
            }
        }
    }
}
