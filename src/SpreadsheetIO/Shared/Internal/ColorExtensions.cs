using System.Drawing;
using System.Text;

namespace LanceC.SpreadsheetIO.Shared.Internal;

internal static class ColorExtensions
{
    private const string HexFormat = "X2";

    public static Color ToColor(this uint hex)
        => Color.FromArgb((int)hex);

    public static string ToHex(this Color color)
        => new StringBuilder()
        .Append(color.A.ToString(HexFormat))
        .Append(color.R.ToString(HexFormat))
        .Append(color.G.ToString(HexFormat))
        .Append(color.B.ToString(HexFormat))
        .ToString();
}
