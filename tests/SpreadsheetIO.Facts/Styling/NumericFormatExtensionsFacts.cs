using System;
using LanceC.SpreadsheetIO.Styling;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Styling
{
    public class NumericFormatExtensionsFacts
    {
        public class TheToNumericFormatMethodWithNumberNumericFormatParameter : NumericFormatExtensionsFacts
        {
            public static TheoryData<NumberNumericFormat, string> DataForReturnsNumericFormatWithEquivalentFormatCode
                => new()
                {
                    {
                        new NumberNumericFormat(0, false, NegativeNumericFormatKind.Default),
                        "0"
                    },
                    {
                        new NumberNumericFormat(0, true, NegativeNumericFormatKind.Default),
                        "#,##0"
                    },
                    {
                        new NumberNumericFormat(2, false, NegativeNumericFormatKind.Default),
                        "0.00"
                    },
                    {
                        new NumberNumericFormat(4, true, NegativeNumericFormatKind.Default),
                        "#,##0.0000"
                    },
                    {
                        new NumberNumericFormat(4, true, NegativeNumericFormatKind.Red),
                        "#,##0.0000;[Red]#,##0.0000"
                    },
                    {
                        new NumberNumericFormat(4, true, NegativeNumericFormatKind.Parentheses),
                        @"#,##0.0000_);\(#,##0.0000\)"
                    },
                    {
                        new NumberNumericFormat(4, true, NegativeNumericFormatKind.RedParentheses),
                        @"#,##0.0000_);[Red]\(#,##0.0000\)"
                    },
                };

            [Theory]
            [MemberData(nameof(DataForReturnsNumericFormatWithEquivalentFormatCode))]
            public void ReturnsNumericFormatWithEquivalentFormatCode(
                NumberNumericFormat numberNumericFormat,
                string expectedFormatCode)
            {
                // Act
                var numericFormat = numberNumericFormat.ToNumericFormat();

                // Assert
                Assert.Equal(expectedFormatCode, numericFormat.Code);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenNumberNumericFormatIsNull()
            {
                // Arrange
                var numberNumericFormat = default(NumberNumericFormat?);

                // Act
                var exception = Record.Exception(() => numberNumericFormat!.ToNumericFormat());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheToNumericFormatMethodWithCurrencyNumericFormatParameter : NumericFormatExtensionsFacts
        {
            public static TheoryData<CurrencyNumericFormat, string> DataForReturnsNumericFormatWithEquivalentFormatCode
                => new()
                {
                    {
                        new CurrencyNumericFormat(0, string.Empty, NegativeNumericFormatKind.Default),
                        "#,##0"
                    },
                    {
                        new CurrencyNumericFormat(2, string.Empty, NegativeNumericFormatKind.Default),
                        "#,##0.00"
                    },
                    {
                        new CurrencyNumericFormat(2, "$", NegativeNumericFormatKind.Default),
                        "\"$\"#,##0.00"
                    },
                    {
                        new CurrencyNumericFormat(4, "USD", NegativeNumericFormatKind.Default),
                        @"[$USD]\ #,##0.0000"
                    },
                    {
                        new CurrencyNumericFormat(4, "USD", NegativeNumericFormatKind.Red),
                        @"[$USD]\ #,##0.0000;[Red][$USD]\ #,##0.0000"
                    },
                    {
                        new CurrencyNumericFormat(4, "USD", NegativeNumericFormatKind.Parentheses),
                        @"[$USD]\ #,##0.0000_);\([$USD]\ #,##0.0000\)"
                    },
                    {
                        new CurrencyNumericFormat(4, "USD", NegativeNumericFormatKind.RedParentheses),
                        @"[$USD]\ #,##0.0000_);[Red]\([$USD]\ #,##0.0000\)"
                    },
                };

            [Theory]
            [MemberData(nameof(DataForReturnsNumericFormatWithEquivalentFormatCode))]
            public void ReturnsNumericFormatWithEquivalentFormatCode(
                CurrencyNumericFormat currencyNumericFormat,
                string expectedFormatCode)
            {
                // Act
                var numericFormat = currencyNumericFormat.ToNumericFormat();

                // Assert
                Assert.Equal(expectedFormatCode, numericFormat.Code);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenCurrencyNumericFormatIsNull()
            {
                // Arrange
                var currencyNumericFormat = default(CurrencyNumericFormat?);

                // Act
                var exception = Record.Exception(() => currencyNumericFormat!.ToNumericFormat());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheToNumericFormatMethodWithAccountingNumericFormatParameter : NumericFormatExtensionsFacts
        {
            public static TheoryData<AccountingNumericFormat, string> DataForReturnsNumericFormatWithEquivalentFormatCode
                => new()
                {
                    {
                        new AccountingNumericFormat(0, string.Empty),
                        "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)"
                    },
                    {
                        new AccountingNumericFormat(2, string.Empty),
                        "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)"
                    },
                    {
                        new AccountingNumericFormat(2, "$"),
                        "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)"
                    },
                    {
                        new AccountingNumericFormat(4, "USD"),
                        "_([$USD]\\ * #,##0.0000_);_([$USD]\\ * \\(#,##0.0000\\);_([$USD]\\ * \"-\"????_);_(@_)"
                    },
                };

            [Theory]
            [MemberData(nameof(DataForReturnsNumericFormatWithEquivalentFormatCode))]
            public void ReturnsNumericFormatWithEquivalentFormatCode(
                AccountingNumericFormat accountingNumericFormat,
                string expectedFormatCode)
            {
                // Act
                var numericFormat = accountingNumericFormat.ToNumericFormat();

                // Assert
                Assert.Equal(expectedFormatCode, numericFormat.Code);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenAccountingNumericFormatIsNull()
            {
                // Arrange
                var accountingNumericFormat = default(AccountingNumericFormat?);

                // Act
                var exception = Record.Exception(() => accountingNumericFormat!.ToNumericFormat());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheToNumericFormatMethodWithTextNumericFormatParameter : NumericFormatExtensionsFacts
        {
            [Fact]
            public void ReturnsNumericFormatWithEquivalentFormatCode()
            {
                // Arrange
                var textNumericFormat = new TextNumericFormat();

                // Act
                var numericFormat = textNumericFormat.ToNumericFormat();

                // Assert
                Assert.Equal("@", numericFormat.Code);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenTextNumericFormatIsNull()
            {
                // Arrange
                var textNumericFormat = default(TextNumericFormat?);

                // Act
                var exception = Record.Exception(() => textNumericFormat!.ToNumericFormat());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }
    }
}
