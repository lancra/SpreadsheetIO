using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping;

public class ResourceMapResultFacts
{
    public class TheSuccessMethod : ResourceMapResultFacts
    {
        [Fact]
        public void ReturnsValidResult()
        {
            // Arrange
            var resourceMap = ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>());

            // Act
            var result = ResourceMapResult.Success(resourceMap);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(resourceMap, result.Value);
            Assert.Null(result.Error);
        }
    }

    public class TheFailureMethod : ResourceMapResultFacts
    {
        [Fact]
        public void ReturnsInvalidResult()
        {
            // Arrange
            var error = ResourceMapErrorCreator.Create();

            // Act
            var result = ResourceMapResult.Failure(error);

            // Assert
            Assert.False(result.IsValid);
            Assert.Null(result.Value);
            Assert.Equal(error, result.Error);
        }
    }
}
