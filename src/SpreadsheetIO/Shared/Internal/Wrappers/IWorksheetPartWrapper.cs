namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

/// <summary>
/// Defines a wrapper for a worksheet part.
/// </summary>
internal interface IWorksheetPartWrapper
{
    /// <summary>
    /// Gets the worksheet name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Creates an Open XML reader for the worksheet part.
    /// </summary>
    /// <returns>The created Open XML reader.</returns>
    IOpenXmlReaderWrapper CreateReader();

    /// <summary>
    /// Creates an Open XML writer for the worksheet part.
    /// </summary>
    /// <returns>The created Open XML writer.</returns>
    IOpenXmlWriterWrapper CreateWriter();
}
