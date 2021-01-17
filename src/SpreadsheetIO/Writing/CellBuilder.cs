using System;
using LanceC.SpreadsheetIO.Writing.Internal;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Provides cell value builder initializations.
    /// </summary>
    public static class CellBuilder
    {
        private static ICellStringValueBuilder DefaultBuilder
            => new CellBuilderImpl(new OpenXml.Cell());

        /// <summary>
        /// Creates a cell builder with an integer value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(int value)
            => new CellBuilderImpl(
                new OpenXml.Cell
                {
                    CellValue = new OpenXml.CellValue(value),
                });

        /// <summary>
        /// Creates a cell builder with a nullable integer value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(int? value)
            => value.HasValue
            ? WithValue(value.Value)
            : DefaultBuilder;

        /// <summary>
        /// Creates a cell builder with a double value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(double value)
            => new CellBuilderImpl(
                new OpenXml.Cell
                {
                    CellValue = new OpenXml.CellValue(value),
                });

        /// <summary>
        /// Creates a cell builder with a nullable double value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(double? value)
            => value.HasValue
            ? WithValue(value.Value)
            : DefaultBuilder;

        /// <summary>
        /// Creates a cell builder with a boolean value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(bool value)
            => new CellBuilderImpl(
                new OpenXml.Cell
                {
                    CellValue = new OpenXml.CellValue(Convert.ToInt32(value)),
                    DataType = OpenXml.CellValues.Boolean,
                });

        /// <summary>
        /// Creates a cell builder with a nullable boolean value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(bool? value)
            => value.HasValue
            ? WithValue(value.Value)
            : DefaultBuilder;

        /// <summary>
        /// Creates a cell builder with a decimal value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(decimal value)
            => new CellBuilderImpl(
                new OpenXml.Cell
                {
                    CellValue = new OpenXml.CellValue(value),
                });

        /// <summary>
        /// Creates a cell builder with a nullable decimal value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(decimal? value)
            => value.HasValue
            ? WithValue(value.Value)
            : DefaultBuilder;

        /// <summary>
        /// Creates a cell builder with a datetime value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(DateTime value)
            => new CellBuilderImpl(
                new OpenXml.Cell
                {
                    CellValue = new OpenXml.CellValue(value.ToOADate()),
                });

        /// <summary>
        /// Creates a cell builder with a nullable datetime value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(DateTime? value)
            => value.HasValue
            ? WithValue(value.Value)
            : DefaultBuilder;

        /// <summary>
        /// Creates a cell builder with a datetime offset value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(DateTimeOffset value)
            => WithValue(value.DateTime);

        /// <summary>
        /// Creates a cell builder with a nullable datetime offset value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellValueBuilder WithValue(DateTimeOffset? value)
            => value.HasValue
            ? WithValue(value.Value)
            : DefaultBuilder;

        /// <summary>
        /// Creates a cell builder with a character value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellStringValueBuilder WithValue(char value)
            => WithValue(value.ToString());

        /// <summary>
        /// Creates a cell builder with a nullable character value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellStringValueBuilder WithValue(char? value)
            => value.HasValue
            ? WithValue(value.ToString())
            : DefaultBuilder;

        /// <summary>
        /// Creates a cell builder with a string value.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <returns>The cell value builder.</returns>
        public static ICellStringValueBuilder WithValue(string? value)
            => !string.IsNullOrEmpty(value)
            ? new CellBuilderImpl(
                new OpenXml.Cell
                {
                    CellValue = new OpenXml.CellValue(value),
                    DataType = OpenXml.CellValues.SharedString,
                })
            : DefaultBuilder;
    }
}
