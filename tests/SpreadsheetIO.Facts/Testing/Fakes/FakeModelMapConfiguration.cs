using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

public class FakeModelMapConfiguration : IResourceMapConfiguration<FakeModel>
{
    public bool Configured { get; private set; }

    public void Configure(IResourceMapBuilder<FakeModel> builder)
    {
        Configured = true;
    }
}
