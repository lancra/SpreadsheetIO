using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

internal static class OpenXmlReaderWrapperExtensions
{
    public static bool ReadNextElement(this IOpenXmlReaderWrapper reader, Type startElementType, Type endElementType)
    {
        Guard.Against.Null(reader, nameof(reader));
        Guard.Against.Null(startElementType, nameof(startElementType));
        Guard.Against.Null(endElementType, nameof(endElementType));

        while (reader.Read())
        {
            if (reader.IsStartElementOfType(startElementType))
            {
                return true;
            }

            if (reader.ElementType == endElementType && reader.IsEndElement)
            {
                return false;
            }
        }

        return false;
    }

    public static DocumentFormat.OpenXml.OpenXmlAttribute GetAttribute(
        this IOpenXmlReaderWrapper reader,
        ElementAttributeKind kind)
    {
        Guard.Against.Null(reader, nameof(reader));
        Guard.Against.Null(kind, nameof(kind));

        return reader.Attributes
            .SingleOrDefault(attribute => attribute.LocalName.Equals(kind.LocalName, StringComparison.OrdinalIgnoreCase));
    }

    public static bool IsStartElementOfType(this IOpenXmlReaderWrapper reader, Type elementType)
    {
        Guard.Against.Null(reader, nameof(reader));
        Guard.Against.Null(elementType, nameof(elementType));

        return reader.ElementType == elementType &&
            reader.IsStartElement;
    }
}
