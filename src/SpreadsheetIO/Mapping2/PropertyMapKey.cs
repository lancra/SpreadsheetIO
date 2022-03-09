using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Represents an identifier for a resource property map.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record PropertyMapKey(string Name, uint? Number, bool IsNameIgnored)
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
    /// Gets the value that determines whether the name is ignored for reading.
    /// </summary>
    public bool IsNameIgnored { get; init; } = IsNameIgnored;

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
