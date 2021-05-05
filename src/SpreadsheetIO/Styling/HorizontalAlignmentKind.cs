using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents the kind of horizontal alignment for cell content.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HorizontalAlignmentKind : Enumeration
    {
        /// <summary>
        /// Specifies a general horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind General =
            new(0, "General", OpenXml.HorizontalAlignmentValues.General, isDefault: true);

        /// <summary>
        /// Specifies a left horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind Left = new(1, "Left", OpenXml.HorizontalAlignmentValues.Left);

        /// <summary>
        /// Specifies a center horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind Center = new(2, "Center", OpenXml.HorizontalAlignmentValues.Center);

        /// <summary>
        /// Specifies a right horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind Right = new(3, "Right", OpenXml.HorizontalAlignmentValues.Right);

        /// <summary>
        /// Specifies a fill horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind Fill = new(4, "Fill", OpenXml.HorizontalAlignmentValues.Fill);

        /// <summary>
        /// Specifies a justify horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind Justify = new(5, "Justify", OpenXml.HorizontalAlignmentValues.Justify);

        /// <summary>
        /// Specifies a continuous center horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind CenterContinuous =
            new(6, "Center Continuous", OpenXml.HorizontalAlignmentValues.CenterContinuous);

        /// <summary>
        /// Specifies a distributed horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind Distributed =
            new(7, "Distributed", OpenXml.HorizontalAlignmentValues.Distributed);

        /// <summary>
        /// Specifies a distributed justify horizontal alignment.
        /// </summary>
        public static readonly HorizontalAlignmentKind JustifyDistributed =
            new(8, "Justify Distributed", OpenXml.HorizontalAlignmentValues.Distributed);

        private HorizontalAlignmentKind(int id, string name, OpenXml.HorizontalAlignmentValues openXmlValue, bool isDefault = false)
            : base(id, name)
        {
            OpenXmlValue = openXmlValue;
            IsDefault = isDefault;
        }

        /// <summary>
        /// Gets the value that determines whether this is the default horizontal alignment kind.
        /// </summary>
        public bool IsDefault { get; }

        internal OpenXml.HorizontalAlignmentValues OpenXmlValue { get; }
    }
}
