using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using LanceC.SpreadsheetIO.Writing.Internal.Writers;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Writers;

public class SpreadsheetPageMapWriterFacts
{
    private readonly AutoMocker _mocker = new();

    private SpreadsheetPageMapWriter CreateSystemUnderTest()
        => _mocker.CreateInstance<SpreadsheetPageMapWriter>();

    public class TheWriteMethod : SpreadsheetPageMapWriterFacts
    {
        [Fact]
        public void SortsPropertyMapsByColumnNumberIfSpecifiedThenByDefinedOrder()
        {
            // Arrange
            var spreadsheetPageMock = new FakeWritingSpreadsheetPage();
            var resources = Array.Empty<FakeModel>();

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id, keyAction => keyAction.WithNumber(1U)),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Display, keyAction => keyAction.WithNumber(3U)),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Decimal, keyAction => keyAction.WithNumber(2U)),
                    PropertyMapCreator2.CreateForFakeModel(model => model.DateTime),
                });

            var sut = CreateSystemUnderTest();

            // Act
            sut.Write(spreadsheetPageMock, resources, map);

            // Assert
            spreadsheetPageMock.VerifyAddCell(1, 1, nameof(FakeModel.Id), default);
            spreadsheetPageMock.VerifyAddCell(1, 2, nameof(FakeModel.Decimal), default);
            spreadsheetPageMock.VerifyAddCell(1, 3, nameof(FakeModel.Display), default);
            spreadsheetPageMock.VerifyAddCell(1, 4, nameof(FakeModel.Name), default);
            spreadsheetPageMock.VerifyAddCell(1, 5, nameof(FakeModel.DateTime), default);
        }

        [Fact]
        public void UsesResourcePropertySerializerForBodyCells()
        {
            // Arrange
            var spreadsheetPageMock = new FakeWritingSpreadsheetPage();

            var resource = new FakeModel { Id = 1, Name = "One", Display = "Uno", };
            var resources = new[] { resource, };

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Display),
                });

            var idPropertyMap = map.Properties.Single(propertyMap => propertyMap.Key.Name == nameof(FakeModel.Id));
            var namePropertyMap = map.Properties.Single(propertyMap => propertyMap.Key.Name == nameof(FakeModel.Name));
            var displayPropertyMap = map.Properties.Single(propertyMap => propertyMap.Key.Name == nameof(FakeModel.Display));

            var idCellValue = new WritingCellValue(resource.Id);
            var nameCellValue = new WritingCellValue(resource.Name);
            var displayCellValue = new WritingCellValue(resource.Display);
            _mocker.GetMock<IResourcePropertySerializer>()
                .Setup(resourcePropertySerializer => resourcePropertySerializer.Serialize(resource, idPropertyMap))
                .Returns(idCellValue);
            _mocker.GetMock<IResourcePropertySerializer>()
                .Setup(resourcePropertySerializer => resourcePropertySerializer.Serialize(resource, namePropertyMap))
                .Returns(nameCellValue);
            _mocker.GetMock<IResourcePropertySerializer>()
                .Setup(resourcePropertySerializer => resourcePropertySerializer.Serialize(resource, displayPropertyMap))
                .Returns(displayCellValue);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Write(spreadsheetPageMock, resources, map);

            // Assert
            spreadsheetPageMock.VerifyAddCell(2, 1, idCellValue, default);
            spreadsheetPageMock.VerifyAddCell(2, 2, nameCellValue, default);
            spreadsheetPageMock.VerifyAddCell(2, 3, displayCellValue, default);
        }

        [Fact]
        public void AdvancesHeaderRowWhenOptionsExtensionSpecified()
        {
            // Arrange
            var spreadsheetPageMock = new FakeWritingSpreadsheetPage();
            var resources = Array.Empty<FakeModel>();
            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Display),
                },
                new HeaderRowNumberResourceMapOption(2U));

            var sut = CreateSystemUnderTest();

            // Act
            sut.Write(spreadsheetPageMock, resources, map);

            // Assert
            spreadsheetPageMock.VerifyAddCell(2, 1, nameof(FakeModel.Id), default);
            spreadsheetPageMock.VerifyAddCell(2, 2, nameof(FakeModel.Name), default);
            spreadsheetPageMock.VerifyAddCell(2, 3, nameof(FakeModel.Display), default);
        }

        [Fact]
        public void SetsHeaderStyleWhenOptionsExtensionSpecified()
        {
            // Arrange
            var spreadsheetPageMock = new FakeWritingSpreadsheetPage();
            var resources = Array.Empty<FakeModel>();

            var idStyle = Style.Default;
            var idStyleName = "foo";
            var nameStyle = BuiltInExcelStyle.Bad;
            var displayStyle = BuiltInPackageStyle.Bold;

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(
                        model => model.Id,
                        options: new HeaderStyleMapOption(new(idStyleName, IndexerKeyKind.Custom), idStyle)),
                    PropertyMapCreator2.CreateForFakeModel(
                        model => model.Name,
                        options: new HeaderStyleMapOption(nameStyle.IndexerKey, nameStyle.Style)),
                    PropertyMapCreator2.CreateForFakeModel(
                        model => model.Display,
                        options: new HeaderStyleMapOption(displayStyle.IndexerKey, displayStyle.Style)),
                });

            var sut = CreateSystemUnderTest();

            // Act
            sut.Write(spreadsheetPageMock, resources, map);

            // Assert
            spreadsheetPageMock.VerifyAddCell(1, 1, nameof(FakeModel.Id), new IndexerKey(idStyleName, IndexerKeyKind.Custom));
            spreadsheetPageMock.VerifyAddCell(1, 2, nameof(FakeModel.Name), nameStyle.IndexerKey);
            spreadsheetPageMock.VerifyAddCell(1, 3, nameof(FakeModel.Display), displayStyle.IndexerKey);
        }

        [Fact]
        public void SetsBodyStyleWhenOptionsExtensionSpecified()
        {
            // Arrange
            var spreadsheetPageMock = new FakeWritingSpreadsheetPage();

            var resource = new FakeModel { Id = 1, Name = "One", Display = "Uno", };
            var resources = new[] { resource, };

            var idStyle = Style.Default;
            var idStyleName = "foo";
            var nameStyle = BuiltInExcelStyle.Bad;
            var displayStyle = BuiltInPackageStyle.Bold;

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(
                        model => model.Id,
                        options: new BodyStyleMapOption(new(idStyleName, IndexerKeyKind.Custom), idStyle)),
                    PropertyMapCreator2.CreateForFakeModel(
                        model => model.Name,
                        options: new BodyStyleMapOption(nameStyle.IndexerKey, nameStyle.Style)),
                    PropertyMapCreator2.CreateForFakeModel(
                        model => model.Display,
                        options: new BodyStyleMapOption(displayStyle.IndexerKey, displayStyle.Style)),
                });

            var idPropertyMap = map.Properties.Single(propertyMap => propertyMap.Key.Name == nameof(FakeModel.Id));
            var namePropertyMap = map.Properties.Single(propertyMap => propertyMap.Key.Name == nameof(FakeModel.Name));
            var displayPropertyMap = map.Properties.Single(propertyMap => propertyMap.Key.Name == nameof(FakeModel.Display));

            var idCellValue = new WritingCellValue(resource.Id);
            var nameCellValue = new WritingCellValue(resource.Name);
            var displayCellValue = new WritingCellValue(resource.Display);
            _mocker.GetMock<IResourcePropertySerializer>()
                .Setup(resourcePropertySerializer => resourcePropertySerializer.Serialize(resource, idPropertyMap))
                .Returns(idCellValue);
            _mocker.GetMock<IResourcePropertySerializer>()
                .Setup(resourcePropertySerializer => resourcePropertySerializer.Serialize(resource, namePropertyMap))
                .Returns(nameCellValue);
            _mocker.GetMock<IResourcePropertySerializer>()
                .Setup(resourcePropertySerializer => resourcePropertySerializer.Serialize(resource, displayPropertyMap))
                .Returns(displayCellValue);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Write(spreadsheetPageMock, resources, map);

            // Assert
            spreadsheetPageMock.VerifyAddCell(2, 1, idCellValue, new IndexerKey(idStyleName, IndexerKeyKind.Custom));
            spreadsheetPageMock.VerifyAddCell(2, 2, nameCellValue, nameStyle.IndexerKey);
            spreadsheetPageMock.VerifyAddCell(2, 3, displayCellValue, displayStyle.IndexerKey);
        }

        [Fact]
        public void TreatsNullResourceCollectionAsEmptyCollection()
        {
            // Arrange
            var spreadsheetPageMock = new FakeWritingSpreadsheetPage();
            var resources = default(IEnumerable<FakeModel>);
            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Display),
                });

            var sut = CreateSystemUnderTest();

            // Act
            sut.Write(spreadsheetPageMock, resources!, map);

            // Assert
            spreadsheetPageMock.VerifyAddCell(1, 1, nameof(FakeModel.Id), default);
            spreadsheetPageMock.VerifyAddCell(1, 2, nameof(FakeModel.Name), default);
            spreadsheetPageMock.VerifyAddCell(1, 3, nameof(FakeModel.Display), default);
        }

        [Fact]
        public void HandlesGapBetweenColumns()
        {
            // Arrange
            var spreadsheetPageMock = new FakeWritingSpreadsheetPage();
            var resources = default(IEnumerable<FakeModel>);
            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id, keyAction => keyAction.WithNumber(1U)),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name, keyAction => keyAction.WithNumber(3U)),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Display, keyAction => keyAction.WithNumber(5U)),
                });

            var sut = CreateSystemUnderTest();

            // Act
            sut.Write(spreadsheetPageMock, resources!, map);

            // Assert
            spreadsheetPageMock.VerifyAddCell(1, 1, nameof(FakeModel.Id), default);
            spreadsheetPageMock.VerifyAddCell(1, 3, nameof(FakeModel.Name), default);
            spreadsheetPageMock.VerifyAddCell(1, 5, nameof(FakeModel.Display), default);
        }
    }
}
