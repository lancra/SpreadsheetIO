using System.Reflection;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Provides the builder for generating a property map key.
/// </summary>
internal class PropertyMapKeyBuilder : IInternalPropertyMapKeyBuilder
{
    public PropertyMapKeyBuilder(PropertyInfo propertyInfo)
    {
        Guard.Against.Null(propertyInfo, nameof(propertyInfo));

        Key = new(propertyInfo.Name, default, false);
    }

    public PropertyMapKey Key { get; private set; }

    public IPropertyMapKeyBuilder WithName(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        Key = new(name, Key.Number, Key.IsNameIgnored);
        return this;
    }

    public IPropertyMapKeyBuilder WithoutName()
    {
        Key = new(Key.Name, Key.Number, true);
        return this;
    }

    public IPropertyMapKeyBuilder WithNumber(uint number)
    {
        Guard.Against.Zero(number, nameof(number));

        Key = new(Key.Name, number, Key.IsNameIgnored);
        return this;
    }
}
