namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Represents the options for a resource map.
/// </summary>
public abstract class ResourceMapOptions
{
    private readonly IReadOnlyDictionary<Type, IResourceMapOptionsExtension> _extensions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceMapOptions"/> class.
    /// </summary>
    /// <param name="extensions">The options extensions to copy.</param>
    protected ResourceMapOptions(IReadOnlyDictionary<Type, IResourceMapOptionsExtension> extensions)
    {
        _extensions = extensions ?? new Dictionary<Type, IResourceMapOptionsExtension>();
    }

    /// <summary>
    /// Gets the options extensions.
    /// </summary>
    public IEnumerable<IResourceMapOptionsExtension> Extensions
        => _extensions.Values;

    /// <summary>
    /// Gets the value that determines whether the options are frozen against changes.
    /// </summary>
    internal bool IsFrozen { get; private set; }

    /// <summary>
    /// Finds the extension of the specified type.
    /// </summary>
    /// <typeparam name="TExtension">The type of extension to find.</typeparam>
    /// <returns>The extension, or <c>null</c> if none was found.</returns>
    public TExtension? FindExtension<TExtension>()
        where TExtension : class, IResourceMapOptionsExtension
        => _extensions.TryGetValue(typeof(TExtension), out var extension) ? (TExtension)extension : null;

    /// <summary>
    /// Determines the existence of an extension of the specified type.
    /// </summary>
    /// <typeparam name="TExtension">The type of extension to determine existence for.</typeparam>
    /// <returns><c>true</c> when the extension exists; otherwise, <c>false</c>.</returns>
    public bool HasExtension<TExtension>()
        where TExtension : class, IResourceMapOptionsExtension
        => _extensions.ContainsKey(typeof(TExtension));

    /// <summary>
    /// Adds an extension to a new instance of the options.
    /// </summary>
    /// <typeparam name="TExtension">The type of extension to add.</typeparam>
    /// <param name="extension">The extension to add.</param>
    /// <returns>The new options instance with the given extension added.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the options are frozen.</exception>
    internal abstract ResourceMapOptions WithExtension<TExtension>(TExtension extension)
        where TExtension : class, IResourceMapOptionsExtension;

    /// <summary>
    /// Freezes the options against changes.
    /// </summary>
    internal void Freeze()
        => IsFrozen = true;
}
