using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Represents the kind of cell date.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CellDateKind : Enumeration
    {
        /// <summary>
        /// Specifies a date written as a number with a format required for readability.
        /// </summary>
        public static readonly CellDateKind Number = new CellDateKind(1, "Number");

        /// <summary>
        /// Specifies a date written as text where formatting cannot be modified.
        /// </summary>
        public static readonly CellDateKind Text = new CellDateKind(2, "Text");

        private CellDateKind(int id, string name)
            : base(id, name)
        {
        }
    }
}
