using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace LanceC.SpreadsheetIO.Tests.Testing;

[SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "This naming best represents the usage.")]
public record ExcelAttribute(ExcelAttributeKind Kind, XAttribute XAttribute)
{
    public ExcelAttributeKind Kind { get; init; } = Kind;

    public XAttribute XAttribute { get; init; } = XAttribute;
}
