using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public sealed class TextureFont : DisposableObject
	{
		public const string DefaultFontName = "Unknown";

		public const int DefaultFontSize = 12;

		public const int DefaultCharacterSpacing = 2;

		public const int DefaultLineSpacing = 0;

		private static readonly char[] CharsToExclude = new char[] { '\r' };

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

		public string FontName
		{
			get;
			private set;
		}

		public int FontSize
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

		/// <summary>
		/// The amount of space to use between each line when rendering a string.
		/// </summary>
		public int LineSpacing
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
			: this(texture, rectangles, DefaultFontName, DefaultFontSize)
		{
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="rectangles"></param>
		/// <param name="fontName"></param>
		/// <param name="fontSize"></param>
		public TextureFont(Texture texture, Dictionary<char, Rectangle> rectangles, string fontName, int fontSize)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			if (rectangles == null)
				throw new ArgumentNullException("rectangles");

			if (string.IsNullOrEmpty(fontName))
				throw new ArgumentNullException("fontName");

			if (fontSize < 1)
				throw new ArgumentOutOfRangeException("fontSize", "fontSize must be >= 1.");

			this.Texture = texture;
			this.rectangles = rectangles;
			this.CharacterSpacing = DefaultCharacterSpacing;
			this.LineSpacing = DefaultLineSpacing;

			foreach (Rectangle rectangle in this.rectangles.Values)
				if (rectangle.Height > this.LineHeight)
					this.LineHeight = rectangle.Height;
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.Texture != null)
				{
					this.Texture.Dispose();
					this.Texture = null;
				}
			}
		}

		/// <summary>
		/// Loads a TextureFont from a file.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="fileName"></param>
		/// <param name="loadTextureFunc"></param>
		/// <returns></returns>
		public static TextureFont FromFile(GraphicsDevice graphicsDevice, string fileName, Func<string, Color?, Texture> loadTextureFunc)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			if (!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file " + fileName + ".");

			using(Stream stream = File.OpenRead(fileName))
				return FromStream(graphicsDevice, stream, loadTextureFunc);
		}

		/// <summary>
		/// Loads a TextureFont from a stream.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="stream"></param>
		/// <param name="loadTextureFunc"></param>
		/// <returns></returns>
		public static TextureFont FromStream(GraphicsDevice graphicsDevice, Stream stream, Func<string, Color?, Texture> loadTextureFunc)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			if (stream == null)
				throw new ArgumentNullException("stream");

			if (loadTextureFunc == null)
				throw new ArgumentNullException("loadTextureFunc");

			graphicsDevice.EnsureDeviceCreated();

			Dictionary<char, Rectangle> rectangles = new Dictionary<char, Rectangle>();
			string textureFile = null;
			Color backgroundColor = Color.Transparent;
			string fontName = TextureFont.DefaultFontName;
			int fontSize = TextureFont.DefaultFontSize;
			int characterSpacing;
			int lineSpacing;

			try
			{
				using (var xml = new XmlTextReader(stream))
				{
					xml.WhitespaceHandling = WhitespaceHandling.None;

					xml.Read();

					if (xml.NodeType == XmlNodeType.XmlDeclaration)
						xml.Read();

					if (xml.NodeType != XmlNodeType.Element && xml.Name != "TextureFont")
						throw new XmlException("Invalid TextureFont xml file.");

					textureFile = xml.ReadRequiredAttributeValue("Texture");

					backgroundColor = Color.FromHexString(xml.ReadAttributeValueOrDefault("BackgroundColor", "FFFFFFFF"));
					fontName = xml.ReadAttributeValueOrDefault("FontName", DefaultFontName);
					fontSize = xml.ReadAttributeValueOrDefault<int>("FontSize", DefaultFontSize);
					characterSpacing = xml.ReadAttributeValueOrDefault<int>("CharacterSpacing", DefaultCharacterSpacing);
					lineSpacing = xml.ReadAttributeValueOrDefault<int>("LineSpacing", DefaultLineSpacing);

					xml.Read();
					while (xml.Name == "Character")
					{
						Rectangle rectangle = new Rectangle(
							xml.ReadRequiredAttributeValue<int>("X"),
							xml.ReadRequiredAttributeValue<int>("Y"),
							xml.ReadRequiredAttributeValue<int>("Width"),
							xml.ReadRequiredAttributeValue<int>("Height"));

						rectangles.Add(xml.ReadRequiredAttributeValue("Value")[0], rectangle);
						xml.Read();
					}
				}
			}
			catch (XmlException ex)
			{
				throw new GraphicsException("An error occured while parsing the TextureFont xml file.", ex);
			}

			Texture texture = loadTextureFunc(textureFile, backgroundColor);

			if (texture == null)
				throw new InvalidOperationException("loadTextureFunc returned null.");

			return new TextureFont(texture, rectangles, fontName, fontSize)
			{
				CharacterSpacing = characterSpacing,
				LineSpacing = lineSpacing
			};
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
		public Size MeasureString(string s)
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
		public Size MeasureString(string s, int start, int length)
		{
			if (start < 0 || start > s.Length)
				throw new ArgumentOutOfRangeException("start", "Start is not an index within the string.");

			if (length < 0)
				throw new ArgumentOutOfRangeException("length", "Length must me >= 0.");

			if (start + length > s.Length)
				throw new ArgumentOutOfRangeException("length", "Start + length is greater than the string's length.");

			Size size = Size.Zero;

			size.Height = this.LineHeight;

			int lineWidth = 0;
			for (int i = start; i < length; i++)
			{
				if (s[i] == '\n')
				{
					if (lineWidth > size.Width)
						size.Width = lineWidth;

					lineWidth = 0;

					size.Height += this.LineHeight + this.LineSpacing;
				}
				else if (!CharsToExclude.Contains(s[i]))
				{
					lineWidth += this.rectangles[s[i]].Width + this.CharacterSpacing;
				}
			}

			if (lineWidth > size.Width)
				size.Width = lineWidth;

			return size;
		}
	}
}
