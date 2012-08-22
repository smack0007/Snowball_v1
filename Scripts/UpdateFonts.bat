@ECHO OFF

SET Program = %~DP0\..\Tools\TextureFontGenerator\bin\Release\TextureFontGenerator.exe

%~DP0\..\Tools\TextureFontGenerator\bin\Release\TextureFontGenerator.exe "Segoe UI" 12 /XmlFileName:%~DP0\..\Snowball\Graphics\BasicTextureFont.xml
%~DP0\..\Tools\TextureFontGenerator\bin\Release\TextureFontGenerator.exe "Segoe UI" 12 /XmlFileName:%~DP0\..\Snowball\UI\Font.xml
%~DP0\..\Tools\TextureFontGenerator\bin\Release\TextureFontGenerator.exe "Segoe UI" 12 /XmlFileName:%~DP0\..\Snowball.Demo\font.xml
%~DP0\..\Tools\TextureFontGenerator\bin\Release\TextureFontGenerator.exe "Segoe UI" 72 /XmlFileName:%~DP0\..\Samples\CustomGameWindowSample\CustomGameWindowFont.xml
%~DP0\..\Tools\TextureFontGenerator\bin\Release\TextureFontGenerator.exe "Segoe UI" 24 /XmlFileName:%~DP0\..\Samples\GamePadReader\GamePadReaderFont.xml