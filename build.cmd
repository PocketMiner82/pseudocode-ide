@echo off
nuget restore pseudocodeIde.sln
msbuild pseudocodeIde.sln -t:rebuild -property:Configuration=Release
call .\create_release_zip.cmd