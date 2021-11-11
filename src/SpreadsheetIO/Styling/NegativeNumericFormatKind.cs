using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Styling;

/// <summary>
/// Represents the kind of numeric format for negative numbers.
/// </summary>
[ExcludeFromCodeCoverage]
public class NegativeNumericFormatKind : SmartEnum<NegativeNumericFormatKind>
{
    /// <summary>
    /// Specifies that negative numbers will be prefixed by a hyphen.
    /// </summary>
    public static readonly NegativeNumericFormatKind Default = new(1, "Default");

    /// <summary>
    /// Specifies that negative numbers will be surrounded by parentheses.
    /// </summary>
    public static readonly NegativeNumericFormatKind Parentheses = new(2, "Parentheses", hasParentheses: true);

    /// <summary>
    /// Specifies that negative numbers will be red.
    /// </summary>
    public static readonly NegativeNumericFormatKind Red = new(3, "Red", hasColor: true);

    /// <summary>
    /// Specifies that negative numbers will be red and surrounded by parentheses.
    /// </summary>
    public static readonly NegativeNumericFormatKind RedParentheses =
        new(4, "Red Parentheses", hasColor: true, hasParentheses: true);

    private NegativeNumericFormatKind(int id, string name, bool hasColor = false, bool hasParentheses = false)
        : base(name, id)
    {
        HasColor = hasColor;
        HasParentheses = hasParentheses;
    }

    /// <summary>
    /// Gets the value that determines whether the format has a color.
    /// </summary>
    public bool HasColor { get; }

    /// <summary>
    /// Gets the value that determines whether the format is surrounded by parentheses.
    /// </summary>
    public bool HasParentheses { get; }
}
