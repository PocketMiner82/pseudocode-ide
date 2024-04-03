@echo off
nuget restore pseudocodeIde.sln
msbuild pseudocodeIde.sln -t:rebuild -property:Configuration=Release
if not "%1"=="nozip" (
    del "pseudocode-ide.zip"
    7z a pseudocode-ide.zip .\bin\Release\*
)