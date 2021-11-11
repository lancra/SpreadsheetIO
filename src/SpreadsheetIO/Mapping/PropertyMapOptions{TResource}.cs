namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Represents the options for a property map.
    /// </summary>
    /// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
    public abstract class PropertyMapOptions<TResource>
        where TResource : class
    {
        private readonly IReadOnlyDictionary<Type, IPropertyMapOptionsExtension> _extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapOptions{TResource}"/> class.
        /// </summary>
        /// <param name="extensions">The options extensions to copy.</param>
        protected PropertyMapOptions(IReadOnlyDictionary<Type, IPropertyMapOptionsExtension> extensions)
        {
            _extensions = extensions ?? new Dictionary<Type, IPropertyMapOptionsExtension>();
        }

        /// <summary>
        /// Gets the options extensions.
        /// </summary>
        public IEnumerable<IPropertyMapOptionsExtension> Extensions
            => _extensions.Values;

        /// <summary>
        /// Gets the value that determines whether the options are frozen against changes.
        /// </summary>
        public bool IsFrozen { get; private set; }

        /// <summary>
        /// Finds the extension of the specified type.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to find.</typeparam>
        /// <returns>The extension, or <c>null</c> if none was found.</returns>
        public TExtension? FindExtension<TExtension>()
            where TExtension : class, IPropertyMapOptionsExtension
            => _extensions.TryGetValue(typeof(TExtension), out var extension) ? (TExtension)extension : null;

        /// <summary>
        /// Determines the existence of an extension of the specified type.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to determine existence for.</typeparam>
        /// <returns><c>true</c> when the extension exists; otherwise, <c>false</c>.</returns>
        public bool HasExtension<TExtension>()
            where TExtension : class, IPropertyMapOptionsExtension
            => _extensions.ContainsKey(typeof(TExtension));

        /// <summary>
        /// Adds an extension to a new instance of the options.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to add.</typeparam>
        /// <param name="extension">The extension to add.</param>
        /// <returns>The new options instance with the given extension added.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the property type is not allowed or when the options are frozen.
        /// </exception>
        public abstract PropertyMapOptions<TResource> WithExtension<TExtension>(TExtension extension)
            where TExtension : class, IPropertyMapOptionsExtension;

        /// <summary>
        /// Freezes the options against changes.
        /// </summary>
        public void Freeze()
            => IsFrozen = true;
    }
}
