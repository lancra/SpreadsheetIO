using LanceC.SpreadsheetIO.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace LanceC.SpreadsheetIO.Tests.Testing.Fixtures;

public abstract class ExcelFixtureBase
{
    protected static ISpreadsheetFactory GetSpreadsheetFactory(Action<ICartographerBuilder>? mapOptions)
        => new ServiceCollection()
        .AddSpreadsheetIO(mapOptions)
        .BuildServiceProvider()
        .GetRequiredService<ISpreadsheetFactory>();
}
