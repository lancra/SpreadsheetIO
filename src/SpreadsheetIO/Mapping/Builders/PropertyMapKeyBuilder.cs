using System.Reflection;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Mapping.Builders;

/// <summary>
/// Provides the builder for generating a property map key.
/// </summary>
internal class PropertyMapKeyBuilder : IInternalPropertyMapKeyBuilder
{
    public PropertyMapKeyBuilder(PropertyInfo propertyInfo)
    {
        Guard.Against.Null(propertyInfo, nameof(propertyInfo));

        Key = new(propertyInfo.Name, default, false, Array.Empty<string>());
    }

    public PropertyMapKey Key { get; private set; }

    public IPropertyMapKeyBuilder WithName(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        Key = new(name, Key.Number, Key.IsNameIgnored, Key.AlternateNames);
        return this;
    }

    public IPropertyMapKeyBuilder WithoutName()
    {
        Key = new(Key.Name, Key.Number, true, Key.AlternateNames);
        return this;
    }

    public IPropertyMapKeyBuilder WithNumber(uint number)
    {
        Guard.Against.Zero(number, nameof(number));

        Key = new(Key.Name, number, Key.IsNameIgnored, Key.AlternateNames);
        return this;
    }

    public IPropertyMapKeyBuilder WithAlternateNames(params string[] names)
    {
        Guard.Against.Null(names, nameof(names));
        Guard.Against.NullOrEmptyElements(names, nameof(names));

        Key = new(Key.Name, Key.Number, Key.IsNameIgnored, names);
        return this;
    }
}
