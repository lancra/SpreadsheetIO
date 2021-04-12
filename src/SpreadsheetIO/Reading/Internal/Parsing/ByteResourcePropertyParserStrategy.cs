namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class ByteResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<byte>
    {
        protected override bool TryParseValue(string cellValue, out byte value)
            => byte.TryParse(cellValue, out value);
    }
}
