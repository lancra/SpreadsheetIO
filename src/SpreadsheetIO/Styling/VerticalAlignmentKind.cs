using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents the kind of vertical alignment for cell content.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VerticalAlignmentKind : SmartEnum<VerticalAlignmentKind>
    {
        /// <summary>
        /// Specifies a top vertical alignment.
        /// </summary>
        public static readonly VerticalAlignmentKind Top = new(1, "Top", OpenXml.VerticalAlignmentValues.Top);

        /// <summary>
        /// Specifies a center vertical alignment.
        /// </summary>
        public static readonly VerticalAlignmentKind Center = new(2, "Center", OpenXml.VerticalAlignmentValues.Center);

        /// <summary>
        /// Specifies a bottom vertical alignment.
        /// </summary>
        public static readonly VerticalAlignmentKind Bottom =
            new(3, "Bottom", OpenXml.VerticalAlignmentValues.Bottom, isDefault: true);

        /// <summary>
        /// Specifies a justify vertical alignment.
        /// </summary>
        public static readonly VerticalAlignmentKind Justify = new(4, "Justify", OpenXml.VerticalAlignmentValues.Justify);

        /// <summary>
        /// Specifies a distributed vertical alignment.
        /// </summary>
        public static readonly VerticalAlignmentKind Distributed = new(5, "Distributed", OpenXml.VerticalAlignmentValues.Distributed);

        private VerticalAlignmentKind(int id, string name, OpenXml.VerticalAlignmentValues openXmlValue, bool isDefault = false)
            : base(name, id)
        {
            OpenXmlValue = openXmlValue;
            IsDefault = isDefault;
        }

        /// <summary>
        /// Gets the value that determines whether this is the default vertical alignment kind.
        /// </summary>
        public bool IsDefault { get; }

        internal OpenXml.VerticalAlignmentValues OpenXmlValue { get; }
    }
}
