using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

internal class FakeResourceMapOptionConversionStrategy : ResourceMapOptionConversionStrategy<FakeResourceMapOptionRegistration>
{
    public bool RanConversion { get; private set; }

    public override MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        FakeResourceMapOptionRegistration registration,
        ResourceMapBuilder resourceMapBuilder)
    {
        RanConversion = true;
        return MapOptionConversionResult.Skipped<IResourceMapOption>(registration);
    }
}
