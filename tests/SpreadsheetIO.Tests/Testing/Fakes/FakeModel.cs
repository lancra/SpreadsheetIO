namespace LanceC.SpreadsheetIO.Tests.Testing.Fakes;

public class FakeModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? DisplayName { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public char Letter { get; set; }
}
