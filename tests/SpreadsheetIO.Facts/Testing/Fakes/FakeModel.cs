using System;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeModel
    {
        [SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1401:Fields should be private",
            Justification = "Used to verify functionality in unit tests.")]
        public int Field;

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Display { get; set; }

        public decimal Decimal { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
