using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Mapping.Validation;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation
{
    public class ResourceMapOptionsExtensionValidatorFacts
    {
        private readonly AutoMocker _mocker = new();

        private FakeResourceMapOptionsExtensionValidator CreateSystemUnderTest()
            => _mocker.CreateInstance<FakeResourceMapOptionsExtensionValidator>();

        public class TheCanValidateMethod : ResourceMapOptionsExtensionValidatorFacts
        {
            [Fact]
            public void ReturnsTrueWhenMapContainsSpecifiedResourceMapOptionsExtension()
            {
                // Arrange
                var map = new FakeValidatableResourceMap(true);
                var sut = CreateSystemUnderTest();

                // Act
                var canValidate = sut.CanValidate(map);

                // Assert
                Assert.True(canValidate);
            }

            [Fact]
            public void ReturnsFalseWhenMapDoesNotContainSpecifiedResourceMapOptionsExtension()
            {
                // Arrange
                var map = new FakeValidatableResourceMap(false);
                var sut = CreateSystemUnderTest();

                // Act
                var canValidate = sut.CanValidate(map);

                // Assert
                Assert.False(canValidate);
            }
        }

        [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Created from AutoMocker.")]
        private class FakeResourceMapOptionsExtensionValidator :
            ResourceMapOptionsExtensionValidator<ExitOnResourceReadingFailureResourceMapOptionsExtension>
        {
            public override ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
                => ResourceMapValidationResult.Success();
        }

        private class FakeValidatableResourceMap : ResourceMap<FakeModel>
        {
            private readonly bool _hasExtension;

            public FakeValidatableResourceMap(bool hasExtension)
            {
                _hasExtension = hasExtension;
            }

            protected override void Configure(ResourceMapOptionsBuilder<FakeModel> optionsBuilder)
            {
                optionsBuilder.OverrideHeaderRowNumber(2U);
                if (_hasExtension)
                {
                    optionsBuilder.ExitOnResourceReadingFailure();
                }

                base.Configure(optionsBuilder);
            }
        }
    }
}
