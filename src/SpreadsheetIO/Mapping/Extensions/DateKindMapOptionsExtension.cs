using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    /// <summary>
    /// Provides a date format for a resource or a property.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DateKindMapOptionsExtension : IResourceMapOptionsExtension, IPropertyMapOptionsExtension
    {
        internal DateKindMapOptionsExtension(CellDateKind dateKind)
        {
            DateKind = dateKind;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> AllowedTypes
            => new[]
            {
                typeof(DateTime),
                typeof(DateTimeOffset),
            };

        /// <summary>
        /// Gets the date format.
        /// </summary>
        public CellDateKind DateKind { get; }
    }
}
