using System;
using System.Linq.Expressions;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeModelMap : ResourceMap<FakeModel>
    {
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
    }
}
