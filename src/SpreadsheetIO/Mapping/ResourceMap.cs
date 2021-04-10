using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Represents a map to Excel for a resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to map.</typeparam>
    public abstract class ResourceMap<TResource> : IResourceMap
        where TResource : class
    {
        private List<PropertyMap<TResource>> _properties = new();
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
        /// <param name="keyAction">The callback used to modify the map key.</param>
        /// <returns>The resulting resource map.</returns>
        protected ResourceMap<TResource> Map<TProperty>(
            Expression<Func<TResource, TProperty>> property,
            Action<PropertyMapKeyBuilder> keyAction)
            => Map(property, keyAction, options => { });

        /// <summary>
        /// Defines a property map within the resource.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to map.</typeparam>
        /// <param name="property">The property to map.</param>
        /// <param name="optionsAction">The callback used to modify the map options.</param>
        /// <returns>The resulting resource map.</returns>
        protected ResourceMap<TResource> Map<TProperty>(
            Expression<Func<TResource, TProperty>> property,
            Action<PropertyMapOptionsBuilder<TResource, TProperty>> optionsAction)
            => Map(property, key => { }, optionsAction);

        /// <summary>
        /// Defines a property map within the resource.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to map.</typeparam>
        /// <param name="property">The property to map.</param>
        /// <param name="keyAction">The callback used to modify the map key.</param>
        /// <param name="optionsAction">The callback used to modify the map options.</param>
        /// <returns>The resulting resource map.</returns>
        protected ResourceMap<TResource> Map<TProperty>(
            Expression<Func<TResource, TProperty>> property,
            Action<PropertyMapKeyBuilder> keyAction,
            Action<PropertyMapOptionsBuilder<TResource, TProperty>> optionsAction)
        {
            Guard.Against.Null(property, nameof(property));

            var propertyInfo = GetPropertyInfo(property);

            var keyBuilder = new PropertyMapKeyBuilder(propertyInfo);
            keyAction?.Invoke(keyBuilder);

            var optionsBuilder = new PropertyMapOptionsBuilder<TResource, TProperty>(Options)
                .ApplyResourceMapOptions();
            optionsAction?.Invoke(optionsBuilder);

            optionsBuilder.Options.Freeze();

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

            throw new ArgumentException("The provided expression does not represent a resource property.");
        }
    }
}
