using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Creation;

/// <summary>
/// Defines a strategy for instantiating new resources.
/// </summary>
internal interface IResourceCreationStrategy : IResourceCreator
{
    /// <summary>
    /// Gets the handler for determining whether this strategy is applicable to the provided resource map options.
    /// </summary>
    Func<ResourceMapOptions, bool> ApplicabilityHandler { get; }
}
