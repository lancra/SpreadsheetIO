using System.Text;
using LanceC.SpreadsheetIO.Shared.Internal;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal
{
    public class StringBuilderExtensionsFacts
    {
        public class TheAppendIfMethod : StringBuilderExtensionsFacts
        {
            [Fact]
            public void AppendsTheValueWhenTheConditionIsTrue()
            {
                // Arrange
                var builder = new StringBuilder();
                var value = "foo";
                var condition = true;

                // Act
                builder.AppendIf(value, condition);

                // Assert
                Assert.Equal(value, builder.ToString());
            }

            [Fact]
            public void DoesNotAppendTheValueWhenTheConditionIsFalse()
            {
                // Arrange
                var builder = new StringBuilder();
                var value = "foo";
                var condition = false;

                // Act
                builder.AppendIf(value, condition);

                // Assert
                Assert.Equal(string.Empty, builder.ToString());
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void ReturnsProvidedStringBuilderInstance(bool condition)
            {
                // Arrange
                var expectedBuilder = new StringBuilder();

                // Act
                var actualBuilder = expectedBuilder.AppendIf("foo", condition);

                // Assert
                Assert.Equal(expectedBuilder, actualBuilder);
            }
        }
    }
}
