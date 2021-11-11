using System.Linq.Expressions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps
{
    public class FakePropertySetterModelMap : ResourceMap<FakePropertySetterModel>
    {
        private readonly Action<ResourceMapOptionsBuilder<FakePropertySetterModel>>? _optionsBuilderAction;

        public FakePropertySetterModelMap(Action<ResourceMapOptionsBuilder<FakePropertySetterModel>>? optionsBuilderAction = default)
        {
            _optionsBuilderAction = optionsBuilderAction;
        }

        public new FakePropertySetterModelMap Map<TProperty>(Expression<Func<FakePropertySetterModel, TProperty>> property)
            => (FakePropertySetterModelMap)base.Map(property);

        protected override void Configure(ResourceMapOptionsBuilder<FakePropertySetterModel> optionsBuilder)
        {
            _optionsBuilderAction?.Invoke(optionsBuilder);
            base.Configure(optionsBuilder);
        }
    }
}
