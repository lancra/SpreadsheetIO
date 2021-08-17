using System;
using System.Diagnostics;
using System.Reflection;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Writing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace LanceC.SpreadsheetIO.Tests.Testing
{
    public class ExcelFixture : IDisposable
    {
        private readonly ExcelAsserter _asserter;

        public ExcelFixture(ITestOutputHelper output)
        {
            _asserter = new(output);
        }

        public Uri Path { get; } = new(System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{Guid.NewGuid()}.xlsx"));

        public IWritingSpreadsheet CreateSpreadsheet()
            => GetSpreadsheetFactory()
            .Create(Path);

        public IReadingSpreadsheet OpenReadSpreadsheet()
            => GetSpreadsheetFactory()
            .OpenRead(Path);

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
                System.IO.File.Delete(Path.LocalPath);
            }
        }

        private static ISpreadsheetFactory GetSpreadsheetFactory(Action<IServiceCollection>? additionalServices = default)
        {
            var services = new ServiceCollection()
                .AddSpreadsheetIO();

            additionalServices?.Invoke(services);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<ISpreadsheetFactory>();
        }

        private static Uri GetSourcePath()
        {
            var assembly = Assembly.GetExecutingAssembly()
                ?? throw new InvalidOperationException("Could not retrieve the executing assembly.");

            var directoryPath = System.IO.Path.GetDirectoryName(assembly.Location)
                ?? throw new InvalidOperationException("Could not retrieve the directory path from the executing assembly.");

            // Get source frame through [0]:GetSourcePath() -> [1]:EquivalentToSource() -> [2]:<Source Frame>.
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
}