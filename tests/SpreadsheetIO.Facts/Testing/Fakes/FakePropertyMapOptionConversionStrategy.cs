using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

internal class FakePropertyMapOptionConversionStrategy : PropertyMapOptionConversionStrategy<FakePropertyMapOptionRegistration>
{
    public bool RanConversion { get; private set; }

    public override MapOptionConversionResult<IPropertyMapOption> ConvertToOption(
        FakePropertyMapOptionRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder)
    {
        RanConversion = true;
        return MapOptionConversionResult.Skipped<IPropertyMapOption>(registration);
    }
}
