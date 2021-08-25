using System.Xml.Linq;

namespace LanceC.SpreadsheetIO.Tests.Testing
{
    public record ExcelAttribute(ExcelAttributeKind Kind, XAttribute XAttribute)
    {
        public ExcelAttributeKind Kind { get; init; } = Kind;

        public XAttribute XAttribute { get; init; } = XAttribute;
    }
}
