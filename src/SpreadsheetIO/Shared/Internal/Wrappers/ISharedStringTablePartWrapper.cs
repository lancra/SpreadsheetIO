namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    /// <summary>
    /// Defines a wrapper for a shared string table part.
    /// </summary>
    internal interface ISharedStringTablePartWrapper
    {
        /// <summary>
        /// Creates an Open XML writer for the shared string table part.
        /// </summary>
        /// <returns>The created Open XML writer.</returns>
        IOpenXmlWriterWrapper CreateWriter();
    }
}
