namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class DecimalResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<decimal>
{
    protected override bool TryParseValue(string cellValue, out decimal value)
        => decimal.TryParse(cellValue, out value);
}
