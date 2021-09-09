using System;
using System.Linq.Expressions;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps
{
    public class FakeModelMap : ResourceMap<FakeModel>
    {
        private readonly Action<ResourceMapOptionsBuilder<FakeModel>>? _optionsBuilderAction;

        public FakeModelMap(Action<ResourceMapOptionsBuilder<FakeModel>>? optionsBuilderAction = default)
        {
            _optionsBuilderAction = optionsBuilderAction;
        }

        public new FakeModelMap Map<TProperty>(Expression<Func<FakeModel, TProperty>> property)
            => (FakeModelMap)base.Map(property);

        public new FakeModelMap Map<TProperty>(
            Expression<Func<FakeModel, TProperty>> property,
            Action<PropertyMapKeyBuilder> keyAction)
            => (FakeModelMap)base.Map(property, keyAction);

        public new FakeModelMap Map<TProperty>(
            Expression<Func<FakeModel, TProperty>> property,
            Action<PropertyMapOptionsBuilder<FakeModel, TProperty>> optionsAction)
            => (FakeModelMap)base.Map(property, optionsAction);

        public new FakeModelMap Map<TProperty>(
            Expression<Func<FakeModel, TProperty>> property,
            Action<PropertyMapKeyBuilder> keyAction,
            Action<PropertyMapOptionsBuilder<FakeModel, TProperty>> optionsAction)
            => (FakeModelMap)base.Map(property, keyAction, optionsAction);

        protected override void Configure(ResourceMapOptionsBuilder<FakeModel> optionsBuilder)
        {
            _optionsBuilderAction?.Invoke(optionsBuilder);
            base.Configure(optionsBuilder);
        }
    }
}
