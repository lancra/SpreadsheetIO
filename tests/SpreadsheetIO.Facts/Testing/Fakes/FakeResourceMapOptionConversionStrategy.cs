using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

[SuppressMessage(
    "Performance",
    "CA1812:Avoid uninstantiated internal classes",
    Justification = "Only instantiated with dependency injection.")]
internal class FakeResourceMapOptionConversionStrategy : ResourceMapOptionConversionStrategy<FakeResourceMapOptionRegistration>
{
    public bool RanConversion { get; private set; }

    public override MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        FakeResourceMapOptionRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder)
    {
        RanConversion = true;
        return MapOptionConversionResult.Skipped<IResourceMapOption>(registration);
    }
}
