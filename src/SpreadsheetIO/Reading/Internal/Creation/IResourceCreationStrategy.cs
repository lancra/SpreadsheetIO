using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

/// <summary>
/// Defines a strategy for instantiating new resources.
/// </summary>
internal interface IResourceCreationStrategy : IResourceCreator
{
    /// <summary>
    /// Gets the handler for determining whether this strategy is applicable to the provided resource map.
    /// </summary>
    Func<ResourceMap, bool> ApplicabilityHandler { get; }
}
