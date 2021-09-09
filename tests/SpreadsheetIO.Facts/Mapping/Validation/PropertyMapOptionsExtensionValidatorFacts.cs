using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Internal.Extensions;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Styling;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation
{
    public class PropertyMapOptionsExtensionValidatorFacts
    {
        private readonly AutoMocker _mocker = new();

        private FakePropertyMapOptionsExtensionValidator CreateSystemUnderTest()
            => _mocker.CreateInstance<FakePropertyMapOptionsExtensionValidator>();

        public class TheCanValidateMethod : PropertyMapOptionsExtensionValidatorFacts
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
        private class FakePropertyMapOptionsExtensionValidator :
            PropertyMapOptionsExtensionValidator<OptionalPropertyMapOptionsExtension>
        {
            public override ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
                => ResourceMapValidationResult.Success();
        }

        private class FakeValidatableResourceMap : ResourceMap<FakeModel>
        {
            public FakeValidatableResourceMap(bool hasExtension)
            {
                Map(
                    model => model.Id,
                    options =>
                    {
                        options.UseHeaderStyle(BuiltInPackageStyle.Bold);
                        if (hasExtension)
                        {
                            options.MarkAsOptional();
                        }
                    });
            }
        }
    }
}
