namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class UnsignedShortResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<ushort>
{
    protected override bool TryParseValue(string cellValue, out ushort value)
        => ushort.TryParse(cellValue, out value);
}
