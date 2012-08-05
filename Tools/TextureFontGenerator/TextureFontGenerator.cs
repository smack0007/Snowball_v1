using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Text;
using System.Xml;
using System.Text;
using Snowball.Tools.Utilities;
using System.Drawing.Drawing2D;

namespace Snowball.Tools.TextureFontGenerator
{
	public static class TextureFontGenerator
	{
		private static readonly Color TransparentBlack = Color.FromArgb(0, 0, 0, 0);

		public static void Generate(TextureFontGeneratorOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("options");

			Font font = new Font(options.FontName, options.FontSize);
						
			Graphics graphics = Graphics.FromImage(new Bitmap(1, 1, PixelFormat.Format32bppArgb));
			
			List<Bitmap> charBitmaps = new List<Bitmap>();
			Dictionary<char, Rectangle> rectangles = new Dictionary<char, Rectangle>();
			
			int bitmapWidth = 0;
			int bitmapHeight = 0;
			int lineHeight = 0;
			int rows = 1;

			int count = 0;
			int x = 0;
			int y = 0;
			const int padding = 4;

			for (char ch = (char)options.MinChar; ch < options.MaxChar; ch++)
			{
				Bitmap charBitmap = RenderCharcater(graphics, font, ch, options);

				charBitmaps.Add(charBitmap);

				x += charBitmap.Width + padding;
				lineHeight = Math.Max(lineHeight, charBitmap.Height);

				count++;
				if (count >= 16)
				{
					bitmapWidth = Math.Max(bitmapWidth, x);
					rows++;
					x = 0;
					count = 0;
				}
			}

			bitmapHeight = (lineHeight * rows) + (padding * rows);

			using (Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb))
			{
				using (Graphics bitmapGraphics = Graphics.FromImage(bitmap))
				{
					count = 0;
					x = 0;
					y = 0;

					char ch = (char)options.MinChar;
					for (int i = 0; i < charBitmaps.Count; i++)
					{
						bitmapGraphics.DrawImage(charBitmaps[i], x, y);

						rectangles.Add(ch, new Rectangle(x, y, charBitmaps[i].Width, lineHeight));
						ch++;

						x += charBitmaps[i].Width + padding;
						charBitmaps[i].Dispose();

						count++;
						if (count >= 16)
						{
							x = 0;
							y += lineHeight + padding;
							count = 0;
						}
					}
				}

				Color backgroundColor = ColorHelper.FromHexString(options.BackgroundColor);

				if (backgroundColor != TransparentBlack)
				{
					for (y = 0; y < bitmap.Height; y++)
					{
						for (x = 0; x < bitmap.Width; x++)
						{
							if (bitmap.GetPixel(x, y) == TransparentBlack)
								bitmap.SetPixel(x, y, backgroundColor);
						}
					}
				}
								
				bitmap.Save(options.ImageFileName, ImageFormat.Png);
			}

			WriteXmlFile(rectangles, options);
		}

		/// <summary>
		/// Renders a character to a Bitmap.
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="font"></param>
		/// <param name="ch"></param>
		/// <param name="antialias"></param>
		/// <returns></returns>
		private static Bitmap RenderCharcater(Graphics graphics, Font font, char ch, TextureFontGeneratorOptions options)
		{
			string text = ch.ToString();
			SizeF size = graphics.MeasureString(text, font);

			int charWidth = (int)Math.Ceiling(size.Width);
			int charHeight = (int)Math.Ceiling(size.Height);

			Bitmap charBitmap = new Bitmap(charWidth, charHeight, PixelFormat.Format32bppArgb);

			using (Graphics bitmapGraphics = Graphics.FromImage(charBitmap))
			{
				if (options.Antialias)
				{
					bitmapGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				}
				else
				{
					bitmapGraphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
				}

				bitmapGraphics.Clear(Color.Transparent);

				using (Brush brush = new SolidBrush(System.Drawing.Color.White))
				using (StringFormat format = new StringFormat())
				{
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;

					bitmapGraphics.DrawString(text, font, brush, 0, 0, format);
				}

				bitmapGraphics.Flush();
			}

			return CropCharacter(charBitmap);
		}

		/// <summary>
		/// Removes the left and right blank space of a character bitmap.
		/// </summary>
		/// <param name="charBitmap"></param>
		/// <returns></returns>
		private static Bitmap CropCharacter(Bitmap charBitmap)
		{
			int left = 0;
			int right = charBitmap.Width - 1;
			bool go = true;

			// See how far we can crop on the left
			while (go)
			{
				for (int y = 0; y < charBitmap.Height; y++)
				{
					if (charBitmap.GetPixel(left, y).A != 0)
					{
						go = false;
						break;
					}
				}

				if (go)
				{
					left++;

					if (left >= charBitmap.Width)
						break;
				}
			}

			go = true;

			// See how far we can crop on the right
			while (go)
			{
				for (int y = 0; y < charBitmap.Height; y++)
				{
					if (charBitmap.GetPixel(right, y).A != 0)
					{
						go = false;
						break;
					}
				}

				if (go)
				{
					right--;

					if (right < 0)
						break;
				}
			}

			// We can't crop or don't need to crop
			if (left > right || (left == 0 && right == charBitmap.Width - 1))
				return charBitmap;

			Bitmap croppedBitmap = new Bitmap((right - left) + 1, charBitmap.Height, PixelFormat.Format32bppArgb);

			using (System.Drawing.Graphics croppedGraphics = System.Drawing.Graphics.FromImage(croppedBitmap))
			{
				croppedGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

				System.Drawing.RectangleF dest = new System.Drawing.RectangleF(0, 0, (right - left) + 1, charBitmap.Height);
				System.Drawing.RectangleF src = new System.Drawing.RectangleF(left, 0, (right - left) + 1, charBitmap.Height);
				croppedGraphics.DrawImage(charBitmap, dest, src, GraphicsUnit.Pixel);
				croppedGraphics.Flush();
			}

			return croppedBitmap;
		}

		private static void WriteXmlFile(Dictionary<char, Rectangle> rectangles, TextureFontGeneratorOptions options)
		{			
			using (XmlTextWriter xml = new XmlTextWriter(options.XmlFileName, Encoding.UTF8))
			{
				xml.Formatting = Formatting.Indented;

				xml.WriteStartDocument();
				xml.WriteStartElement("TextureFont");

				xml.WriteAttributeString("Name", options.FontName);
				xml.WriteAttributeString("Texture", Path.GetFileName(options.ImageFileName));
				xml.WriteAttributeString("BackgroundColor", options.BackgroundColor);
				xml.WriteAttributeString("FontName", options.FontName);
				xml.WriteAttributeString("FontSize", options.FontSize.ToString());

				foreach (char ch in rectangles.Keys)
				{
					xml.WriteStartElement("Character");

					xml.WriteAttributeString("Value", ch.ToString());

					Rectangle rectangle = rectangles[ch];
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
