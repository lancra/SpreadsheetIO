using System.Drawing;
using LanceC.SpreadsheetIO.Shared;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared
{
    public class ColorExtensionsFacts
    {
        public class TheToColorMethod
        {
            [Fact]
            public void ReturnsColor()
            {
                // Arrange
                var hex = 0xFFC32148;

                // Act
                var color = hex.ToColor();

                // Assert
                Assert.Equal(255, color.A);
                Assert.Equal(195, color.R);
                Assert.Equal(33, color.G);
                Assert.Equal(72, color.B);
            }
        }

        public class TheToHexMethod
        {
            [Fact]
            public void ReturnsHex()
            {
                // Arrange
                var color = Color.Fuchsia;

                // Act
                var hex = color.ToHex();

                // Assert
                Assert.Equal("FFFF00FF", hex);
            }
        }
    }
}
