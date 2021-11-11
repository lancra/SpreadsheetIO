using Ardalis.GuardClauses;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal
{
    public class AlphanumericGuardFacts
    {
        public class TheNonAlphabeticMethod
        {
            [Theory]
            [InlineData("A,")]
            [InlineData("A1")]
            [InlineData("A@")]
            [InlineData("()")]
            [InlineData("1")]
            public void ThrowsArgumentExceptionWhenInputIsNonAlphabetic(string input)
            {
                // Arrange
                var parameterName = "input";

                // Act
                var exception = Record.Exception(() => Guard.Against.NonAlphabetic(input, parameterName));

                // Assert
                Assert.NotNull(exception);
                var argumentException = Assert.IsType<ArgumentException>(exception);
                Assert.Equal(parameterName, argumentException.ParamName);
            }

            [Fact]
            public void DoesNotThrowExceptionWhenInputIsAlphabetic()
            {
                // Arrange
                var input = "AaZz";

                // Act
                var exception = Record.Exception(() => Guard.Against.NonAlphabetic(input, "input"));

                // Assert
                Assert.Null(exception);
            }
        }

        public class TheNonAlphanumericMethod
        {
            [Theory]
            [InlineData("A,")]
            [InlineData("A@")]
            [InlineData("()")]
            public void ThrowsArgumentExceptionWhenInputIsNonAlphanumeric(string input)
            {
                // Arrange
                var parameterName = "input";

                // Act
                var exception = Record.Exception(() => Guard.Against.NonAlphanumeric(input, parameterName));

                // Assert
                Assert.NotNull(exception);
                var argumentException = Assert.IsType<ArgumentException>(exception);
                Assert.Equal(parameterName, argumentException.ParamName);
            }

            [Fact]
            public void DoesNotThrowExceptionWhenInputIsAlphanumeric()
            {
                // Arrange
                var input = "AaZz10";

                // Act
                var exception = Record.Exception(() => Guard.Against.NonAlphanumeric(input, "input"));

                // Assert
                Assert.Null(exception);
            }
        }

        public class TheNonNumericMethod
        {
            [Theory]
            [InlineData("A,")]
            [InlineData("A1")]
            [InlineData("A@")]
            [InlineData("()")]
            [InlineData("A")]
            public void ThrowsArgumentExceptionWhenInputIsNonNumeric(string input)
            {
                // Arrange
                var parameterName = "input";

                // Act
                var exception = Record.Exception(() => Guard.Against.NonNumeric(input, parameterName));

                // Assert
                Assert.NotNull(exception);
                var argumentException = Assert.IsType<ArgumentException>(exception);
                Assert.Equal(parameterName, argumentException.ParamName);
            }

            [Fact]
            public void DoesNotThrowExceptionWhenInputIsNumeric()
            {
                // Arrange
                var input = "1234567890";

                // Act
                var exception = Record.Exception(() => Guard.Against.NonNumeric(input, "input"));

                // Assert
                Assert.Null(exception);
            }
        }
    }
}
