// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Design",
    "CA1034:Nested types should not be visible",
    Justification = "Nested classes are used for methods in unit test classes.")]
[assembly: SuppressMessage(
    "Design",
    "CA1052:Static holder types should be Static or NotInheritable",
    Justification = "Test organization can result in this class structure.")]
[assembly: SuppressMessage(
    "Design",
    "CA1062:Validate arguments of public methods",
    Justification = "Test methods are not under the same constraints as typical public methods.")]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1506:Avoid excessive class coupling",
    Justification = "Test methods are not under the same constraints as typical public methods.")]
[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Allowed in test method names.")]
[assembly: SuppressMessage(
    "Reliability",
    "CA2007:Consider calling ConfigureAwait on the awaited task",
    Justification = "Test methods will not be called outside of the scope of the project.")]
[assembly: SuppressMessage(
    "Microsoft.CodeAnalysis.CSharp",
    "CS8604:Possible null reference argument.",
    Justification = "Necessary to add coverage for projects that do not use nullable reference types.")]
[assembly: SuppressMessage(
    "Microsoft.CodeAnalysis.CSharp",
    "CS8625:Cannot convert null literal to non-nullable reference type.",
    Justification = "Necessary to add coverage for projects that do not use nullable reference types.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.SpecialRules",
    "SA0001:XML comment analysis is disabled due to project configuration",
    Justification = "XML documentation is not required for test projects.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1118:Parameter should not span multiple lines",
    Justification = "This situation is hard to avoid with complex theory data.")]
