using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Tests.Testing.Fakes;
using LanceC.SpreadsheetIO.Tests.Testing.Fixtures;
using Xunit;

namespace LanceC.SpreadsheetIO.Tests;

public class MappingPropertyOptionsTests : IDisposable
{
    private readonly InMemoryExcelFixture _excelFixture = new();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _excelFixture.Dispose();
        }
    }

    public class TheDefaultValuePropertyMapOption : MappingPropertyOptionsTests
    {
        [Fact]
        public void ReadsDefaultValueForAllFailedParseResults()
        {
            // Arrange
            var expectedModel = new FakeModel
            {
                Id = 42,
                Name = "name",
                DisplayName = "displayName",
            };

            _excelFixture.ArrangeSpreadsheet(
                spreadsheet => spreadsheet.AddPage("Sheet1")
                .AddCell("Id").AddCell("Name").AddCell("DisplayName").AdvanceRow()
                .AddCell("Foo").AdvanceRow());

            var spreadsheet = _excelFixture.OpenReadSpreadsheet(map => map.Configure<FakeModel>(builder =>
                {
                    builder.Property(model => model.Id)
                        .HasDefault(expectedModel.Id);
                    builder.Property(model => model.Name)
                        .HasDefault(expectedModel.Name);
                    builder.Property(model => model.DisplayName)
                        .IsOptional(PropertyElementKind.Body)
                        .HasDefault(expectedModel.DisplayName);
                }));

            var spreadsheetPage = spreadsheet.Pages[0];

            // Act
            var readingResult = spreadsheetPage.ReadAll<FakeModel>();

            // Assert
            Assert.Equal(ReadingResultKind.Success, readingResult.Kind);
            Assert.Null(readingResult.HeaderFailure);
            Assert.Empty(readingResult.ResourceFailures);

            var resourceResult = Assert.Single(readingResult.Resources);
            Assert.Equal(2U, resourceResult.RowNumber);
            Assert.NotNull(resourceResult.Resource);
            Assert.Equal(expectedModel.Id, resourceResult.Resource!.Id);
            Assert.Equal(expectedModel.Name, resourceResult.Resource.Name);
            Assert.Equal(expectedModel.DisplayName, resourceResult.Resource.DisplayName);
        }

        [Fact]
        public void DoesNotReadDefaultValueForUnmatchingParseResults()
        {
            // Arrange
            var expectedModel = new FakeModel
            {
                Id = 0,
                Name = string.Empty,
                DisplayName = string.Empty,
                Amount = 0M,
            };

            _excelFixture.ArrangeSpreadsheet(
                spreadsheet => spreadsheet.AddPage("Sheet1")
                .AddCell("Id").AddCell("Name").AddCell("DisplayName").AdvanceRow()
                .AddCell("Foo").AdvanceRow());

            var spreadsheet = _excelFixture.OpenReadSpreadsheet(map => map.Configure<FakeModel>(builder =>
            {
                builder.Property(model => model.Id)
                    .HasDefault(
                        42,
                        ResourcePropertyDefaultReadingResolution.Empty,
                        ResourcePropertyDefaultReadingResolution.Missing);
                builder.Property(model => model.Name)
                    .HasDefault(
                        "name",
                        ResourcePropertyDefaultReadingResolution.Empty,
                        ResourcePropertyDefaultReadingResolution.Invalid);
                builder.Property(model => model.DisplayName)
                    .IsOptional(PropertyElementKind.Body)
                    .HasDefault(
                        "displayName",
                        ResourcePropertyDefaultReadingResolution.Missing,
                        ResourcePropertyDefaultReadingResolution.Invalid);
                builder.Property(model => model.Amount)
                    .IsOptional(PropertyElementKind.Header)
                    .HasDefault(42M);
            }));

            var spreadsheetPage = spreadsheet.Pages[0];

            // Act
            var readingResult = spreadsheetPage.ReadAll<FakeModel>();

            // Assert
            Assert.Equal(ReadingResultKind.Failure, readingResult.Kind);
            Assert.Null(readingResult.HeaderFailure);
            Assert.Empty(readingResult.Resources);

            var resourceFailure = Assert.Single(readingResult.ResourceFailures);
            Assert.Equal(2U, resourceFailure.RowNumber);

            var missingPropertyFailure = Assert.Single(resourceFailure.MissingProperties);
            Assert.Equal(2U, missingPropertyFailure.ColumnNumber);

            var invalidPropertyFailure = Assert.Single(resourceFailure.InvalidProperties);
            Assert.Equal(1U, invalidPropertyFailure.ColumnNumber);
            Assert.Equal("Foo", invalidPropertyFailure.Value);
        }
    }
}
