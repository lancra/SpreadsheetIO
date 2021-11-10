using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using LanceC.CommandLine.Help;
using LanceC.Tooling.DevOps;

namespace LanceC.SpreadsheetIO.Tool.Build;

public static class Program
{
    public static async Task<int> Main(string[] args)
        => await new CommandLineBuilder(new BuildCommand())
        .UseDefaults()
        .UseHelpBuilder(context => new CommandTreeHelpBuilder(context.Console))
        .UseHost(host => host
            .ConfigureServices(
                (hostingContext, services) => services
                    .AddBuildTooling(
                        options =>
                        {
                            options.AddTestProject("tests/SpreadsheetIO.Facts", BuiltInTestSuites.Unit);
                            options.AddTestProject("tests/SpreadsheetIO.Tests", BuiltInTestSuites.Functional);
                            options.AddPackProject("src/SpreadsheetIO");
                        }))
            .UseBuildCommand())
        .Build()
        .InvokeAsync(args)
        .ConfigureAwait(false);
}
