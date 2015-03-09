@ECHO OFF

ECHO Cleaning Build directory...
rmdir /s /q build

ECHO Building Snowball.sln...
msbuild ".\Source\Snowball.sln" /nologo /verbosity:quiet /target:clean
msbuild ".\Source\Snowball.sln" /nologo /verbosity:quiet /property:Configuration=Release /property:Platform="Any CPU" /property:OutputPath=.\..\..\..\build /property:WarningLevel=2

IF ERRORLEVEL 0 GOTO END

ECHO ERROR: Failed while building Snowball.sln.
GOTO END

:END