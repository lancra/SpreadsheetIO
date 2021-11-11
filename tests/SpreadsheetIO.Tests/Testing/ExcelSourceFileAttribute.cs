namespace LanceC.SpreadsheetIO.Tests.Testing;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ExcelSourceFileAttribute : Attribute
{
    public ExcelSourceFileAttribute(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; }
}
