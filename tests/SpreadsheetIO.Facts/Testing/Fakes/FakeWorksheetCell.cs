using LanceC.SpreadsheetIO.Shared.Internal;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

internal class FakeWorksheetCell
{
    public FakeWorksheetCell(CellLocation location, string value)
    {
        Location = location;
        Value = value;
    }

    public CellLocation Location { get; }

    public string Value { get; }
}
