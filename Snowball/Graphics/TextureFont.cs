using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace Snowball.Graphics
{
	public class TextureFont : GameResource
	{
		Dictionary<char, Rectangle> rectangles;
				
		public Texture Texture
		{
			get;
			private set;
		}

		public int LineHeight
		{
			get;
			private set;
		}

		/// <summary>
		/// The amount of space to use between each character when rendering a string.
		/// </summary>
		public int CharacterSpacing
		{
			get;
			set;
		}

		public Rectangle this[char ch]
		{
			get { return this.rectangles[ch]; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="texture">The texture used by the font.</param>
		/// <param name="rectangles">Dictionary of characters to rectangles.</param>
		public TextureFont(Texture texture, Dictionary<char, Rectangle> rectangles)
		{
			if(texture == null)
				throw new ArgumentNullException("texture");

			if(rectangles == null)
				throw new ArgumentNullException("rectangles");

			this.Texture = texture;
			this.rectangles = rectangles;
			this.CharacterSpacing = 2;

			foreach(Rectangle rectangle in this.rectangles.Values)
				if(rectangle.Height > this.LineHeight)
					this.LineHeight = rectangle.Height;
		}

		internal TextureFont(GraphicsManager graphicsManager, string fontName, int fontSize, bool antialias)
		{
			if(graphicsManager == null)
			{
				throw new ArgumentNullException("graphicsManager");
			}

			Font font = new Font(fontName, fontSize);

			int minChar = 0x20;
			int maxChar = 0x7F;

			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1, PixelFormat.Format32bppArgb));
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

			MemoryStream stream = new MemoryStream();

			for(char ch = (char)minChar; ch < maxChar; ch++)
			{
				Bitmap charBitmap = this.RenderCharcater(graphics, font, ch, antialias);

				charBitmaps.Add(charBitmap);

				x += charBitmap.Width + padding;
				lineHeight = Math.Max(lineHeight, charBitmap.Height);

				count++;
				if(count >= 16)
				{
					bitmapWidth = Math.Max(bitmapWidth, x);
					rows++;
					x = 0;
					count = 0;
				}
			}

			bitmapHeight = (lineHeight * rows) + (padding * rows);

			using(Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb))
			{
				using(System.Drawing.Graphics bitmapGraphics = System.Drawing.Graphics.FromImage(bitmap))
				{
					count = 0;
					x = 0;
					y = 0;

					char ch = (char)minChar;
					for(int i = 0; i < charBitmaps.Count; i++)
					{
						bitmapGraphics.DrawImage(charBitmaps[i], x, y);

						rectangles.Add(ch, new Rectangle(x, y, charBitmaps[i].Width, lineHeight));
						ch++;

						x += charBitmaps[i].Width + padding;
						charBitmaps[i].Dispose();

						count++;
						if(count >= 16)
						{
							x = 0;
							y += lineHeight + padding;
							count = 0;
						}
					}
				}

				bitmap.Save(stream, ImageFormat.Bmp);
				stream.Position = 0;
			}

			SlimDX.Direct3D9.Texture texture = SlimDX.Direct3D9.Texture.FromStream(graphicsManager.InternalDevice, stream, bitmapWidth, bitmapHeight, 0,
																				   SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8,
																				   SlimDX.Direct3D9.Pool.Managed, SlimDX.Direct3D9.Filter.Point,
																				   SlimDX.Direct3D9.Filter.Point, 0);

			stream.Dispose();

			this.Texture = new Texture(texture, bitmapWidth, bitmapHeight);
			this.rectangles = rectangles;
			this.LineHeight = lineHeight;
			this.CharacterSpacing = 2;
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.Texture != null)
				{
					this.Texture.Dispose();
					this.Texture = null;
				}
			}
		}

		internal static TextureFont Load(GraphicsManager graphicsManager, string fileName, Color? colorKey)
		{
			if(graphicsManager == null)
			{
				throw new ArgumentNullException("graphicsManager");
			}

			if(!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file " + fileName + ".");

			Dictionary<char, Rectangle> rectangles = new Dictionary<char, Rectangle>();
			string textureFile = null;

			using(var xml = new XmlTextReader(fileName))
			{
				xml.WhitespaceHandling = WhitespaceHandling.None;

				xml.Read();

				if(xml.NodeType == XmlNodeType.XmlDeclaration)
					xml.Read();

				if(xml.NodeType != XmlNodeType.Element && xml.Name != "TextureFont")
					throw new XmlException("Invalid TextureFont xml file.");

				string name = xml["Name"];
				textureFile = xml["Texture"];

				xml.Read();
				while(xml.Name == "Character")
				{
					Rectangle rectangle = new Rectangle(Int32.Parse(xml["X"]), Int32.Parse(xml["Y"]), Int32.Parse(xml["Width"]), Int32.Parse(xml["Height"]));
					rectangles.Add(xml["Value"][0], rectangle);
					xml.Read();
				}
			}

			return new TextureFont(Texture.Load(graphicsManager, textureFile, colorKey), rectangles);
		}

		/// <summary>
		/// Renders a character to a Bitmap.
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="font"></param>
		/// <param name="ch"></param>
		/// <param name="antialias"></param>
		/// <returns></returns>
		private Bitmap RenderCharcater(System.Drawing.Graphics graphics, Font font, char ch, bool antialias)
		{
			string text = ch.ToString();
			SizeF size = graphics.MeasureString(text, font);

			int charWidth = (int)Math.Ceiling(size.Width);
			int charHeight = (int)Math.Ceiling(size.Height);

			Bitmap charBitmap = new Bitmap(charWidth, charHeight, PixelFormat.Format32bppArgb);

			using(System.Drawing.Graphics bitmapGraphics = System.Drawing.Graphics.FromImage(charBitmap))
			{
				if(antialias)
					bitmapGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				else
					bitmapGraphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

				bitmapGraphics.Clear(System.Drawing.Color.Transparent);

				using(Brush brush = new SolidBrush(System.Drawing.Color.White))
				using(StringFormat format = new StringFormat())
				{
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;

					bitmapGraphics.DrawString(text, font, brush, 0, 0, format);
				}

				bitmapGraphics.Flush();
			}

			return this.CropCharacter(charBitmap);
		}

		private Bitmap CropCharacter(Bitmap charBitmap)
		{
			int left = 0;
			int right = charBitmap.Width - 1;
			bool go = true;

			// See how far we can crop on the left
			while(go)
			{
				for(int y = 0; y < charBitmap.Height; y++)
				{
					if(charBitmap.GetPixel(left, y).A != 0)
					{
						go = false;
						break;
					}
				}

				if(go)
				{
					left++;

					if(left >= charBitmap.Width)
						break;
				}
			}

			go = true;

			// See how far we can crop on the right
			while(go)
			{
				for(int y = 0; y < charBitmap.Height; y++)
				{
					if(charBitmap.GetPixel(right, y).A != 0)
					{
						go = false;
						break;
					}
				}

				if(go)
				{
					right--;

					if(right < 0)
						break;
				}
			}

			// We can't crop or don't need to crop
			if(left > right || (left == 0 && right == charBitmap.Width - 1))
				return charBitmap;

			Bitmap croppedBitmap = new Bitmap((right - left) + 1, charBitmap.Height, PixelFormat.Format32bppArgb);

			using(System.Drawing.Graphics croppedGraphics = System.Drawing.Graphics.FromImage(croppedBitmap))
			{
				croppedGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

				System.Drawing.RectangleF dest = new System.Drawing.RectangleF(0, 0, (right - left) + 1, charBitmap.Height);
				System.Drawing.RectangleF src = new System.Drawing.RectangleF(left, 0, (right - left) + 1, charBitmap.Height);
				croppedGraphics.DrawImage(charBitmap, dest, src, GraphicsUnit.Pixel);
				croppedGraphics.Flush();
			}

			return croppedBitmap;
		}

		/// <summary>
		/// Returns true if the TextureFont can render the given character.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public bool ContainsCharacter(char ch)
		{
			return this.rectangles.ContainsKey(ch);
		}

		/// <summary>
		/// Measures the given string.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string s)
		{
			return this.MeasureString(s, 0, s.Length);
		}

		/// <summary>
		/// Measures the size of the string.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="start">The index of the string at which to start measuring.</param>
		/// <param name="length">How many characters to measure from the start.</param>
		/// <returns></returns>
		public Vector2 MeasureString(string s, int start, int length)
		{
			if(start < 0 || start > s.Length)
				throw new ArgumentOutOfRangeException("start", "Start is not an index within the string.");

			if(length < 0)
				throw new ArgumentOutOfRangeException("length", "Length must me >= 0.");

			if(start + length > s.Length)
				throw new ArgumentOutOfRangeException("length", "Start + length is greater than the string's length.");

			Vector2 size = new Vector2();

			size.Y = this.LineHeight;

			int lineWidth = 0;
			for(int i = start; i < length; i++)
			{
				if(s[i] == '\n')
				{
					if(lineWidth > size.X)
						size.X = lineWidth;

					lineWidth = 0;

					size.Y += this.LineHeight;
				}
				else
				{
					lineWidth += this.rectangles[s[i]].Width + this.CharacterSpacing;
				}
			}

			if(lineWidth > size.X)
				size.X = lineWidth;

			return size;
		}
	}
}
