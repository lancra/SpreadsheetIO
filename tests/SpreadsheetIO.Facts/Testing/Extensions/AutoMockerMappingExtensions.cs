using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Mapping;
using Moq;
using Moq.AutoMock;

namespace LanceC.SpreadsheetIO.Facts.Testing.Extensions;

internal static class AutoMockerMappingExtensions
{
    [SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "The mock container is present for organization of extensions.")]
    public static Mock<IInternalPropertyMapBuilder> GetMockForInternalPropertyMapBuilder(
        this AutoMocker mocker,
        Type resourceType,
        string propertyName,
        PropertyMapKey? key = default)
    {
        var builderMock = new Mock<IInternalPropertyMapBuilder>();

        var propertyInfo = resourceType.GetProperty(propertyName)!;
        builderMock.SetupGet(builder => builder.PropertyInfo)
            .Returns(propertyInfo);

        var keyBuilderMock = new Mock<IInternalPropertyMapKeyBuilder>();
        keyBuilderMock.SetupGet(builder => builder.Key)
            .Returns(key ?? PropertyMapKeyCreator.Create(name: propertyName));

        builderMock.SetupGet(builder => builder.KeyBuilder)
            .Returns(keyBuilderMock.Object);

        return builderMock;
    }
}
