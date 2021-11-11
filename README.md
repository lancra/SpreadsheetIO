SpreadsheetIO
===
[nuget]: https://www.nuget.org/packages/LanceC.SpreadsheetIO/

This package is used to read and write to an Excel spreadsheet through model mappings. Writing includes support for styling and formatting cells.

## Install
Install the [NuGet package][nuget] into your project.
``` shell
PM> Install-Package LanceC.SpreadsheetIO
```
``` shell
$ dotnet add package LanceC.SpreadsheetIO
```

## Usage
### Mapping
``` c#
public class Model
{
    public int Id { get; set; }
    
    public string Name { get; set; }
}

public class ModelMap : ResourceMap<Model>
{
    public ModelMap()
    {
        Map(model => model.Id);
        Map(model => model.Name);
    }
}
```

### Reading
``` c#
public void Read(string filePath)
{
    var provider = new ServiceCollection()
        .AddSpreadsheetIO()
        .AddScoped<IResourceMap, ModelMap>()
        .BuildServiceProvider();

    using var spreadsheetStream = new FileStream(
        filePath,
        FileMode.Open,
        FileAccess.Read,
        FileShare.ReadWrite);

    var spreadsheetFactory = provider.GetRequiredService<ISpreadsheetFactory>();
    var spreadsheet = spreadsheetFactory.OpenRead(spreadsheetStream);
    var spreadsheetPage = spreadsheet.Pages[0];

    var readingResult = spreadsheetPage.ReadAll<Model>();
}
```

### Writing
``` c#
public void Write(string filePath, IEnumerable<Model> models)
{
    var provider = new ServiceCollection()
        .AddSpreadsheetIO()
        .AddScoped<IResourceMap, ModelMap>()
        .BuildServiceProvider();

    using var spreadsheetStream = new FileStream(
        filePath,
        FileMode.Create,
        FileAccess.ReadWrite,
        FileShare.ReadWrite);

    var spreadsheetFactory = provider.GetRequiredService<ISpreadsheetFactory>();
    using var spreadsheet = spreadsheetFactory.Create(spreadsheetStream);

    using var spreadsheetPage = spreadsheet.WritePage<Model>("Sheet1", models);
}
```
