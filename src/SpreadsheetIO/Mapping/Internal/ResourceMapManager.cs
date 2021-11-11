using LanceC.SpreadsheetIO.Mapping.Internal.Validators;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Internal
{
    internal class ResourceMapManager : IResourceMapManager
    {
        private readonly IResourceMapAggregateValidator _resourceMapValidator;
        private readonly IDictionary<Type, IResourceMap[]> _resourceMaps =
            new Dictionary<Type, IResourceMap[]>();

        public ResourceMapManager(IResourceMapAggregateValidator resourceMapValidator, IEnumerable<IResourceMap> resourceMaps)
        {
            _resourceMapValidator = resourceMapValidator;
            _resourceMaps = resourceMaps.GroupBy(map => map.ResourceType)
                .ToDictionary(mapGrouping => mapGrouping.Key, mapGrouping => mapGrouping.ToArray());
        }

        public ResourceMap<TResource> Single<TResource>()
            where TResource : class
        {
            var resourceMaps = All<TResource>();
            if (resourceMaps.Count == 0)
            {
                throw new KeyNotFoundException(Messages.MissingMapForResourceType(typeof(TResource).Name));
            }
            else if (resourceMaps.Count > 1)
            {
                throw new InvalidOperationException(Messages.DuplicateMapForResourceType(typeof(TResource).Name));
            }

            var resourceMap = resourceMaps.Single();
            Validate(resourceMap);
            return resourceMap;
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
                    Messages.MissingMapForResourceMapType(typeof(TResourceMap).Name, typeof(TResource).Name));
            }

            Validate(resourceMap);
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

        private void Validate<TResource>(ResourceMap<TResource> map)
            where TResource : class
        {
            var validationResult = _resourceMapValidator.Validate(map);
            if (!validationResult.IsValid)
            {
                throw new ResourceMapValidationException(validationResult);
            }
        }
    }
}
