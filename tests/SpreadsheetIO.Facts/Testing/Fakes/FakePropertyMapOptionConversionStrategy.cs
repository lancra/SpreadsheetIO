using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

[SuppressMessage(
    "Performance",
    "CA1812:Avoid uninstantiated internal classes",
    Justification = "Only instantiated with dependency injection.")]
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
