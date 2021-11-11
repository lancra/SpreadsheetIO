using DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

public class FakeOpenXmlElement
{
    private FakeOpenXmlElement(
        bool isStartElement,
        bool isEndElement,
        Type elementType,
        string text,
        params OpenXmlAttribute[] attributes)
    {
        IsStartElement = isStartElement;
        IsEndElement = isEndElement;
        ElementType = elementType;
        Text = text;
        Attributes = attributes;
    }

    public bool IsStartElement { get; }

    public bool IsEndElement { get; }

    public Type ElementType { get; }

    public string Text { get; }

    public IReadOnlyCollection<OpenXmlAttribute> Attributes { get; }

    public static FakeOpenXmlElement Create(Type elementType, string text = "", params OpenXmlAttribute[] attributes)
        => new(true, true, elementType, text, attributes);

    public static FakeOpenXmlElement CreateStart(Type elementType, string text = "", params OpenXmlAttribute[] attributes)
        => new(true, false, elementType, text, attributes);

    public static FakeOpenXmlElement CreateEnd(Type elementType, string text = "", params OpenXmlAttribute[] attributes)
        => new(false, true, elementType, text, attributes);
}
