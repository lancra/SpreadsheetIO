namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Provides the internal builder for generating a property map key.
/// </summary>
internal interface IInternalPropertyMapKeyBuilder : IPropertyMapKeyBuilder
{
    /// <summary>
    /// Gets the property map key.
    /// </summary>
    PropertyMapKey Key { get; }
}
