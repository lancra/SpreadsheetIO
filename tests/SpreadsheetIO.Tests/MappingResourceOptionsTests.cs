using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Tests.Testing.Fakes;
using LanceC.SpreadsheetIO.Tests.Testing.Fixtures;
using Xunit;

namespace LanceC.SpreadsheetIO.Tests;

public class MappingResourceOptionsTests : IDisposable
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

    public class TheContinueOnResourceReadingFailureResourceMapOptionOption : MappingResourceOptionsTests
    {
        [Fact]
        public void ResultsInPartialFailureWhenSpecified()
        {
            // Arrange
            var expectedModel = new FakeModel
            {
                Id = 3,
                Name = "Baz",
                Amount = 3M,
            };

            _excelFixture.ArrangeSpreadsheet(
                writingSpreadsheet => writingSpreadsheet.AddPage("Sheet1")
                    .AddCell("Id").AddCell("Name").AddCell("Amount").AdvanceRow()
                    .AddCell("1").AddCell("Foo").AddCell("foo").AdvanceRow()
                    .AddCell("Bar").AdvanceColumn().AddCell("2").AdvanceRow()
                    .AddCell("3").AddCell("Baz").AddCell("3"));

            var readingSpreadsheet = _excelFixture.OpenReadSpreadsheet(map => map.Configure<FakeModel>(
                builder =>
                {
                    builder.ContinuesOnResourceReadingFailure();

                    builder.Property(model => model.Id);
                    builder.Property(model => model.Name);
                    builder.Property(model => model.Amount);
                }));
            var readingSpreadsheetPage = readingSpreadsheet.Pages[0];

            // Act
            var readingResult = readingSpreadsheetPage.ReadAll<FakeModel>();

            // Assert
            Assert.Equal(ReadingResultKind.PartialFailure, readingResult.Kind);
            Assert.Null(readingResult.HeaderFailure);

            var resourceResult = Assert.Single(readingResult.Resources);
            Assert.Equal(4U, resourceResult.RowNumber);
            Assert.NotNull(resourceResult.Resource);
            Assert.Equal(expectedModel.Id, resourceResult.Resource!.Id);
            Assert.Equal(expectedModel.Name, resourceResult.Resource.Name);
            Assert.Equal(expectedModel.Amount, resourceResult.Resource.Amount);

            Assert.Equal(2, readingResult.ResourceFailures.Count);
            Assert.Single(
                readingResult.ResourceFailures,
                resourceFailure =>
                    resourceFailure.RowNumber == 2U &&
                    !resourceFailure.MissingProperties.Any() &&
                    resourceFailure.InvalidProperties.Count == 1 &&
                    resourceFailure.InvalidProperties.Any(invalidPropertyFailure =>
                        invalidPropertyFailure.ColumnNumber == 3U &&
                        invalidPropertyFailure.Value == "foo"));
            Assert.Single(
                readingResult.ResourceFailures,
                resourceFailure =>
                    resourceFailure.RowNumber == 3U &&
                    resourceFailure.MissingProperties.Count == 1 &&
                    resourceFailure.MissingProperties.Any(missingPropertyFailure => missingPropertyFailure.ColumnNumber == 2U) &&
                    resourceFailure.InvalidProperties.Count == 1 &&
                    resourceFailure.InvalidProperties.Any(invalidPropertyFailure =>
                        invalidPropertyFailure.ColumnNumber == 1U &&
                        invalidPropertyFailure.Value == "Bar"));
        }

        [Fact]
        public void ResultsInFailureWhenNotSpecified()
        {
            // Arrange
            _excelFixture.ArrangeSpreadsheet(
                writingSpreadsheet => writingSpreadsheet.AddPage("Sheet1")
                    .AddCell("Id").AddCell("Name").AddCell("Amount").AdvanceRow()
                    .AddCell("1").AddCell("Foo").AddCell("foo").AdvanceRow()
                    .AddCell("Bar").AdvanceColumn().AddCell("2").AdvanceRow()
                    .AddCell("3").AddCell("Baz").AddCell("3"));

            var readingSpreadsheet = _excelFixture.OpenReadSpreadsheet(map => map.Configure<FakeModel>(
                builder =>
                {
                    builder.Property(model => model.Id);
                    builder.Property(model => model.Name);
                    builder.Property(model => model.Amount);
                }));
            var readingSpreadsheetPage = readingSpreadsheet.Pages[0];

            // Act
            var readingResult = readingSpreadsheetPage.ReadAll<FakeModel>();

            // Assert
            Assert.Equal(ReadingResultKind.Failure, readingResult.Kind);
            Assert.Null(readingResult.HeaderFailure);
            Assert.Empty(readingResult.Resources);

            Assert.Equal(1, readingResult.ResourceFailures.Count);
            Assert.Single(
                readingResult.ResourceFailures,
                resourceFailure =>
                    resourceFailure.RowNumber == 2U &&
                    !resourceFailure.MissingProperties.Any() &&
                    resourceFailure.InvalidProperties.Count == 1 &&
                    resourceFailure.InvalidProperties.Any(invalidPropertyFailure =>
                        invalidPropertyFailure.ColumnNumber == 3U &&
                        invalidPropertyFailure.Value == "foo"));
        }
    }

    public class TheHeaderRowNumberResourceMapOption : MappingResourceOptionsTests
    {
        [Fact]
        public void StartsReadingAtSpecifiedRow()
        {
            // Arrange
            var model = new FakeModel
            {
                Id = 1,
                Name = "One",
                DisplayName = "Uno",
            };

            _excelFixture.ArrangeSpreadsheet(
                spreadsheet => spreadsheet.AddPage("Sheet1")
                    .AdvanceRow()
                    .AdvanceRow()
                    .AddCell("Id").AddCell("Name").AddCell("DisplayName").AdvanceRow()
                    .AddCell(model.Id.ToString()).AddCell(model.Name).AddCell(model.DisplayName));

            var spreadsheet = _excelFixture.OpenReadSpreadsheet(map => map.Configure<FakeModel>(builder =>
                {
                    builder.HasHeaderRowNumber(3U);

                    builder.Property(model => model.Id);
                    builder.Property(model => model.Name);
                    builder.Property(model => model.DisplayName);
                }));
            var spreadsheetPage = spreadsheet.Pages[0];

            // Act
            var readingResult = spreadsheetPage.ReadAll<FakeModel>();

            // Assert
            Assert.Equal(ReadingResultKind.Success, readingResult.Kind);
            Assert.Null(readingResult.HeaderFailure);
            Assert.Empty(readingResult.ResourceFailures);

            var resourceResult = Assert.Single(readingResult.Resources);
            Assert.Equal(4U, resourceResult.RowNumber);
            Assert.NotNull(resourceResult.Resource);
            Assert.Equal(model.Id, resourceResult.Resource!.Id);
            Assert.Equal(model.Name, resourceResult.Resource.Name);
            Assert.Equal(model.DisplayName, resourceResult.Resource.DisplayName);
        }
    }
}
