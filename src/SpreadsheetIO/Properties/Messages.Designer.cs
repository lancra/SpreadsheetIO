// <auto-generated />

using System.Resources;

#nullable enable

namespace LanceC.SpreadsheetIO.Properties
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Messages
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("LanceC.SpreadsheetIO.Properties.Messages", typeof(Messages).Assembly);

        /// <summary>
        /// Another resource is already indexed for {key}.
        /// </summary>
        public static string AlreadyIndexedResourceForKey(object? key)
            => string.Format(
                GetString("AlreadyIndexedResourceForKey", nameof(key)),
                key);

        /// <summary>
        /// Cannot to advance backwards from column {currentColumnNumber} to {columnNumber}.
        /// </summary>
        public static string CannotAdvanceColumnsBackwards(object? currentColumnNumber, object? columnNumber)
            => string.Format(
                GetString("CannotAdvanceColumnsBackwards", nameof(currentColumnNumber), nameof(columnNumber)),
                currentColumnNumber, columnNumber);

        /// <summary>
        /// Cannot to advance backwards from row {currentRowNumber} to {rowNumber}.
        /// </summary>
        public static string CannotAdvanceRowsBackwards(object? currentRowNumber, object? rowNumber)
            => string.Format(
                GetString("CannotAdvanceRowsBackwards", nameof(currentRowNumber), nameof(rowNumber)),
                currentRowNumber, rowNumber);

        /// <summary>
        /// Cannot advance zero columns.
        /// </summary>
        public static string CannotAdvanceZeroColumns
            => GetString("CannotAdvanceZeroColumns");

        /// <summary>
        /// Cannot advance zero rows.
        /// </summary>
        public static string CannotAdvanceZeroRows
            => GetString("CannotAdvanceZeroRows");

        /// <summary>
        /// Cannot retrieve assembly for {type}.
        /// </summary>
        public static string CannotGetAssemblyFromType(object? type)
            => string.Format(
                GetString("CannotGetAssemblyFromType", nameof(type)),
                type);

        /// <summary>
        /// {resourceType} has duplicate key names: {keyNames}.
        /// </summary>
        public static string DuplicatePropertyMapKeyNames(object? resourceType, object? keyNames)
            => string.Format(
                GetString("DuplicatePropertyMapKeyNames", nameof(resourceType), nameof(keyNames)),
                resourceType, keyNames);

        /// <summary>
        /// {resourceType} has duplicate key numbers: {keyNumbers}.
        /// </summary>
        public static string DuplicatePropertyMapKeyNumbers(object? resourceType, object? keyNumbers)
            => string.Format(
                GetString("DuplicatePropertyMapKeyNumbers", nameof(resourceType), nameof(keyNumbers)),
                resourceType, keyNumbers);

        /// <summary>
        /// {resourceType} has duplicate maps for the following properties: {propertyNames}.
        /// </summary>
        public static string DuplicatePropertyMaps(object? resourceType, object? propertyNames)
            => string.Format(
                GetString("DuplicatePropertyMaps", nameof(resourceType), nameof(propertyNames)),
                resourceType, propertyNames);

        /// <summary>
        /// A spreadsheet page has already been added with the name '{name}'.
        /// </summary>
        public static string DuplicateSpreadsheetPageForName(object? name)
            => string.Format(
                GetString("DuplicateSpreadsheetPageForName", nameof(name)),
                name);

        /// <summary>
        /// The {parentName} cannot be constructed because there are multiple {strategyType} instances registered for the {propertyType} property type.
        /// </summary>
        public static string DuplicateStrategy(object? parentName, object? strategyType, object? propertyType)
            => string.Format(
                GetString("DuplicateStrategy", nameof(parentName), nameof(strategyType), nameof(propertyType)),
                parentName, strategyType, propertyType);

        /// <summary>
        /// This operation cannot be performed since writing has been closed.
        /// </summary>
        public static string FinishedWriting
            => GetString("FinishedWriting");

        /// <summary>
        /// The cell reference '{cellReference}' is not valid.
        /// </summary>
        public static string InvalidCellReference(object? cellReference)
            => string.Format(
                GetString("InvalidCellReference", nameof(cellReference)),
                cellReference);

        /// <summary>
        /// The cell reference row number '{rowNumberText}' is not valid.
        /// </summary>
        public static string InvalidCellReferenceRowNumber(object? rowNumberText)
            => string.Format(
                GetString("InvalidCellReferenceRowNumber", nameof(rowNumberText)),
                rowNumberText);

        /// <summary>
        /// Cannot retrieve {value} from the unrecognized element type.
        /// </summary>
        public static string InvalidElementForValue(object? value)
            => string.Format(
                GetString("InvalidElementForValue", nameof(value)),
                value);

        /// <summary>
        /// The provided Excel date is not valid.
        /// </summary>
        public static string InvalidExcelDate
            => GetString("InvalidExcelDate");

        /// <summary>
        /// The {styleName} Excel style has not been set up with an identifier. Please contact the package maintainer.
        /// </summary>
        public static string InvalidExcelStyleSetup(object? styleName)
            => string.Format(
                GetString("InvalidExcelStyleSetup", nameof(styleName)),
                styleName);

        /// <summary>
        /// The {resourceType} map has configuration errors and cannot be used.
        /// </summary>
        public static string InvalidMapForResourceType(object? resourceType)
            => string.Format(
                GetString("InvalidMapForResourceType", nameof(resourceType)),
                resourceType);

        /// <summary>
        /// The {resourceType} map does not specify a constructor and the following properties do not have public setters: {propertyNames}.
        /// </summary>
        public static string InvalidPropertiesForSetterCreation(object? resourceType, object? propertyNames)
            => string.Format(
                GetString("InvalidPropertiesForSetterCreation", nameof(resourceType), nameof(propertyNames)),
                resourceType, propertyNames);

        /// <summary>
        /// The provided expression does not represent a resource property.
        /// </summary>
        public static string InvalidResourcePropertyExpression
            => GetString("InvalidResourcePropertyExpression");

        /// <summary>
        /// A null or empty string cannot be indexed.
        /// </summary>
        public static string InvalidStringForIndexing
            => GetString("InvalidStringForIndexing");

        /// <summary>
        /// The {registrationType} option registration is not allowed for a {propertyType} property.
        /// </summary>
        public static string MapOptionRegistrationNotAllowedForType(object? registrationType, object? propertyType)
            => string.Format(
                GetString("MapOptionRegistrationNotAllowedForType", nameof(registrationType), nameof(propertyType)),
                registrationType, propertyType);

        /// <summary>
        /// No header was found at column {columnNumber}.
        /// </summary>
        public static string MissingHeaderForColumnNumber(object? columnNumber)
            => string.Format(
                GetString("MissingHeaderForColumnNumber", nameof(columnNumber)),
                columnNumber);

        /// <summary>
        /// The {resourceType} map has properties that were specified as constructor parameters but do not have a defined map: {propertyNames}.
        /// </summary>
        public static string MissingMapForResourceProperties(object? resourceType, object? propertyNames)
            => string.Format(
                GetString("MissingMapForResourceProperties", nameof(resourceType), nameof(propertyNames)),
                resourceType, propertyNames);

        /// <summary>
        /// No map is defined for the {resourceType} resource.
        /// </summary>
        public static string MissingMapForResourceType(object? resourceType)
            => string.Format(
                GetString("MissingMapForResourceType", nameof(resourceType)),
                resourceType);

        /// <summary>
        /// The {resourceType} constructor could not be found.
        /// </summary>
        public static string MissingResourceConstructor(object? resourceType)
            => string.Format(
                GetString("MissingResourceConstructor", nameof(resourceType)),
                resourceType);

        /// <summary>
        /// The index does not match a valid resource.
        /// </summary>
        public static string MissingResourceForIndex
            => GetString("MissingResourceForIndex");

        /// <summary>
        /// The {propertyName} property is designated as a constructor parameter but is not defined as a resource property.
        /// </summary>
        public static string MissingResourcePropertyForConstructorParameter(object? propertyName)
            => string.Format(
                GetString("MissingResourcePropertyForConstructorParameter", nameof(propertyName)),
                propertyName);

        /// <summary>
        /// No spreadsheet page was found for the name '{name}'.
        /// </summary>
        public static string MissingSpreadsheetPageForName(object? name)
            => string.Format(
                GetString("MissingSpreadsheetPageForName", nameof(name)),
                name);

        /// <summary>
        /// No {strategyType} was defined for the {propertyType} property type.
        /// </summary>
        public static string MissingStrategy(object? strategyType, object? propertyType)
            => string.Format(
                GetString("MissingStrategy", nameof(strategyType), nameof(propertyType)),
                strategyType, propertyType);

        /// <summary>
        /// Required input {parameterName} was not alphabetic.
        /// </summary>
        public static string NonAlphabeticInput(object? parameterName)
            => string.Format(
                GetString("NonAlphabeticInput", nameof(parameterName)),
                parameterName);

        /// <summary>
        /// Required input {parameterName} was not alphanumeric.
        /// </summary>
        public static string NonAlphanumericInput(object? parameterName)
            => string.Format(
                GetString("NonAlphanumericInput", nameof(parameterName)),
                parameterName);

        /// <summary>
        /// Required input {parameterName} was not numeric.
        /// </summary>
        public static string NonNumericInput(object? parameterName)
            => string.Format(
                GetString("NonNumericInput", nameof(parameterName)),
                parameterName);

        /// <summary>
        /// The resource has not been indexed.
        /// </summary>
        public static string UnindexedResource
            => GetString("UnindexedResource");

        /// <summary>
        /// No resource is indexed for {key}.
        /// </summary>
        public static string UnindexedResourceForKey(object? key)
            => string.Format(
                GetString("UnindexedResourceForKey", nameof(key)),
                key);

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name)!;
            for (var i = 0; i < formatterNames.Length; i++)
            {
                value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
            }

            return value;
        }
    }
}


