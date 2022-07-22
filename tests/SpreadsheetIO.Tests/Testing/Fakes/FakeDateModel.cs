namespace LanceC.SpreadsheetIO.Tests.Testing.Fakes;

public class FakeDateModel
{
    public FakeDateModel(DateTime date)
    {
        DateNumber = date;
        DateText = date;
    }

    public FakeDateModel(DateTime dateNumber, DateTime dateText)
    {
        DateNumber = dateNumber;
        DateText = dateText;
    }

    public DateTime DateNumber { get; }

    public DateTime DateText { get; }
}
