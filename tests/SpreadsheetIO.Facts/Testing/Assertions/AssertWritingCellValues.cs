using LanceC.SpreadsheetIO.Writing;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Testing.Assertions
{
    public static class AssertWritingCellValues
    {
        public static void Equal(WritingCellValue expected, WritingCellValue actual)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);

            var expectedCell = new OpenXml.Cell();
            expected.CellModifier?.Invoke(expectedCell);

            var actualCell = new OpenXml.Cell();
            actual.CellModifier?.Invoke(actualCell);

            Assert.Equal(expectedCell, actualCell);
        }
    }
}
