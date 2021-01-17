using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml.Spreadsheet;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Represents the kind of cell string.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CellStringKind : Enumeration
    {
        /// <summary>
        /// Specifies a string shared between cells in a spreadsheet.
        /// </summary>
        public static readonly CellStringKind SharedString = new CellStringKind(1, "Shared String", CellValues.SharedString);

        /// <summary>
        /// Specifies a string written directly to a cell.
        /// </summary>
        public static readonly CellStringKind InlineString = new CellStringKind(2, "Inline String", CellValues.InlineString);

        private CellStringKind(int id, string name, CellValues openXmlValue)
            : base(id, name)
        {
            OpenXmlValue = openXmlValue;
        }

        internal CellValues OpenXmlValue { get; }
    }
}
