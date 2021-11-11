using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties
{
    internal class ResourcePropertyHeaders<TResource> : IResourcePropertyHeaders<TResource>
        where TResource : class
    {
        private readonly IDictionary<uint, PropertyMap<TResource>> _mapsByNumber = new Dictionary<uint, PropertyMap<TResource>>();
        private readonly IDictionary<PropertyMapKey, uint> _numbersByMap = new Dictionary<PropertyMapKey, uint>();

        public IEnumerable<uint> ColumnNumbers
            => _mapsByNumber.Keys;

        public void Add(PropertyMap<TResource> map, uint columnNumber)
        {
            _mapsByNumber.TryAdd(columnNumber, map);
            _numbersByMap.TryAdd(map.Key, columnNumber);
        }

        public bool ContainsMap(PropertyMap<TResource> map)
            => _numbersByMap.ContainsKey(map.Key);

        public IResourcePropertyHeaderUsageTracker CreateUsageTracker()
            => new ResourcePropertyHeaderUsageTracker(ColumnNumbers);

        public PropertyMap<TResource> GetMap(uint columnNumber)
        {
            var hasMap = TryGetMap(columnNumber, out var map);
            if (!hasMap)
            {
                throw new KeyNotFoundException(Messages.MissingHeaderForColumnNumber(columnNumber));
            }

            return map!;
        }

        public bool TryGetMap(uint columnNumber, out PropertyMap<TResource>? map)
            => _mapsByNumber.TryGetValue(columnNumber, out map);
    }
}
