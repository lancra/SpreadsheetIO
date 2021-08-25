using System;
using System.Linq;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Tests.Testing;
using LanceC.SpreadsheetIO.Tests.Testing.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace LanceC.SpreadsheetIO.Tests
{
    public class ReadingTests : IDisposable
    {
        private readonly ExcelFixture _excelFixture;

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

            var spreadsheet = _excelFixture.OpenReadSpreadsheet(services => services
                .AddSingleton<IResourceMap, FakeModelMap>());
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
}
