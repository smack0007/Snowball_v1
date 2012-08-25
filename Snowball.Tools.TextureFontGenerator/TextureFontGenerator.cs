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

			int spaceCharBitmapIndex = -1;

			for (char ch = (char)options.MinChar; ch < options.MaxChar; ch++)
			{
				Bitmap charBitmap = RenderChar(graphics, font, ch, options);

				charBitmaps.Add(charBitmap);

				x += charBitmap.Width + padding;

				if (ch != ' ')
				{
					lineHeight = Math.Max(lineHeight, charBitmap.Height);
				}
				else
				{
					spaceCharBitmapIndex = charBitmaps.Count - 1;
				}

				count++;
				if (count >= 16)
				{
					bitmapWidth = Math.Max(bitmapWidth, x);
					rows++;
					x = 0;
					count = 0;
				}
			}

			int top = lineHeight;
			int bottom = 0;

			for (int i = 0; i < charBitmaps.Count; i++)
			{
				if (i != spaceCharBitmapIndex)
				{
					Bitmap charBitmap = charBitmaps[i];

					int charTop = FindTopOfChar(charBitmap);

					if (charTop < top)
						top = charTop;

					int charBottom = FindBottomOfChar(charBitmap);

					if (charBottom > bottom)
						bottom = charBottom;
				}
			}

			lineHeight = bottom - top;

			for (int i = 0; i < charBitmaps.Count; i++)
			{
				charBitmaps[i] = CropCharHeight(charBitmaps[i], top, bottom);
			}

			bitmapHeight = (lineHeight * rows) + (padding * rows);

			using (Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb))
			{								
				using (Graphics bitmapGraphics = Graphics.FromImage(bitmap))
				{
					Color backgroundColor = ColorHelper.FromHexString(options.BackgroundColor);
					bitmapGraphics.Clear(backgroundColor);

					count = 0;
					x = 0;
					y = 0;

					char ch = (char)options.MinChar;
					for (int i = 0; i < charBitmaps.Count; i++)
					{
						int offset = (lineHeight - charBitmaps[i].Height);
						bitmapGraphics.DrawImage(charBitmaps[i], x, y + offset);

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
		private static Bitmap RenderChar(Graphics graphics, Font font, char ch, TextureFontGeneratorOptions options)
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

				bitmapGraphics.Clear(TransparentBlack);

				using (Brush brush = new SolidBrush(Color.White))
				using (StringFormat format = new StringFormat())
				{
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;

					bitmapGraphics.DrawString(text, font, brush, 0, 0, format);
				}

				bitmapGraphics.Flush();
			}

			return CropCharWidth(charBitmap);
		}

		private static int FindLeftOfChar(Bitmap charBitmap)
		{
			for (int x = 0; x < charBitmap.Width; x++)
			{
				for (int y = 0; y < charBitmap.Height; y++)	
				{
					if (charBitmap.GetPixel(x, y).A != 0)
						return x;
				}
			}

			return 0;
		}

		private static int FindRightOfChar(Bitmap charBitmap)
		{
			for (int x = charBitmap.Width - 1; x >= 0; x--)
			{
				for (int y = 0; y < charBitmap.Height; y++)
				{
					if (charBitmap.GetPixel(x, y).A != 0)
						return x;
				}
			}

			return 0;
		}

		private static int FindTopOfChar(Bitmap charBitmap)
		{
			for (int y = 0; y < charBitmap.Height; y++)
			{
				for (int x = 0; x < charBitmap.Width; x++)
				{
					if (charBitmap.GetPixel(x, y).A != 0)
						return y;
				}
			}

			return 0;
		}

		private static int FindBottomOfChar(Bitmap charBitmap)
		{
			for (int y = charBitmap.Height - 1; y >= 0; y--)
			{
				for (int x = 0; x < charBitmap.Width; x++)
				{
					if (charBitmap.GetPixel(x, y).A != 0)
						return y;					
				}
			}

			return charBitmap.Height;
		}

		/// <summary>
		/// Removes the left and right blank space of a character bitmap.
		/// </summary>
		/// <param name="charBitmap"></param>
		/// <returns></returns>
		private static Bitmap CropCharWidth(Bitmap charBitmap)
		{
			int left = FindLeftOfChar(charBitmap);
			int right = FindRightOfChar(charBitmap);
					
			// We can't crop or don't need to crop
			if (left > right || (left == 0 && right == charBitmap.Width - 1))
				return charBitmap;

			Bitmap croppedBitmap = new Bitmap((right - left) + 1, charBitmap.Height, PixelFormat.Format32bppArgb);

			using (Graphics graphics = Graphics.FromImage(croppedBitmap))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;

				RectangleF dest = new RectangleF(0, 0, (right - left) + 1, charBitmap.Height);
				RectangleF src = new RectangleF(left, 0, (right - left) + 1, charBitmap.Height);
				graphics.DrawImage(charBitmap, dest, src, GraphicsUnit.Pixel);
				graphics.Flush();
			}

			return croppedBitmap;
		}

		private static Bitmap CropCharHeight(Bitmap charBitmap, int top, int bottom)
		{	
			Bitmap croppedBitmap = new Bitmap(charBitmap.Width, bottom - top + 1, PixelFormat.Format32bppArgb);

			using (Graphics graphics = Graphics.FromImage(croppedBitmap))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;

				RectangleF dest = new RectangleF(0, 0, croppedBitmap.Width, croppedBitmap.Height);
				RectangleF src = new RectangleF(0, top, croppedBitmap.Width, croppedBitmap.Height);
				graphics.DrawImage(charBitmap, dest, src, GraphicsUnit.Pixel);
				graphics.Flush();
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
								
				xml.WriteAttributeString("Texture", Path.GetFileName(options.ImageFileName));
				xml.WriteAttributeString("BackgroundColor", options.BackgroundColor);
				xml.WriteAttributeString("FontName", options.FontName);
				xml.WriteAttributeString("FontSize", options.FontSize.ToString());
				xml.WriteAttributeString("CharacterSpacing", options.CharacterSpacing.ToString());
				xml.WriteAttributeString("LineSpacing", options.LineSpacing.ToString());

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
