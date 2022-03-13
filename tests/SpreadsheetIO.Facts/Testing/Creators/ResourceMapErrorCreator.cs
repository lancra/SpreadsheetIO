using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Validation;

namespace LanceC.SpreadsheetIO.Facts.Testing.Creators;

public static class ResourceMapErrorCreator
{
    public static ResourceMapError Create(
        IEnumerable<MapOptionConversionResult>? conversions = default,
        IEnumerable<ResourceMapBuilderValidationResult>? validations = default)
        => new(
            conversions?.ToArray() ?? Array.Empty<MapOptionConversionResult>(),
            validations?.ToArray() ?? Array.Empty<ResourceMapBuilderValidationResult>());
}
