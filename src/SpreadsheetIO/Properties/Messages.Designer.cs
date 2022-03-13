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
        /// Multiple maps are defined for the {resourceType} resource.
        /// </summary>
        public static string DuplicateMapForResourceType(object? resourceType)
            => string.Format(
                GetString("DuplicateMapForResourceType", nameof(resourceType)),
                resourceType);

        /// <summary>
        /// The {propertyName} property has already been mapped for the {resourceMapType} resource map.
        /// </summary>
        public static string DuplicatePropertyMap(object? propertyName, object? resourceMapType)
            => string.Format(
                GetString("DuplicatePropertyMap", nameof(propertyName), nameof(resourceMapType)),
                propertyName, resourceMapType);

        /// <summary>
        /// The {keyName} property key name has already been defined for the {resourceMapType} resource map.
        /// </summary>
        public static string DuplicatePropertyMapKeyName(object? keyName, object? resourceMapType)
            => string.Format(
                GetString("DuplicatePropertyMapKeyName", nameof(keyName), nameof(resourceMapType)),
                keyName, resourceMapType);

        /// <summary>
        /// The {keyNumber} property key number has already been defined for the {resourceMapType} resource map.
        /// </summary>
        public static string DuplicatePropertyMapKeyNumber(object? keyNumber, object? resourceMapType)
            => string.Format(
                GetString("DuplicatePropertyMapKeyNumber", nameof(keyNumber), nameof(resourceMapType)),
                keyNumber, resourceMapType);

        /// <summary>
        /// Multiple {resourceType} constructors are defined.
        /// </summary>
        public static string DuplicateResourceConstructor(object? resourceType)
            => string.Format(
                GetString("DuplicateResourceConstructor", nameof(resourceType)),
                resourceType);

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
        /// One or more validation failures occurred.
        /// </summary>
        public static string FailedResourceMapValidation
            => GetString("FailedResourceMapValidation");

        /// <summary>
        /// This operation cannot be performed since writing has been closed.
        /// </summary>
        public static string FinishedWriting
            => GetString("FinishedWriting");

        /// <summary>
        /// The map options are frozen and cannot be modified.
        /// </summary>
        public static string FrozenMapOptions
            => GetString("FrozenMapOptions");

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
        /// No header was found at column {columnNumber}.
        /// </summary>
        public static string MissingHeaderForColumnNumber(object? columnNumber)
            => string.Format(
                GetString("MissingHeaderForColumnNumber", nameof(columnNumber)),
                columnNumber);

        /// <summary>
        /// The {resourceMapType} map is not defined for the {resourceType} resource.
        /// </summary>
        public static string MissingMapForResourceMapType(object? resourceMapType, object? resourceType)
            => string.Format(
                GetString("MissingMapForResourceMapType", nameof(resourceMapType), nameof(resourceType)),
                resourceMapType, resourceType);

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
        /// The {extensionType} option is not allowed for a {propertyType} property.
        /// </summary>
        public static string OptionsExtensionNotAllowedForType(object? extensionType, object? propertyType)
            => string.Format(
                GetString("OptionsExtensionNotAllowedForType", nameof(extensionType), nameof(propertyType)),
                extensionType, propertyType);

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


