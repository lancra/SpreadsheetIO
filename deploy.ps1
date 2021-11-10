$StartLocation = Get-Location
Set-Location $PSScriptRoot

$Args = $Args -join ''
dotnet run --project 'tools\SpreadsheetIO.Tool.Deploy\LanceC.SpreadsheetIO.Tool.Deploy.csproj' -- $Args

Set-Location $StartLocation
