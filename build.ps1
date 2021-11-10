$StartLocation = Get-Location
Set-Location $PSScriptRoot

$Args = $Args -join ''
dotnet run --project 'tools\SpreadsheetIO.Tool.Build\LanceC.SpreadsheetIO.Tool.Build.csproj' -- $Args

Set-Location $StartLocation
