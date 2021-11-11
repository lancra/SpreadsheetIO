using System.Reflection;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Provides a builder for modifying a property map key.
/// </summary>
public class PropertyMapKeyBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyMapKeyBuilder"/> class.
    /// </summary>
    /// <param name="property">The underlying resource property.</param>
    public PropertyMapKeyBuilder(PropertyInfo property)
    {
        Guard.Against.Null(property, nameof(property));

        Key = new PropertyMapKey(property.Name, default, false);
    }

    /// <summary>
    /// Gets the current property map key.
    /// </summary>
    public PropertyMapKey Key { get; private set; }

    /// <summary>
    /// Overrides the underlying property name for the property map key.
    /// </summary>
    /// <param name="name">The name to use for the property map key.</param>
    /// <returns>The resulting property map key builder.</returns>
    public PropertyMapKeyBuilder OverrideName(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        Key = new PropertyMapKey(name, Key.Number, Key.IsNameIgnored);
        return this;
    }

    /// <summary>
    /// Ignores the name in the property map key for reading.
    /// </summary>
    /// <returns>The resulting property map key builder.</returns>
    public PropertyMapKeyBuilder IgnoreName()
    {
        Key = new PropertyMapKey(Key.Name, Key.Number, true);
        return this;
    }

    /// <summary>
    /// Sets the column number for the property map key.
    /// </summary>
    /// <param name="number">The column number to use for the property map key.</param>
    /// <returns>The resulting property map key builder.</returns>
    public PropertyMapKeyBuilder UseNumber(uint number)
    {
        Guard.Against.Zero(number, nameof(number));

        Key = new PropertyMapKey(Key.Name, number, Key.IsNameIgnored);
        return this;
    }
}
