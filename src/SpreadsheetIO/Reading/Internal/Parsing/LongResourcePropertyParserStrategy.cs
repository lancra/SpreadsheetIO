namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class LongResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<long>
{
    protected override bool TryParseValue(string cellValue, out long value)
        => long.TryParse(cellValue, out value);
}
