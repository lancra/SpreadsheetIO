namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;

public class FakeConstructionModel
{
    public FakeConstructionModel()
    {
    }

    public FakeConstructionModel(int id, string? name, decimal amount = 10M)
    {
        Id = id;
        Name = name;
        Amount = amount;
    }

    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Amount { get; set; }
}
