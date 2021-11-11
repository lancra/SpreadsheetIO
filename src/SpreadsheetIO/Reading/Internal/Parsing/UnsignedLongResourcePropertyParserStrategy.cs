namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class UnsignedLongResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<ulong>
{
    protected override bool TryParseValue(string cellValue, out ulong value)
        => ulong.TryParse(cellValue, out value);
}
