using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

/// <summary>
/// Defines the factory for generating element readers.
/// </summary>
internal interface IElementReaderFactory
{
    /// <summary>
    /// Creates an element reader for a shared string table.
    /// </summary>
    /// <param name="sharedStringTablePart">The Open XML shared string table part.</param>
    /// <returns>The shared string table element reader.</returns>
    ISharedStringTableElementReader CreateSharedStringTableReader(ISharedStringTablePartWrapper sharedStringTablePart);

    /// <summary>
    /// Creates an element reader for a worksheet.
    /// </summary>
    /// <param name="worksheetPart">The Open XML worksheet part.</param>
    /// <returns>The worksheet element reader.</returns>
    IWorksheetElementReader CreateWorksheetReader(IWorksheetPartWrapper worksheetPart);
}
