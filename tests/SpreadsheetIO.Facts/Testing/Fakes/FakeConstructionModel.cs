namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeConstructionModel
    {
        public FakeConstructionModel()
        {
        }

        public FakeConstructionModel(int id, string? name, decimal amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }

        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Amount { get; set; }
    }
}
