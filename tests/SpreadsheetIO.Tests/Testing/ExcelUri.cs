using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Tests.Testing;

public class ExcelUri : Uri
{
    [SuppressMessage(
        "Design",
        "CA1054:URI-like parameters should not be strings",
        Justification = "The purpose of this constructor is to create a URI from a string.")]
    public ExcelUri(string uriString)
        : base(uriString, UriKind.Relative)
    {
        IsFile = !string.IsNullOrEmpty(Path.GetExtension(uriString));
    }

    public new bool IsFile { get; }
}
