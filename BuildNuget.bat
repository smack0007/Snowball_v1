@ECHO OFF

msbuild "Snowball.sln" /property:Configuration="Release"

rmdir /s /q nuget
mkdir nuget
pushd nuget
..\.nuget\nuget pack ..\Snowball.nuspec
popd