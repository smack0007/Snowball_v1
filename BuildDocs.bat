@ECHO OFF

msbuild "Source\Snowball.sln" /property:Configuration=Release
msbuild "Source\Snowball.shfbproj"