namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class FloatResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<float>
    {
        protected override bool TryParseValue(string cellValue, out float value)
            => float.TryParse(cellValue, out value);
    }
}
