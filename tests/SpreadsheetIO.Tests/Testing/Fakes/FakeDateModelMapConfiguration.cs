using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Tests.Testing.Fakes;

public class FakeDateModelMapConfiguration : IResourceMapConfiguration<FakeDateModel>
{
    public void Configure(IResourceMapBuilder<FakeDateModel> builder)
    {
        builder.Property(model => model.DateNumber)
            .UsesDateKind(CellDateKind.Number);

        builder.Property(model => model.DateText)
            .UsesDateKind(CellDateKind.Text);

        builder.ContinuesOnResourceReadingFailure()
            .UsesImplicitConstructor();
    }
}
