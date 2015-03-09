using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Snowball.Tools.Commands
{
    public class GenerateSpriteSheetXmlCommand : Command<GenerateSpriteSheetXmlCommand.Options>
    {
        public class Options
        {
            [OptionRequired(0)]
            [OptionDescription("The file name of the image of the SpriteSheet.")]
            public string ImageFileName;

            [OptionDescription("The file name to use for the output XML file.")]
            public string XmlFileName;
            
            [OptionDescription("Hex color string (RGBA) representing the grid color to be used.")]
            public string GridColor = "FF00FFFF";

            [OptionDescription("Hex color string (RGBA) representing the color key to be used when the SpriteSheet is loaded.")]
            public string ColorKey = "00000000";
        }

        public override string Description
        {
            get { return "Searches for frames in a SpriteSheet and writes an XML file containing the locations of the sprites frames."; }
        }

        public override bool EnsureOptions(Options options, ICommandLogger logger)
        {
            this.EnsureParamsAreNotNull(options, logger);

            if (string.IsNullOrWhiteSpace(options.ImageFileName))
            {
                logger.WriteError("ImageFileName is required.");
                return false;
            }

            if (options.XmlFileName == null)
            {
                options.XmlFileName = Path.Combine(Path.GetDirectoryName(options.ImageFileName), Path.GetFileNameWithoutExtension(options.ImageFileName) + ".xml");
            }

            if (!ColorHelper.IsValidHexString(options.GridColor))
            {
                logger.WriteError("GridColor must be a valid hex color string.");
                return false;
            }

            return true;
        }

        public override void Execute(Options options, ICommandLogger logger)
        {
            this.EnsureParamsAreNotNull(options, logger);

            if (!File.Exists(options.ImageFileName))
            {
                logger.WriteError("File \"{0}\" does not exist.", options.ImageFileName);
                return;
            }

            Bitmap image = null;

            try
            {
                image = (Bitmap)Bitmap.FromFile(options.ImageFileName);
            }
            catch (Exception)
            {
                logger.WriteError("Unable to load file \"{0}\" as an image.", options.ImageFileName);
                return;
            }

            List<Rectangle> rectangles = new List<Rectangle>();

            PerformGridBasedSearch(options, image, rectangles);

            image.Dispose();

            WriteXmlFile(options, rectangles);
        }

        private static bool DoesRectangleListContainPoint(List<Rectangle> rectangles, int x, int y)
        {
            for (int i = 0; i < rectangles.Count; i++)
            {
                if (rectangles[i].Contains(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        private static void PerformGridBasedSearch(Options options, Bitmap image, List<Rectangle> rectangles)
        {
            int x2, y2;

            Color gridColor = ColorHelper.FromHexString(options.GridColor);
            Color pixel;
            Color pixel2;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    pixel = image.GetPixel(x, y);

                    if (pixel != gridColor)
                    {                        
                        if (!DoesRectangleListContainPoint(rectangles, x, y))
                        {
                            x2 = x;
                            y2 = y;

                            pixel2 = pixel;

                            while (x2 < image.Width - 1 && pixel2 != gridColor)
                                pixel2 = image.GetPixel((++x2), y2);

                            x2--;

                            pixel2 = image.GetPixel(x2, y2);

                            while (y2 < image.Height - 1 && pixel2 != gridColor)
                                pixel2 = image.GetPixel(x2, (++y2));

                            y2--;

                            rectangles.Add(new Rectangle(x, y, x2 - x + 1, y2 - y + 1));
                        }
                    }
                }
            }
        }

        private static void WriteXmlFile(Options options, List<Rectangle> rectangles)
        {
            using (XmlTextWriter xml = new XmlTextWriter(options.XmlFileName, Encoding.UTF8))
            {
                xml.Formatting = Formatting.Indented;

                xml.WriteStartDocument();
                xml.WriteStartElement("SpriteSheet");

                xml.WriteAttributeString("Texture", Path.GetFileName(options.ImageFileName));
                xml.WriteAttributeString("ColorKey", Path.GetFileName(options.ColorKey));

                foreach (Rectangle rectangle in rectangles)
                {
                    xml.WriteStartElement("Frame");                    
                    xml.WriteAttributeString("X", rectangle.X.ToString());
                    xml.WriteAttributeString("Y", rectangle.Y.ToString());
                    xml.WriteAttributeString("Width", rectangle.Width.ToString());
                    xml.WriteAttributeString("Height", rectangle.Height.ToString());

                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
                xml.WriteEndDocument();
            }
        }
    }
}
