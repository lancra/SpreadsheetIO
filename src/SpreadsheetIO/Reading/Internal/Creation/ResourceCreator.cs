using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

internal class ResourceCreator : IResourceCreator
{
    private readonly IEnumerable<IResourceCreationStrategy> _resourceCreationStrategies;

    public ResourceCreator(IEnumerable<IResourceCreationStrategy> resourceCreationStrategies)
    {
        _resourceCreationStrategies = resourceCreationStrategies;
    }

    public TResource? Create<TResource>(ResourceMap map, IResourcePropertyValues values)
        where TResource : class
    {
        // Options are validated further back in the reading process to ensure that only a single strategy will be matched.
        // `First` is used rather than `Single` to avoid running every strategy handler on resource creation.
        var strategy = _resourceCreationStrategies.First(rcs => rcs.ApplicabilityHandler(map));
        return strategy.Create<TResource>(map, values);
    }
}
