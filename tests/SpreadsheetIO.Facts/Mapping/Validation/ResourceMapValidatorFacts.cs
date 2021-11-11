using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Validation;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Validation;

public class ResourceMapValidatorFacts
{
    private readonly AutoMocker _mocker = new();

    private FakeResourceMapValidator CreateSystemUnderTest()
        => _mocker.CreateInstance<FakeResourceMapValidator>();

    public class TheCanValidateMethod : ResourceMapValidatorFacts
    {
        [Fact]
        public void ReturnsTrue()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var canValidate = sut.CanValidate(new FakeStringResourceMap());

            // Assert
            Assert.True(canValidate);
        }
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Created from AutoMocker.")]
    private class FakeResourceMapValidator : ResourceMapValidator
    {
        public override ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
            => ResourceMapValidationResult.Success();
    }
}
