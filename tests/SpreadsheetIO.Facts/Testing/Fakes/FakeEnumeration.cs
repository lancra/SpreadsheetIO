using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeEnumeration : Enumeration
    {
        public static readonly FakeEnumeration Foo = new FakeEnumeration(1, "foo");

        public static readonly FakeEnumeration Bar = new FakeEnumeration(2, "bar");

        public static readonly FakeEnumeration Baz = new FakeEnumeration(3, "baz");

        private FakeEnumeration(int id, string name)
            : base(id, name)
        {
        }
    }
}
