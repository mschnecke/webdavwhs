@echo off
set MSBUILD=%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
set PRJFILE=_build.proj

"%MSBUILD%" "%PRJFILE%" /t:CustomAction
pause

