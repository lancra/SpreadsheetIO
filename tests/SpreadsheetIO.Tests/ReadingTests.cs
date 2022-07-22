using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Tests.Testing;
using LanceC.SpreadsheetIO.Tests.Testing.Fakes;
using LanceC.SpreadsheetIO.Tests.Testing.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace LanceC.SpreadsheetIO.Tests;

public class ReadingTests : IDisposable
{
    private readonly FileExcelFixture _excelFixture;

    public ReadingTests(ITestOutputHelper output)
    {
        _excelFixture = new(output);
    }

    [Fact]
    [ExcelSourceFile("MapSimple.xlsx")]
    public void MappedSimpleSpreadsheetPage()
    {
        // Arrange
        var expectedModels = new[]
        {
            new FakeModel
            {
                Id = 1,
                Name = "One",
                DisplayName = "Uno",
                Date = new DateTime(2021, 1, 1),
                Amount = 1.1100000000000001M,
                Letter = 'O',
            },
            new FakeModel
            {
                Id = 2,
                Name = "Two",
                Date = new DateTime(2021, 2, 22),
                Amount = 22M,
                Letter = 'T',
            },
            new FakeModel
            {
                Id = 3,
                Name = "Three",
                DisplayName = "Tres",
                Date = new DateTime(2021, 3, 3),
                Amount = 3M,
                Letter = 'T',
            },
        };

        var spreadsheet = _excelFixture.OpenReadSpreadsheet(map => map.ApplyConfiguration(new FakeModelMapConfiguration()));
        var spreadsheetPage = spreadsheet.Pages["Map"];

        // Act
        var readingResult = spreadsheetPage.ReadAll<FakeModel>();

        // Assert
        Assert.Equal(ReadingResultKind.Success, readingResult.Kind);
        Assert.Null(readingResult.HeaderFailure);
        Assert.Empty(readingResult.ResourceFailures);

        var actualModels = readingResult.Resources.ToArray();
        Assert.Equal(expectedModels.Length, actualModels.Length);
        for (var i = 0; i < expectedModels.Length; i++)
        {
            var expectedModel = expectedModels[i];
            var actualNumberedModel = actualModels[i];

            Assert.Equal(Convert.ToUInt32(i + 2), actualNumberedModel.RowNumber);
            Assert.NotNull(actualNumberedModel.Resource);
            Assert.Equal(expectedModel.Id, actualNumberedModel.Resource!.Id);
            Assert.Equal(expectedModel.Name, actualNumberedModel.Resource.Name);
            Assert.Equal(expectedModel.DisplayName, actualNumberedModel.Resource.DisplayName);
            Assert.Equal(expectedModel.Date, actualNumberedModel.Resource.Date);
            Assert.Equal(expectedModel.Amount, actualNumberedModel.Resource.Amount);
            Assert.Equal(expectedModel.Letter, actualNumberedModel.Resource.Letter);
        }
    }

    [Fact]
    [ExcelSourceFile("Read1900Dates.xlsx")]
    public void Mapped1900Dates()
    {
        // Arrange
        var expectedResources = new[]
        {
            new NumberedResource<FakeDateModel>(2U, new(new DateTime(1900, 3, 1))),
            new NumberedResource<FakeDateModel>(4U, new(new DateTime(1900, 2, 28))),
            new NumberedResource<FakeDateModel>(5U, new(new DateTime(1900, 2, 27))),
            new NumberedResource<FakeDateModel>(6U, new(new DateTime(1900, 1, 2))),
            new NumberedResource<FakeDateModel>(7U, new(new DateTime(1900, 1, 1))),
            new NumberedResource<FakeDateModel>(9U, new(new DateTime(1899, 12, 31))),
        };

        var spreadsheet = _excelFixture.OpenReadSpreadsheet(map => map.ApplyConfiguration(new FakeDateModelMapConfiguration()));
        var spreadsheetPage = spreadsheet.Pages[0];

        // Act
        var readingResult = spreadsheetPage.ReadAll<FakeDateModel>();

        // Assert
        Assert.Equal(ReadingResultKind.PartialFailure, readingResult.Kind);
        Assert.Null(readingResult.HeaderFailure);

        Assert.Equal(2, readingResult.ResourceFailures.Count);
        Assert.Single(
            readingResult.ResourceFailures,
            resourceFailure => resourceFailure.RowNumber == 3U && resourceFailure.InvalidProperties.Count == 2);
        Assert.Single(
            readingResult.ResourceFailures,
            resourceFailure => resourceFailure.RowNumber == 8U && resourceFailure.InvalidProperties.Count == 2);

        var actualResources = readingResult.Resources.ToArray();
        Assert.Equal(expectedResources.Length, actualResources.Length);
        for (var i = 0; i < expectedResources.Length; i++)
        {
            var expectedResource = expectedResources[i];
            var actualResource = actualResources[i];

            Assert.Equal(expectedResource.RowNumber, actualResource.RowNumber);
            Assert.Equal(expectedResource.Resource!.DateNumber, actualResource.Resource!.DateNumber);
            Assert.Equal(expectedResource.Resource.DateText, actualResource.Resource.DateText);
        }
    }

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
}
