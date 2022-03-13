namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Defines the internal builder for generating a resource map.
/// </summary>
/// <typeparam name="TResource">The type of resource to map.</typeparam>
internal interface IInternalResourceMapBuilder<TResource> : IResourceMapBuilder<TResource>, IInternalResourceMapBuilder
    where TResource : class
{
}
