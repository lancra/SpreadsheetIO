using System;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Represents an identifier for a resource property map.
    /// </summary>
    public class PropertyMapKey : IEquatable<PropertyMapKey>
    {
        internal PropertyMapKey(string? name, uint? number, bool isNameIgnored)
        {
            Name = name;
            Number = number;
            IsNameIgnored = isNameIgnored;
        }

        /// <summary>
        /// Gets the name for the property.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the column number for the property.
        /// </summary>
        public uint? Number { get; }

        /// <summary>
        /// Gets the value that determines whether the name is ignored for reading.
        /// </summary>
        public bool IsNameIgnored { get; }

        /// <summary>
        /// Compares two property map keys on equality.
        /// </summary>
        /// <param name="firstKey">The first property map key to compare.</param>
        /// <param name="secondKey">The second property map key to compare.</param>
        /// <returns><c>true</c> if the property map keys are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PropertyMapKey firstKey, PropertyMapKey secondKey)
        {
            if (ReferenceEquals(firstKey, secondKey))
            {
                return true;
            }

            if (firstKey is null || secondKey is null)
            {
                return false;
            }

            return firstKey.Equals(secondKey);
        }

        /// <summary>
        /// Compares two property map keys on inequality.
        /// </summary>
        /// <param name="firstKey">The first property map key to compare.</param>
        /// <param name="secondKey">The second property map key to compare.</param>
        /// <returns><c>true</c> if the property map keys are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PropertyMapKey firstKey, PropertyMapKey secondKey)
            => !(firstKey == secondKey);

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
        /// Compares another object for equality.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><c>true</c> if the object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
            => Equals(obj as PropertyMapKey);

        /// <summary>
        /// Gets the hash code for the property map key.
        /// </summary>
        /// <returns>The property map key hash code.</returns>
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => HashCode.Combine(Name, Number);
    }
}
