using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class WorkbookStylesPartWrapper : IWorkbookStylesPartWrapper
    {
        private readonly WorkbookStylesPart _workbookStylesPart;

        public WorkbookStylesPartWrapper(WorkbookStylesPart workbookStylesPart)
        {
            _workbookStylesPart = workbookStylesPart;
        }

        public void SetStylesheet(Stylesheet stylesheet)
        {
            _workbookStylesPart.Stylesheet = stylesheet;
        }
    }
}
