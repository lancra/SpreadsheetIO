using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml.Spreadsheet;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents the kind of border line.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BorderLineKind : Enumeration
    {
        /// <summary>
        /// Specifies a blank border line.
        /// </summary>
        public static readonly BorderLineKind None = new BorderLineKind(0, "None", BorderStyleValues.None);

        /// <summary>
        /// Specifies a thin border line.
        /// </summary>
        public static readonly BorderLineKind Thin = new BorderLineKind(1, "Thin", BorderStyleValues.Thin);

        /// <summary>
        /// Specifies a thick border line.
        /// </summary>
        public static readonly BorderLineKind Thick = new BorderLineKind(2, "Thick", BorderStyleValues.Thick);

        /// <summary>
        /// Specifies a dashed border line.
        /// </summary>
        public static readonly BorderLineKind Dashed = new BorderLineKind(3, "Dashed", BorderStyleValues.Dashed);

        /// <summary>
        /// Specifies a dotted border line.
        /// </summary>
        public static readonly BorderLineKind Dotted = new BorderLineKind(4, "Dotted", BorderStyleValues.Dotted);

        /// <summary>
        /// Specifies a double border line.
        /// </summary>
        public static readonly BorderLineKind Double = new BorderLineKind(5, "Double", BorderStyleValues.Double);

        private BorderLineKind(int id, string name, BorderStyleValues openXmlValue)
            : base(id, name)
        {
            OpenXmlValue = openXmlValue;
        }

        internal BorderStyleValues OpenXmlValue { get; }
    }
}
