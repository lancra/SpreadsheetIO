using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling.Internal
{
    [ExcludeFromCodeCoverage]
    internal class ConstantNumericFormats : ReadOnlyDictionary<string, uint>
    {
        private static readonly Dictionary<string, uint> KeyValues =
            new()
            {
                ["_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)"] = 43,
                ["_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)"] = 44,
            };

        public ConstantNumericFormats()
            : base(KeyValues)
        {
        }
    }
}
