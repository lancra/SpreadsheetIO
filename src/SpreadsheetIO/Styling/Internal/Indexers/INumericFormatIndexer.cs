using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers;

/// <summary>
/// Defines a numeric format indexer.
/// </summary>
internal interface INumericFormatIndexer : IIndexer<NumericFormat>
{
    /// <summary>
    /// Gets the count of indexed numeric formats that are not built-in to Excel.
    /// </summary>
    uint NonBuiltInCount { get; }
}
