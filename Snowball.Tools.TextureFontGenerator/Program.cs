using System;
using Snowball.Tools.Utilities;
using System.IO;

namespace Snowball.Tools.TextureFontGenerator
{
	public static class Program
	{
		public static int Main(string[] args)
		{
			var optionsParser = new CommandLineOptionsParser<TextureFontGeneratorOptions>();
			TextureFontGeneratorOptions options;
						
			if (optionsParser.Parse(args, out options))
			{
				if (options.XmlFileName == null && options.ImageFileName == null)
				{
					options.XmlFileName = "font.xml";
					options.ImageFileName = "font.png";
				}
				else if (options.ImageFileName == null)
				{
					options.ImageFileName = Path.Combine(Path.GetDirectoryName(options.XmlFileName), Path.GetFileNameWithoutExtension(options.XmlFileName) + ".png");
				}
				else if (options.XmlFileName == null)
				{
					options.XmlFileName = Path.Combine(Path.GetDirectoryName(options.XmlFileName), Path.GetFileNameWithoutExtension(options.XmlFileName) + ".xml");
				}

				if (!ColorHelper.IsValidHexString(options.BackgroundColor))
				{
					Console.Error.WriteLine("Please provide a valid hex color string for BackgroundColor.");
					return 2;
				}

				TextureFontGenerator.Generate(options);
				return 0;
			}

			return 1;
		}
	}
}
