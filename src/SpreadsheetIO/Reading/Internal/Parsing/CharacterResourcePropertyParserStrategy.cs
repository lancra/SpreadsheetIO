namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class CharacterResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<char>
    {
        protected override bool TryParseValue(string cellValue, out char value)
            => char.TryParse(cellValue, out value);
    }
}
