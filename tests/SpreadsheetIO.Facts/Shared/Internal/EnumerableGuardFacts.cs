using Ardalis.GuardClauses;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal;

public class EnumerableGuardFacts
{
    public class TheNullElementsMethod
    {
        [Fact]
        public void ThrowsArgumentNullExceptionWhenInputContainsNullElement()
        {
            // Arrange
            var input = new[] { "1", default, string.Empty, };
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullElements(input, parameterName));

            // Assert
            Assert.NotNull(exception);
            var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(parameterName, argumentNullException.ParamName);
        }

        [Fact]
        public void DoesNotThrowExceptionWhenInputIsNull()
        {
            // Arrange
            var input = default(IEnumerable<string>);
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullElements(input, parameterName));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DoesNotThrowExceptionWhenInputIsEmpty()
        {
            // Arrange
            var input = Array.Empty<string>();
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullElements(input, parameterName));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DoesNotThrowExceptionWhenInputDoesNotContainNullElements()
        {
            // Arrange
            var input = new[] { "1", "two", string.Empty, };
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullElements(input, parameterName));

            // Assert
            Assert.Null(exception);
        }
    }

    public class TheNullOrEmptyElementsMethod
    {
        [Fact]
        public void ThrowsArgumentExceptionWhenInputContainsEmptyElement()
        {
            // Arrange
            var input = new[] { "1", "two", string.Empty, };
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullOrEmptyElements(input, parameterName));

            // Assert
            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);
            Assert.Equal(parameterName, argumentException.ParamName);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenInputContainsNullElement()
        {
            // Arrange
            var input = new[] { "1", default, "three", };
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullOrEmptyElements(input!, parameterName));

            // Assert
            Assert.NotNull(exception);
            var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(parameterName, argumentNullException.ParamName);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenInputContainsNullAndEmptyElements()
        {
            // Arrange
            var input = new[] { "1", default, string.Empty, };
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullOrEmptyElements(input!, parameterName));

            // Assert
            Assert.NotNull(exception);
            var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(parameterName, argumentNullException.ParamName);
        }

        [Fact]
        public void DoesNotThrowExceptionWhenInputIsNull()
        {
            // Arrange
            var input = default(IEnumerable<string>);
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullOrEmptyElements(input, parameterName));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DoesNotThrowExceptionWhenInputIsEmpty()
        {
            // Arrange
            var input = Array.Empty<string>();
            var parameterName = "input";

            // Act
            var exception = Record.Exception(() => Guard.Against.NullOrEmptyElements(input, parameterName));

            // Assert
            Assert.Null(exception);
        }
    }
}
