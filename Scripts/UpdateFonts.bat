@ECHO OFF

%~DP0\..\Snowball.Tools.CommandLine\bin\Debug\SnowballTools.exe GenerateTextureFont "Segoe UI" 12 /XmlFileName:%~DP0\..\Snowball\GameConsoleFont.xml
%~DP0\..\Snowball.Tools.CommandLine\bin\Debug\SnowballTools.exe GenerateTextureFont "Segoe UI" 12 /XmlFileName:%~DP0\..\Snowball.Demo\font.xml
%~DP0\..\Snowball.Tools.CommandLine\bin\Debug\SnowballTools.exe GenerateTextureFont "Segoe UI" 72 /XmlFileName:%~DP0\..\Samples\CustomGameWindowSample\CustomGameWindowFont.xml
%~DP0\..\Snowball.Tools.CommandLine\bin\Debug\SnowballTools.exe GenerateTextureFont "Segoe UI" 24 /XmlFileName:%~DP0\..\Samples\GamePadReader\GamePadReaderFont.xml
%~DP0\..\Snowball.Tools.CommandLine\bin\Debug\SnowballTools.exe GenerateTextureFont "Segoe UI" 24 /XmlFileName:%~DP0\..\Samples\TextBlockSample\TextBlockFont.xml