using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    internal class StylesheetGenerator : ISpreadsheetGenerator
    {
        private readonly IEnumerable<IStylesheetMutator> _stylesheetMutators;

        public StylesheetGenerator(IEnumerable<IStylesheetMutator> stylesheetMutators)
        {
            _stylesheetMutators = stylesheetMutators;
        }

        public void Generate(ISpreadsheetDocumentWrapper spreadsheetDocument)
        {
            var workbookStylesPart = spreadsheetDocument.AddWorkbookStylesPart();
            var stylesheet = new OpenXml.Stylesheet();
            foreach (var mutator in _stylesheetMutators)
            {
                mutator.Mutate(stylesheet);
            }

            workbookStylesPart.SetStylesheet(stylesheet);
        }
    }
}
