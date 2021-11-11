namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class UnsignedIntegerResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<uint>
{
    protected override bool TryParseValue(string cellValue, out uint value)
        => uint.TryParse(cellValue, out value);
}
