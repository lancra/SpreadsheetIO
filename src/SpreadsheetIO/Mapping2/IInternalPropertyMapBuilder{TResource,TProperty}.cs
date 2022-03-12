namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Defines the internal builder for generating a property map.
/// </summary>
/// <typeparam name="TResource">The type of resource the property is defined in.</typeparam>
/// <typeparam name="TProperty">The type of property to map.</typeparam>
internal interface IInternalPropertyMapBuilder<TResource, TProperty> : IPropertyMapBuilder<TResource, TProperty>,
    IInternalPropertyMapBuilder
    where TResource : class
{
}
