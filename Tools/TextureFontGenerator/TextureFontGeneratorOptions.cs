using System;
using Snowball.Tools.Utilities;

namespace Snowball.Tools.TextureFontGenerator
{
	public class TextureFontGeneratorOptions
	{
		[CommandLineOptionRequired(0)]
		[CommandLineOptionDescription("The name of the font to be used.")]
		public string FontName;

		[CommandLineOptionRequired(1)]
		[CommandLineOptionDescription("The size of the font.")]
		public int FontSize;
				
		[CommandLineOptionDescription("The file name to use for the xml file.")]
		public string XmlFileName;

		[CommandLineOptionDescription("The file name to use for the image file.")]
		public string ImageFileName;

		[CommandLineOptionDescription("The decimal value of the first char to be used. Default is 32.")]
		public int MinChar = 32;

		[CommandLineOptionDescription("The decimal value of the last char to be used. Default is 127.")]
		public int MaxChar = 127;

		[CommandLineOptionDescription("Specifies antialiasing should be used.")]
		public bool Antialias = true;

		[CommandLineOptionDescription("Hex color string (RGBA) representing the background color to be used.")]
		public string BackgroundColor = "000000FF";

		[CommandLineOptionDescription("The default amount of spacing to place between characters when rendering text.")]
		public int CharacterSpacing = 2;

		[CommandLineOptionDescription("The default amount of spacing to place between lines when rendering text.")]
		public int LineSpacing = 2;
	}
}
