@ECHO OFF

msbuild "Snowball.sln" /property:Configuration=Release
msbuild Snowball.shfbproj