@echo off
REM Restore NuGet packages
nuget restore pseudocodeIde.sln
if %errorlevel% neq 0 exit /b %errorlevel%

REM Build solution
msbuild pseudocodeIde.sln -t:rebuild -property:Configuration=Release
if %errorlevel% neq 0 exit /b %errorlevel%

REM Archive files (only if notzip parameter isn't provided)
if not "%1"=="nozip" (
    REM Delete existing zip (ignoring potential errors)
    del "pseudocode-ide.zip"
    REM Create new archive
    7z a pseudocode-ide.zip .\bin\Release\*
    if %errorlevel% neq 0 exit /b %errorlevel%
)