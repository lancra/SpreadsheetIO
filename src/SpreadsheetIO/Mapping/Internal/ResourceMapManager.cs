using System;
using System.Collections.Generic;
using System.Linq;

namespace LanceC.SpreadsheetIO.Mapping.Internal
{
    internal class ResourceMapManager : IResourceMapManager
    {
        private readonly IDictionary<Type, IResourceMap[]> _resourceMaps =
            new Dictionary<Type, IResourceMap[]>();

        public ResourceMapManager(IEnumerable<IResourceMap> resourceMaps)
        {
            _resourceMaps = resourceMaps.GroupBy(map => map.ResourceType)
                .ToDictionary(mapGrouping => mapGrouping.Key, mapGrouping => mapGrouping.ToArray());
        }

        public ResourceMap<TResource> Single<TResource>()
            where TResource : class
        {
            var resourceMaps = All<TResource>();
            if (resourceMaps.Count == 0)
            {
                throw new KeyNotFoundException($"No map is defined for the '{typeof(TResource)}' resource.");
            }
            else if (resourceMaps.Count > 1)
            {
                throw new InvalidOperationException($"Multiple maps are defined for the '{typeof(TResource)}' resource.");
            }

            return resourceMaps.Single();
        }

        public ResourceMap<TResource> Single<TResource, TResourceMap>()
            where TResource : class
            where TResourceMap : ResourceMap<TResource>
        {
            var resourceMaps = All<TResource>();
            var resourceMap = resourceMaps.SingleOrDefault(rm => rm.GetType() == typeof(TResourceMap));
            if (resourceMap is null)
            {
                throw new KeyNotFoundException(
                    $"The '{typeof(TResourceMap)}' map is not defined for the '{typeof(TResource)}' resource.");
            }

            return resourceMap;
        }

        private IReadOnlyCollection<ResourceMap<TResource>> All<TResource>()
            where TResource : class
        {
            var hasResourceMaps = _resourceMaps.TryGetValue(typeof(TResource), out var resourceMaps);
            if (!hasResourceMaps)
            {
                return Array.Empty<ResourceMap<TResource>>();
            }

            return resourceMaps!.Cast<ResourceMap<TResource>>()
                .ToArray();
        }
    }
}
