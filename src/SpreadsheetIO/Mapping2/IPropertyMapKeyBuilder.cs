namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides the builder for generating a property map key.
/// </summary>
public interface IPropertyMapKeyBuilder
{
    /// <summary>
    /// Specifies the name to use for the property.
    /// </summary>
    /// <param name="name">The new property name.</param>
    /// <returns>The resulting property map key builder.</returns>
    IPropertyMapKeyBuilder WithName(string name);

    /// <summary>
    /// Specifies that no name will be used for reading the property.
    /// </summary>
    /// <returns>The resulting property map key builder.</returns>
    IPropertyMapKeyBuilder WithoutName();

    /// <summary>
    /// Specifies the column number to use for the property.
    /// </summary>
    /// <param name="number">The column number.</param>
    /// <returns>The resulting property map key builder.</returns>
    IPropertyMapKeyBuilder WithNumber(uint number);
}
