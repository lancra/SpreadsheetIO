using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

internal class ResourcePropertyValues : IResourcePropertyValues
{
    private readonly IDictionary<PropertyMapKey, object?> _values = new Dictionary<PropertyMapKey, object?>();

    public void Add(PropertyMap map, object? value)
    {
        Guard.Against.Null(map, nameof(map));

        _values.TryAdd(map.Key, value);
    }

    public bool TryGetValue(PropertyMap map, out object? value)
    {
        Guard.Against.Null(map, nameof(map));

        return _values.TryGetValue(map.Key, out value);
    }
}
