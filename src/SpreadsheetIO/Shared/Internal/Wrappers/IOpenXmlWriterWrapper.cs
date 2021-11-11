using DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

/// <summary>
/// Defines a wrapper for an Open XML writer.
/// </summary>
internal interface IOpenXmlWriterWrapper : IDisposable
{
    /// <summary>
    /// Writes an Open XML element.
    /// </summary>
    /// <param name="element">The element to write.</param>
    /// <returns>The Open XML writer wrapper.</returns>
    IOpenXmlWriterWrapper WriteElement(OpenXmlElement element);

    /// <summary>
    /// Writes the start tag for an Open XML element.
    /// </summary>
    /// <param name="element">The element to write.</param>
    /// <returns>The Open XML writer wrapper.</returns>
    IOpenXmlWriterWrapper WriteStartElement(OpenXmlElement element);

    /// <summary>
    /// Writes the end tag for an Open XML element.
    /// </summary>
    /// <returns>The Open XML writer wrapper.</returns>
    IOpenXmlWriterWrapper WriteEndElement();
}
