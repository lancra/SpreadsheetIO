using System.Linq.Expressions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;

public class FakeConstructionModelMap : ResourceMap<FakeConstructionModel>
{
    private readonly Action<ResourceMapOptionsBuilder<FakeConstructionModel>>? _optionsBuilderAction;

    public FakeConstructionModelMap(Action<ResourceMapOptionsBuilder<FakeConstructionModel>>? optionsBuilderAction = default)
    {
        _optionsBuilderAction = optionsBuilderAction;
    }

    public new FakeConstructionModelMap Map<TProperty>(Expression<Func<FakeConstructionModel, TProperty>> property)
        => (FakeConstructionModelMap)base.Map(property);

    protected override void Configure(ResourceMapOptionsBuilder<FakeConstructionModel> optionsBuilder)
    {
        _optionsBuilderAction?.Invoke(optionsBuilder);
        base.Configure(optionsBuilder);
    }
}
