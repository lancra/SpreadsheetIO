using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml.Spreadsheet;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents the kind of fill.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FillKind : Enumeration
    {
        /// <summary>
        /// Specifies a blank fill.
        /// </summary>
        public static readonly FillKind None = new FillKind(0, "None", PatternValues.None);

        /// <summary>
        /// Specifies a solid fill.
        /// </summary>
        public static readonly FillKind Solid = new FillKind(1, "Solid", PatternValues.Solid);

        internal static readonly FillKind Gray125 = new FillKind(-1, "Gray125", PatternValues.Gray125);

        private FillKind(int id, string name, PatternValues openXmlValue)
            : base(id, name)
        {
            OpenXmlValue = openXmlValue;
        }

        internal PatternValues OpenXmlValue { get; }
    }
}
