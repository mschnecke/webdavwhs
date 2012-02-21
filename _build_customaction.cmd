@echo off
set MSBUILD=%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe
set PRJFILE=_build.proj

"%MSBUILD%" "%PRJFILE%" /t:CustomAction
pause

