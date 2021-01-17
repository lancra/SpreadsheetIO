using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Defines a cell builder with a set value.
    /// </summary>
    public interface ICellValueBuilder : ICellBuilder
    {
        /// <summary>
        /// Modifies the cell builder to specify a style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The modified cell builder.</returns>
        ICellValueBuilder WithStyle(Style style);
    }
}
