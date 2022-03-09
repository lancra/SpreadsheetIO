using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

public class FakeModelMapConfiguration : IResourceMapConfiguration<FakeModel>
{
    public void Configure(ResourceMapBuilder<FakeModel> builder)
    {
        builder.ExitsOnResourceReadingFailure()
            .UsesHeaderStyle(BuiltInExcelStyle.Good);

        builder.Property(model => model.Id);
        builder.Property(model => model.Name)
            .UsesBodyStyle(BuiltInExcelStyle.Bad);
    }
}
