SpreadsheetIO
===

[nuget]: https://www.nuget.org/packages/LanceC.SpreadsheetIO/

This package is used to read and write to an Excel spreadsheet through model mappings. Writing includes support for styling and formatting cells.

## Install

Install the [NuGet package][nuget] into your project.

```
PM> Install-Package LanceC.SpreadsheetIO
```
```
$ dotnet add package LanceC.SpreadsheetIO
```

## Usage

- Register the package for dependency injection using `IServiceCollection.AddSpreadsheetIO()`.
- Create a model mapping by inheriting from `ResourceMap<TResource>`.
- Resolve an instance of `ISpreadsheetFactory` to create or open a spreadsheet.
- Use the resulting spreadsheet objects to interact with the spreadsheet.
