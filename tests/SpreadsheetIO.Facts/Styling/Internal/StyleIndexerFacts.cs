using System;
using System.Collections.Generic;
using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal
{
    public class StyleIndexerFacts
    {
        private static readonly IndexerKey Key = new IndexerKey("Style", IndexerKeyKind.Custom);

        private static readonly Style Style =
            new Style(
                new Border(new BorderLine(Color.White, BorderLineKind.Thick)),
                new Fill(FillKind.Solid, Color.Black),
                new Font("Arial", 20D, Color.White, true, true));

        private readonly AutoMocker _mocker = new AutoMocker();

        private StyleIndexer CreateSystemUnderTest()
            => _mocker.CreateInstance<StyleIndexer>();

        public class TheKeysProperty : StyleIndexerFacts
        {
            [Fact]
            public void ReturnsIndexedKeys()
            {
                // Arrange
                var defaultKey = BuiltInExcelStyle.Normal.IndexerKey;
                var defaultStyle = BuiltInExcelStyle.Normal.Style;

                var sut = CreateSystemUnderTest();
                sut.Add(defaultKey, defaultStyle);
                sut.Add(Key, Style);

                // Act
                var keys = sut.Keys;

                // Assert
                Assert.Equal(2, keys.Count);
                Assert.Single(keys, key => key == defaultKey);
                Assert.Single(keys, key => key == Key);
            }
        }

        public class TheKeyIndexer : StyleIndexerFacts
        {
            [Fact]
            public void ReturnsIndexedResourceForKey()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(Key, Style);

                // Act
                var actualIndexedResource = sut[Key];

                // Assert
                Assert.Equal(Style, actualIndexedResource.Resource);
                Assert.Equal(expectedIndex, actualIndexedResource.Index);
            }

            [Fact]
            public void ThrowsKeyNotFoundExceptionWhenStyleHasNotBeenIndexed()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut[Key]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<KeyNotFoundException>(exception);
            }
        }

        public class TheAddMethod : StyleIndexerFacts
        {
            [Fact]
            public void AddsBorderToIndexer()
            {
                // Arrange
                var borderIndexerMock = _mocker.GetMock<IBorderIndexer>();
                borderIndexerMock.Setup(borderIndexer => borderIndexer.Add(Style.Border))
                    .Verifiable();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Add(Key, Style);

                // Assert
                borderIndexerMock.Verify();
            }

            [Fact]
            public void AddsFillToIndexer()
            {
                // Arrange
                var fillIndexerMock = _mocker.GetMock<IFillIndexer>();
                fillIndexerMock.Setup(fillIndexer => fillIndexer.Add(Style.Fill))
                    .Verifiable();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Add(Key, Style);

                // Assert
                fillIndexerMock.Verify();
            }

            [Fact]
            public void AddsFontToIndexer()
            {
                // Arrange
                var fontIndexerMock = _mocker.GetMock<IFontIndexer>();
                fontIndexerMock.Setup(fontIndexer => fontIndexer.Add(Style.Font))
                    .Verifiable();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Add(Key, Style);

                // Assert
                fontIndexerMock.Verify();
            }

            [Fact]
            public void SkipsIndexingWhenStyleIsAlreadyIndexedForKey()
            {
                // Arrange
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(Key, Style);

                // Act
                var actualIndex = sut.Add(Key, Style);

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }

            [Fact]
            public void SkipsIndexingWhenStyleIsAlreadyIndexedForDifferentKey()
            {
                // Arrange
                var differentKey = new IndexerKey("Different Style", IndexerKeyKind.Custom);
                var sut = CreateSystemUnderTest();
                var expectedIndex = sut.Add(differentKey, Style);

                // Act
                var actualIndex = sut.Add(Key, Style);

                // Assert
                Assert.Equal(expectedIndex, actualIndex);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenDifferentStyleIsAlreadyIndexedForKey()
            {
                // Arrange
                var differentStyle = new Style(
                    Border.Default,
                    new Fill(FillKind.Solid, Color.Brown),
                    Font.Default);
                var sut = CreateSystemUnderTest();
                sut.Add(Key, differentStyle);

                // Act
                var exception = Record.Exception(() => sut.Add(Key, Style));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }
        }

        public class TheClearMethod : StyleIndexerFacts
        {
            [Fact]
            public void ClearsBorderIndexer()
            {
                // Arrange
                var borderIndexerMock = _mocker.GetMock<IBorderIndexer>();
                borderIndexerMock.Setup(borderIndexer => borderIndexer.Clear())
                    .Verifiable();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Clear();

                // Assert
                borderIndexerMock.Verify();
            }

            [Fact]
            public void ClearsFillIndexer()
            {
                // Arrange
                var fillIndexerMock = _mocker.GetMock<IFillIndexer>();
                fillIndexerMock.Setup(fillIndexer => fillIndexer.Clear())
                    .Verifiable();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Clear();

                // Assert
                fillIndexerMock.Verify();
            }

            [Fact]
            public void ClearsFontIndexer()
            {
                // Arrange
                var fontIndexerMock = _mocker.GetMock<IFontIndexer>();
                fontIndexerMock.Setup(fontIndexer => fontIndexer.Clear())
                    .Verifiable();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Clear();

                // Assert
                fontIndexerMock.Verify();
            }

            [Fact]
            public void ClearsNonDefaultStyles()
            {
                // Arrange
                var defaultKey = new IndexerKey(BuiltInExcelStyle.Normal.Name, IndexerKeyKind.Excel);

                var sut = CreateSystemUnderTest();
                sut.Add(Key, Style);

                // Act
                sut.Clear();
                var defaultException = Record.Exception(() => sut[defaultKey]);
                var nonDefaultException = Record.Exception(() => sut[Key]);

                // Assert
                Assert.Null(defaultException);
                Assert.NotNull(nonDefaultException);
                Assert.IsType<KeyNotFoundException>(nonDefaultException);
            }
        }
    }
}
