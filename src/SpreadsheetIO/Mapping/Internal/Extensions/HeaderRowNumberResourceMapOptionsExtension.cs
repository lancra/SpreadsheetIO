using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class HeaderRowNumberResourceMapOptionsExtension : IResourceMapOptionsExtension
    {
        public HeaderRowNumberResourceMapOptionsExtension(uint number)
        {
            Number = number;
        }

        public uint Number { get; }
    }
}
