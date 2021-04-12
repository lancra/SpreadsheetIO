namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class ShortResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<short>
    {
        protected override bool TryParseValue(string cellValue, out short value)
            => short.TryParse(cellValue, out value);
    }
}
