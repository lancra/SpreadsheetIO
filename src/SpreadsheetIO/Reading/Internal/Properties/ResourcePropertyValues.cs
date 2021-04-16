using System.Collections.Generic;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties
{
    internal class ResourcePropertyValues<TResource> : IResourcePropertyValues<TResource>
        where TResource : class
    {
        private readonly IDictionary<PropertyMapKey, object?> _values = new Dictionary<PropertyMapKey, object?>();

        public void Add(PropertyMap<TResource> map, object? value)
        {
            Guard.Against.Null(map, nameof(map));

            _values.TryAdd(map.Key, value);
        }

        public bool TryGetValue(PropertyMap<TResource> map, out object? value)
        {
            Guard.Against.Null(map, nameof(map));

            return _values.TryGetValue(map.Key, out value);
        }
    }
}
