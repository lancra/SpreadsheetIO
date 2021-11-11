using DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

/// <summary>
/// Defines a wrapper for an Open XML reader;
/// </summary>
internal interface IOpenXmlReaderWrapper : IDisposable
{
    /// <summary>
    /// Gets the value that determines whether the current element is a start element.
    /// </summary>
    bool IsStartElement { get; }

    /// <summary>
    /// Gets the value that determines whether the current element is an end element.
    /// </summary>
    bool IsEndElement { get; }

    /// <summary>
    /// Gets the current element type.
    /// </summary>
    Type ElementType { get; }

    /// <summary>
    /// Gets the attributes for the current element.
    /// </summary>
    IReadOnlyCollection<OpenXmlAttribute> Attributes { get; }

    /// <summary>
    /// Reads the next element.
    /// </summary>
    /// <returns><c>true</c> if there is another element to read; otherwise, <c>false</c>.</returns>
    bool Read();

    /// <summary>
    /// Gets the text in the current leaf element.
    /// </summary>
    /// <returns>The text of the current element if it is a leaf element. Otherwise, an empty string.</returns>
    string GetText();
}
