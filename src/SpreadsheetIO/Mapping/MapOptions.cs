using LanceC.SpreadsheetIO.Mapping.Options;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Provides a collection of map options.
/// </summary>
/// <typeparam name="TOptionKind">The type representing the kind of map option.</typeparam>
public class MapOptions<TOptionKind>
    where TOptionKind : IMapOption
{
    private readonly IReadOnlyDictionary<Type, TOptionKind> _options;

    internal MapOptions(IReadOnlyDictionary<Type, TOptionKind> options)
    {
        _options = options;
    }

    /// <summary>
    /// Finds a map option.
    /// </summary>
    /// <typeparam name="TOption">The type of map option.</typeparam>
    /// <returns>The map option if found; otherwise, <c>null</c>.</returns>
    public TOption? Find<TOption>()
        where TOption : class, TOptionKind
        => _options.TryGetValue(typeof(TOption), out var option) ? (TOption)option : null;

    /// <summary>
    /// Checks for the existence of a map option.
    /// </summary>
    /// <typeparam name="TOption">The type of map option.</typeparam>
    /// <returns><c>true</c> if the map option is found; otherwise, <c>false</c>.</returns>
    public bool Has<TOption>()
        where TOption : class, TOptionKind
        => _options.ContainsKey(typeof(TOption));
}
