using System.Xml.Linq;

namespace LanceC.SpreadsheetIO.Tests.Testing
{
    public record ExcelElement(ExcelElementKind Kind, XElement XElement)
    {
        public ExcelElementKind Kind { get; init; } = Kind;

        public XElement XElement { get; init; } = XElement;
    }
}
