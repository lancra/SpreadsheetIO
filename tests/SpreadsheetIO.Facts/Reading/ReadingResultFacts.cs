using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Failures;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading
{
    public class ReadingResultFacts
    {
        private static ReadingResult<FakeModel> CreateSystemUnderTest(
            IReadOnlyCollection<NumberedResource<FakeModel>>? resources = default,
            HeaderReadingFailure? headerFailure = default,
            IReadOnlyCollection<ResourceReadingFailure>? resourceFailures = default)
            => new(
                resources ?? Array.Empty<NumberedResource<FakeModel>>(),
                headerFailure,
                resourceFailures ?? Array.Empty<ResourceReadingFailure>());

        public class TheKindProperty : ReadingResultFacts
        {
            [Fact]
            public void ReturnsFailureWhenHeaderFailureIsNotNull()
            {
                // Arrange
                var sut = CreateSystemUnderTest(
                    headerFailure: new HeaderReadingFailure(
                        true,
                        Array.Empty<MissingHeaderReadingFailure>(),
                        Array.Empty<InvalidHeaderReadingFailure>()));

                // Act
                var kind = sut.Kind;

                // Assert
                Assert.Equal(ReadingResultKind.Failure, kind);
            }

            [Fact]
            public void ReturnsFailureWhenResourceFailuresIsNotEmptyAndResourcesIsEmpty()
            {
                // Arrange
                var sut = CreateSystemUnderTest(
                    resourceFailures: new[]
                    {
                        new ResourceReadingFailure(
                            2U,
                            Array.Empty<MissingResourcePropertyReadingFailure>(),
                            Array.Empty<InvalidResourcePropertyReadingFailure>()),
                    });

                // Act
                var kind = sut.Kind;

                // Assert
                Assert.Equal(ReadingResultKind.Failure, kind);
            }

            [Fact]
            public void ReturnsPartialFailureWhenResourceFailuresIsNotEmptyAndResourcesIsNotEmpty()
            {
                // Arrange
                var sut = CreateSystemUnderTest(
                    resources: new[]
                    {
                        new NumberedResource<FakeModel>(2U, new()),
                    },
                    resourceFailures: new[]
                    {
                        new ResourceReadingFailure(
                            3U,
                            Array.Empty<MissingResourcePropertyReadingFailure>(),
                            Array.Empty<InvalidResourcePropertyReadingFailure>()),
                    });

                // Act
                var kind = sut.Kind;

                // Assert
                Assert.Equal(ReadingResultKind.PartialFailure, kind);
            }

            [Fact]
            public void ReturnsSuccessWhenHeaderFailureIsNullAndResourceFailuresIsEmpty()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var kind = sut.Kind;

                // Assert
                Assert.Equal(ReadingResultKind.Success, kind);
            }
        }
    }
}
