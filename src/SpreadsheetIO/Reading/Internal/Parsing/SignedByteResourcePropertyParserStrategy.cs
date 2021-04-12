namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class SignedByteResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<sbyte>
    {
        protected override bool TryParseValue(string cellValue, out sbyte value)
            => sbyte.TryParse(cellValue, out value);
    }
}
