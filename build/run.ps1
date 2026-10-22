$ScriptDir = Split-Path $MyInvocation.MyCommand.Path -Parent

Set-Location "$ScriptDir\.."
dotnet run --project "$ScriptDir\..\src\Build" -- $args

exit $LASTEXITCODE
