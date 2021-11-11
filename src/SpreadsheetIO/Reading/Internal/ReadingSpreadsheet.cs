namespace LanceC.SpreadsheetIO.Reading.Internal
{
    internal class ReadingSpreadsheet : IReadingSpreadsheet
    {
        public ReadingSpreadsheet(IReadingSpreadsheetPageCollection pages)
        {
            Pages = pages;
        }

        public IReadingSpreadsheetPageCollection Pages { get; }
    }
}
