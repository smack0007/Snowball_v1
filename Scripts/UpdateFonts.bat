@ECHO OFF

%~DP0\..\Snowball.Tools.TextureFontGenerator\bin\Debug\TextureFontGenerator.exe "Segoe UI" 12 /XmlFileName:%~DP0\..\Snowball\GameConsoleFont.xml
%~DP0\..\Snowball.Tools.TextureFontGenerator\bin\Debug\TextureFontGenerator.exe "Segoe UI" 12 /XmlFileName:%~DP0\..\Snowball.Demo\font.xml
%~DP0\..\Snowball.Tools.TextureFontGenerator\bin\Debug\TextureFontGenerator.exe "Segoe UI" 72 /XmlFileName:%~DP0\..\Samples\CustomGameWindowSample\CustomGameWindowFont.xml
%~DP0\..\Snowball.Tools.TextureFontGenerator\bin\Debug\TextureFontGenerator.exe "Segoe UI" 24 /XmlFileName:%~DP0\..\Samples\GamePadReader\GamePadReaderFont.xml
%~DP0\..\Snowball.Tools.TextureFontGenerator\bin\Debug\TextureFontGenerator.exe "Segoe UI" 24 /XmlFileName:%~DP0\..\Samples\TextBlockSample\TextBlockFont.xml