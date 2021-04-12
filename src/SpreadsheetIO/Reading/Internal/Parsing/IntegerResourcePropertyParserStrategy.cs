namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class IntegerResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<int>
    {
        protected override bool TryParseValue(string cellValue, out int value)
            => int.TryParse(cellValue, out value);
    }
}
