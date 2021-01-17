namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Defines a cell builder with a set string value.
    /// </summary>
    public interface ICellStringValueBuilder : ICellValueBuilder
    {
        /// <summary>
        /// Modifies the cell builder to change the string writing methodology.
        /// </summary>
        /// <param name="kind">The cell string kind.</param>
        /// <returns>The modified cell builder.</returns>
        ICellStringValueBuilder WrittenAs(CellStringKind kind);
    }
}
