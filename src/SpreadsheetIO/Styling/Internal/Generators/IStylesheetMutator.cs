using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    /// <summary>
    /// Defines a mutator used to modify a stylesheet.
    /// </summary>
    internal interface IStylesheetMutator
    {
        /// <summary>
        /// Mutates a stylesheet.
        /// </summary>
        /// <param name="stylesheet">The stylesheet to modify.</param>
        void Mutate(Stylesheet stylesheet);
    }
}
