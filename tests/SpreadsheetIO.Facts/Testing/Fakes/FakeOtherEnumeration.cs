using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeOtherEnumeration : Enumeration
    {
        public static readonly FakeOtherEnumeration Foo = new FakeOtherEnumeration(1, "foo");

        private FakeOtherEnumeration(int id, string name)
            : base(id, name)
        {
        }
    }
}
