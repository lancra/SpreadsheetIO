namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class DoubleResourcePropertyParserStrategy : SimpleResourcePropertyParserStrategy<double>
{
    protected override bool TryParseValue(string cellValue, out double value)
        => double.TryParse(cellValue, out value);
}
