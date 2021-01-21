using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class SpreadsheetDocumentWrapper : ISpreadsheetDocumentWrapper
    {
        private readonly SpreadsheetDocument _spreadsheetDocument;
        private readonly WorkbookPart _workbookPart;

        public SpreadsheetDocumentWrapper(SpreadsheetDocument spreadsheetDocument)
        {
            _spreadsheetDocument = spreadsheetDocument;
            _workbookPart = _spreadsheetDocument.AddWorkbookPart();
            _workbookPart.Workbook = new Workbook();
            _workbookPart.Workbook.AppendChild(new Sheets());
        }

        public ISharedStringTablePartWrapper AddSharedStringTablePart()
        {
            var sharedStringTablePart = _workbookPart.AddNewPart<SharedStringTablePart>();

            var sharedStringTablePartWrapper = new SharedStringTablePartWrapper(sharedStringTablePart);
            return sharedStringTablePartWrapper;
        }

        public IWorksheetPartWrapper AddWorksheetPart(string name)
        {
            var worksheetPart = _workbookPart.AddNewPart<WorksheetPart>();

            var worksheetPartId = _workbookPart.GetIdOfPart(worksheetPart);
            var sheets = _workbookPart.Workbook.Sheets;
            sheets.Append(
                new Sheet
                {
                    Id = worksheetPartId,
                    Name = name,
                    SheetId = Convert.ToUInt32(sheets.Count() + 1),
                });

            var worksheetPartWrapper = new WorksheetPartWrapper(worksheetPart, name);
            return worksheetPartWrapper;
        }

        public IWorkbookStylesPartWrapper AddWorkbookStylesPart()
        {
            var workbookStylesPart = _workbookPart.AddNewPart<WorkbookStylesPart>();

            var workbookStylesPartWrapper = new WorkbookStylesPartWrapper(workbookStylesPart);
            return workbookStylesPartWrapper;
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
                _spreadsheetDocument.Dispose();
            }
        }
    }
}
