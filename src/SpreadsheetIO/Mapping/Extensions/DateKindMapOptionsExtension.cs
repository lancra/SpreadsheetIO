using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class DateKindMapOptionsExtension : IResourceMapOptionsExtension, IPropertyMapOptionsExtension
    {
        public DateKindMapOptionsExtension(CellDateKind dateKind)
        {
            DateKind = dateKind;
        }

        public IReadOnlyCollection<Type> AllowedTypes
            => new[]
            {
                typeof(DateTime),
                typeof(DateTimeOffset),
            };

        public CellDateKind DateKind { get; }
    }
}
