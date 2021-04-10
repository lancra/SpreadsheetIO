using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeDateTimePropertyMapOptionsExtension : IPropertyMapOptionsExtension
    {
        public IReadOnlyCollection<Type> AllowedTypes { get; } = new[] { typeof(DateTime), };
    }
}
