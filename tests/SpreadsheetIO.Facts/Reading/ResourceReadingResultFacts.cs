using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Failures;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading
{
    public class ResourceReadingResultFacts
    {
        private static ResourceReadingResult<FakeModel> CreateSystemUnderTest(
            NumberedResource<FakeModel>? numberedResource = default,
            ResourceReadingFailure? failure = default)
            => new(numberedResource, failure);

        public class TheKindProperty : ResourceReadingResultFacts
        {
            [Fact]
            public void ReturnsFailureWhenFailureIsNotNull()
            {
                // Arrange
                var sut = CreateSystemUnderTest(
                    failure: new ResourceReadingFailure(
                        2U,
                        Array.Empty<MissingResourcePropertyReadingFailure>(),
                        Array.Empty<InvalidResourcePropertyReadingFailure>()));

                // Act
                var kind = sut.Kind;

                // Assert
                Assert.Equal(ResourceReadingResultKind.Failure, kind);
            }

            [Fact]
            public void ReturnsSuccessWhenFailureIsNull()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var kind = sut.Kind;

                // Assert
                Assert.Equal(ResourceReadingResultKind.Success, kind);
            }
        }
    }
}
