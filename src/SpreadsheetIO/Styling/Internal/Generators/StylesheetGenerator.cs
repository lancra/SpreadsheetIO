using System.Collections.Generic;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    internal class StylesheetGenerator : IStylesheetGenerator
    {
        private readonly IEnumerable<IStylesheetMutator> _stylesheetMutators;

        public StylesheetGenerator(IEnumerable<IStylesheetMutator> stylesheetMutators)
        {
            _stylesheetMutators = stylesheetMutators;
        }

        public OpenXml.Stylesheet Generate()
        {
            var stylesheet = new OpenXml.Stylesheet();
            foreach (var mutator in _stylesheetMutators)
            {
                mutator.Mutate(stylesheet);
            }

            return stylesheet;
        }
    }
}
