@echo off
del "pseudocode-ide.zip"
"C:\Program Files\7-Zip\7z.exe" a pseudocode-ide.zip .\bin\Release\*
pause