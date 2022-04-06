using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Represents an identifier for a resource property map.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record PropertyMapKey(string Name, uint? Number, bool IsNameIgnored, IReadOnlyCollection<string> AlternateNames)
{
    /// <summary>
    /// Gets the name for the property.
    /// </summary>
    public string Name { get; init; } = Name;

    /// <summary>
    /// Gets the column number for the property.
    /// </summary>
    public uint? Number { get; init; } = Number;

    /// <summary>
    /// Gets a value indicating whether the name is ignored for reading.
    /// </summary>
    public bool IsNameIgnored { get; init; } = IsNameIgnored;

    /// <summary>
    /// Gets the altername names that can be used when reading the property.
    /// </summary>
    public IReadOnlyCollection<string> AlternateNames { get; init; } = AlternateNames;

    /// <summary>
    /// Compares another property map key for equality.
    /// </summary>
    /// <param name="other">The other property map key to compare.</param>
    /// <returns><c>true</c> if the property map key is equal to this instance; otherwise, <c>false</c>.</returns>
    public bool Equals(PropertyMapKey? other)
        => other is not null &&
        Name == other.Name &&
        Number == other.Number;

    /// <summary>
    /// Gets the hash code for the property map key.
    /// </summary>
    /// <returns>The property map key hash code.</returns>
    public override int GetHashCode()
        => HashCode.Combine(Name, Number);
}
