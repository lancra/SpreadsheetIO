using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Tests.Testing.Fakes
{
    public class FakeModelMap : ResourceMap<FakeModel>
    {
        public FakeModelMap()
        {
            Map(model => model.Id);
            Map(model => model.Name);
            Map(model => model.DisplayName, options => options.MarkAsOptional(PropertyElementKind.Body));
            Map(model => model.Date);
            Map(model => model.Amount);
            Map(model => model.Letter);
        }
    }
}
