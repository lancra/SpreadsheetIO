using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Tests.Testing.Fakes;

public class FakeModelMapConfiguration : IResourceMapConfiguration<FakeModel>
{
    public void Configure(IResourceMapBuilder<FakeModel> builder)
    {
        builder.Property(model => model.Id);
        builder.Property(model => model.Name);
        builder.Property(model => model.DisplayName)
            .IsOptional(PropertyElementKind.Body);
        builder.Property(model => model.Date);
        builder.Property(model => model.Amount);
        builder.Property(model => model.Letter);
    }
}
