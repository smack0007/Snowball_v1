@ECHO OFF

ECHO Cleaning Build directory...
rmdir /s /q build

ECHO Building Snowball.sln...
msbuild "Snowball.sln" /nologo /verbosity:quiet /target:clean
msbuild "Snowball.sln" /nologo /verbosity:quiet /property:Configuration=Release /property:Platform="Any CPU" /property:OutputPath=.\..\build /property:WarningLevel=2

IF ERRORLEVEL 0 GOTO BuildSamples

ECHO ERROR: Failed while building Snowball.sln.
GOTO END

:BuildSamples

REM mkdir ./build/Samples

REM ECHO Building Samples\SnowballSamples.sln...
REM msbuild ".\Samples\SnowballSamples.sln" /nologo /verbosity:quiet /target:clean
REM msbuild ".\Samples\SnowballSamples.sln" /nologo /verbosity:quiet /property:Configuration=Release /property:Platform="Any CPU" /property:OutputPath=.\..\..\build /property:WarningLevel=2

IF ERRORLEVEL 0 GOTO END

ECHO ERROR: Failed while building Samples\SnowballSamples.sln.
GOTO END

:END