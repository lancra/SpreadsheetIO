using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Writing;
using Xunit.Abstractions;

namespace LanceC.SpreadsheetIO.Tests.Testing.Fixtures;

public class FileExcelFixture : ExcelFixtureBase, IDisposable
{
    private readonly ExcelAsserter _asserter;

    public FileExcelFixture(ITestOutputHelper output)
    {
        _asserter = new(output);
    }

    public Uri Path { get; } = new(System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{Guid.NewGuid()}.xlsx"));

    public IWritingSpreadsheet CreateSpreadsheet(Action<ICartographerBuilder>? mapOptions = default)
        => GetSpreadsheetFactory(mapOptions)
        .Create(Path);

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keeps reading and writing APIs consistent.")]
    public IReadingSpreadsheet OpenReadSpreadsheet(Action<ICartographerBuilder>? mapOptions = default)
        => GetSpreadsheetFactory(mapOptions)
        .OpenRead(GetSourcePath());

    public void EquivalentToSource() => _asserter.Equal(GetSourcePath(), Path);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            File.Delete(Path.LocalPath);
        }
    }

    private static Uri GetSourcePath()
    {
        var assembly = Assembly.GetExecutingAssembly()
            ?? throw new InvalidOperationException("Could not retrieve the executing assembly.");

        var directoryPath = System.IO.Path.GetDirectoryName(assembly.Location)
            ?? throw new InvalidOperationException("Could not retrieve the directory path from the executing assembly.");

        // Get source frame through [0]:GetSourcePath() -> [1]:<ExcelFixture Method> -> [2]:<Source Frame>.
        var sourceFrame = new StackTrace().GetFrame(2)
            ?? throw new InvalidOperationException("Could not retrieve the stack frame containing the test method.");

        var sourceMethod = sourceFrame.GetMethod()
            ?? throw new InvalidOperationException("Could not retrieve the test method from the stack frame.");

        var sourceFileAttribute = sourceMethod.GetCustomAttribute<ExcelSourceFileAttribute>()
            ?? throw new InvalidOperationException("Could not retrieve the source Excel file from the test method.");

        var sourcePath = new Uri(System.IO.Path.Combine(directoryPath, "Testing", "Content", sourceFileAttribute.FileName));
        return sourcePath;
    }
}
