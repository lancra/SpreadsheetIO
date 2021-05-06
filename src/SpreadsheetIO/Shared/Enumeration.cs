using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Shared
{
    /// <summary>
    /// Provides support for derived enumeration classes.
    /// </summary>
    [SuppressMessage(
        "Naming",
        "CA1724:Type names should not match namespaces",
        Justification = "The name is the most appropriate for the class and its usage.")]
    public abstract class Enumeration : IComparable<Enumeration>, IEquatable<Enumeration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        /// <param name="id">The enumeration identifier.</param>
        /// <param name="name">The enumeration name.</param>
        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Gets the enumeration identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the enumeration name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Provides an equality comparison between enumerations.
        /// </summary>
        /// <param name="left">The left enumeration to compare.</param>
        /// <param name="right">The right enumeration to compare.</param>
        /// <returns><c>true</c> if the enumerations are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Enumeration? left, Enumeration? right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Provides an inequality comparison between enumerations.
        /// </summary>
        /// <param name="left">The left enumeration to compare.</param>
        /// <param name="right">The right enumeration to compare.</param>
        /// <returns><c>true</c> if the enumerations are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Enumeration? left, Enumeration? right)
            => !(left == right);

        /// <summary>
        /// Provides a less than comparison between enumerations.
        /// </summary>
        /// <param name="left">The left enumeration to compare.</param>
        /// <param name="right">The right enumeration to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Enumeration? left, Enumeration? right)
            => left is null ? right is object : left.CompareTo(right) < 0;

        /// <summary>
        /// Provides a less than or equal to comparison between enumerations.
        /// </summary>
        /// <param name="left">The left enumeration to compare.</param>
        /// <param name="right">The right enumeration to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator <=(Enumeration? left, Enumeration? right)
            => left is null || left.CompareTo(right) <= 0;

        /// <summary>
        /// Provides a greater than comparison between enumerations.
        /// </summary>
        /// <param name="left">The left enumeration to compare.</param>
        /// <param name="right">The right enumeration to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator >(Enumeration? left, Enumeration? right)
            => left is object && left.CompareTo(right) > 0;

        /// <summary>
        /// Provides a greater than or equal to comparison between enumerations.
        /// </summary>
        /// <param name="left">The left enumeration to compare.</param>
        /// <param name="right">The right enumeration to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator >=(Enumeration? left, Enumeration? right)
            => left is null ? right is null : left.CompareTo(right) >= 0;

        /// <summary>
        /// Parses an enumeration from an identifier.
        /// </summary>
        /// <typeparam name="TEnumeration">The type of enumeration to parse.</typeparam>
        /// <param name="id">The enumeration identifier.</param>
        /// <returns>The parsed enumeration.</returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the <paramref name="id"/> is not a valid <typeparamref name="TEnumeration"/> identifier.
        /// </exception>
        public static TEnumeration FromId<TEnumeration>(int id)
            where TEnumeration : Enumeration
        {
            var matchingItem = Parse<TEnumeration, int>(
                id,
                "Id",
                item => item.Id == id);
            return matchingItem;
        }

        /// <summary>
        /// Parses an enumeration from a name.
        /// </summary>
        /// <typeparam name="TEnumeration">The type of enumeration to parse.</typeparam>
        /// <param name="name">The enumeration name.</param>
        /// <returns>The parsed enumeration.</returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the <paramref name="name"/> is not a valid <typeparamref name="TEnumeration"/> name.
        /// </exception>
        public static TEnumeration FromName<TEnumeration>(string name)
            where TEnumeration : Enumeration
        {
            var matchingItem = Parse<TEnumeration, string>(
                name,
                "Name",
                item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return matchingItem;
        }

        /// <summary>
        /// Attempts to parse an enumeration from an identifier.
        /// </summary>
        /// <typeparam name="TEnumeration">The type of enumeration to parse.</typeparam>
        /// <param name="id">The enumeration identifier.</param>
        /// <param name="enumeration">The parsed enumeration.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="id"/> is a valid <typeparamref name="TEnumeration"/> identifier; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool TryFromId<TEnumeration>(int id, out TEnumeration? enumeration)
            where TEnumeration : Enumeration
        {
            var success = TryParse<TEnumeration, int>(item => item.Id == id, out enumeration);
            return success;
        }

        /// <summary>
        /// Attempts to parse an enumeration from a name.
        /// </summary>
        /// <typeparam name="TEnumeration">The type of enumeration to parse.</typeparam>
        /// <param name="name">The enumeration name.</param>
        /// <param name="enumeration">The parsed enumeration.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="name"/> is a valid <typeparamref name="TEnumeration"/> name; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryFromName<TEnumeration>(string name, out TEnumeration? enumeration)
            where TEnumeration : Enumeration
        {
            var success = TryParse<TEnumeration, string>(
                item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase),
                out enumeration);
            return success;
        }

        /// <summary>
        /// Gets all enumerations.
        /// </summary>
        /// <typeparam name="TEnumeration">The type of enumeration to retrieve.</typeparam>
        /// <returns>The collection of enumerations.</returns>
        public static IReadOnlyCollection<TEnumeration> GetAll<TEnumeration>()
            where TEnumeration : Enumeration
            => GetAllInternal(typeof(TEnumeration))
            .Cast<TEnumeration>()
            .ToArray();

        /// <summary>
        /// Gets all enumerations.
        /// </summary>
        /// <param name="enumerationType">The type of enumeration to retrieve.</param>
        /// <returns>The collection of enumerations.</returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when <paramref name="enumerationType"/> is not a valid enumeration type.
        /// </exception>
        public static IReadOnlyCollection<Enumeration> GetAll(Type enumerationType)
            => GetAllInternal(enumerationType)
            .Cast<Enumeration>()
            .ToArray();

        /// <inheritdoc/>
        public int CompareTo(Enumeration? other)
        {
            if (other is null)
            {
                return 1;
            }

            return Id.CompareTo(other.Id);
        }

        /// <inheritdoc/>
        public bool Equals(Enumeration? other)
        {
            if (other is null)
            {
                return false;
            }

            var typeMatches = GetType() == other.GetType();
            var valueMatches = Id == other.Id;

            return typeMatches && valueMatches;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
            => Equals(obj as Enumeration);

        /// <inheritdoc/>
        public override int GetHashCode()
            => Id.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => Name;

        private static IReadOnlyCollection<object> GetAllInternal(Type enumerationType)
        {
            Guard.Against.Null(enumerationType, nameof(enumerationType));

            if (!typeof(Enumeration).IsAssignableFrom(enumerationType))
            {
                throw new NotSupportedException(Messages.UnsupportedEnumerationType(enumerationType.Name));
            }

            var fields = enumerationType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            return fields.Select(field => field.GetValue(null))
                .Where(value => value is not null)
                .Cast<object>()
                .ToArray();
        }

        private static TEnumeration Parse<TEnumeration, TValue>(
            TValue value,
            string description,
            Func<TEnumeration, bool> predicate)
            where TEnumeration : Enumeration
        {
            var matchingItem = GetAll<TEnumeration>()
                .FirstOrDefault(predicate);

            if (matchingItem is null)
            {
                throw new NotSupportedException(Messages.InvalidEnumerationValue(value, description, typeof(TEnumeration).Name));
            }

            return matchingItem;
        }

        private static bool TryParse<TEnumeration, TValue>(Func<TEnumeration, bool> predicate, out TEnumeration? enumeration)
            where TEnumeration : Enumeration
        {
            enumeration = GetAll<TEnumeration>()
                .FirstOrDefault(predicate);

            var success = !(enumeration is null);
            return success;
        }
    }
}
