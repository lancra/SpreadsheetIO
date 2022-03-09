using System.Reflection;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides the builder for generating a property map key.
/// </summary>
public class PropertyMapKeyBuilder
{
    internal PropertyMapKeyBuilder(PropertyInfo propertyInfo)
    {
        Guard.Against.Null(propertyInfo, nameof(propertyInfo));

        Key = new(propertyInfo.Name, default, false);
    }

    internal PropertyMapKey Key { get; private set; }

    /// <summary>
    /// Specifies the name to use for the property.
    /// </summary>
    /// <param name="name">The new property name.</param>
    /// <returns>The resulting property map key builder.</returns>
    public PropertyMapKeyBuilder WithName(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        Key = new(name, Key.Number, Key.IsNameIgnored);
        return this;
    }

    /// <summary>
    /// Specifies that no name will be used for reading the property.
    /// </summary>
    /// <returns>The resulting property map key builder.</returns>
    public PropertyMapKeyBuilder WithoutName()
    {
        Key = new(Key.Name, Key.Number, true);
        return this;
    }

    /// <summary>
    /// Specifies the column number to use for the property.
    /// </summary>
    /// <param name="number">The column number.</param>
    /// <returns>The resulting property map key builder.</returns>
    public PropertyMapKeyBuilder WithNumber(uint number)
    {
        Guard.Against.Zero(number, nameof(number));

        Key = new(Key.Name, number, Key.IsNameIgnored);
        return this;
    }
}
