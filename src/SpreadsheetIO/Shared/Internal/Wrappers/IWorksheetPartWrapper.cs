namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    /// <summary>
    /// Defines a wrapper for a worksheet part.
    /// </summary>
    internal interface IWorksheetPartWrapper
    {
        /// <summary>
        /// Creates an Open XML writer for the worksheet part.
        /// </summary>
        /// <returns>The created Open XML writer.</returns>
        IOpenXmlWriterWrapper CreateWriter();
    }
}
