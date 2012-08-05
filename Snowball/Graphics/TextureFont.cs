using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public sealed class TextureFont : GameResource, ITextureFont
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
			if (texture == null)
				throw new ArgumentNullException("texture");

			if (rectangles == null)
				throw new ArgumentNullException("rectangles");

			this.Texture = texture;
			this.rectangles = rectangles;
			this.CharacterSpacing = 2;

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

			using(var xml = new XmlTextReader(stream))
			{
				xml.WhitespaceHandling = WhitespaceHandling.None;

				xml.Read();

				if (xml.NodeType == XmlNodeType.XmlDeclaration)
					xml.Read();

				if (xml.NodeType != XmlNodeType.Element && xml.Name != "TextureFont")
					throw new XmlException("Invalid TextureFont xml file.");

				string name = xml["Name"];
				textureFile = xml["Texture"];
				backgroundColor = Color.FromHexString(xml["BackgroundColor"]);

				xml.Read();
				while (xml.Name == "Character")
				{
					Rectangle rectangle = new Rectangle(Int32.Parse(xml["X"]), Int32.Parse(xml["Y"]), Int32.Parse(xml["Width"]), Int32.Parse(xml["Height"]));
					rectangles.Add(xml["Value"][0], rectangle);
					xml.Read();
				}
			}

			Texture texture = loadTextureFunc(textureFile, backgroundColor);

			if (texture == null)
				throw new InvalidOperationException("loadTextureFunc returned null.");

			return new TextureFont(texture, rectangles);
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
			if (start < 0 || start > s.Length)
				throw new ArgumentOutOfRangeException("start", "Start is not an index within the string.");

			if (length < 0)
				throw new ArgumentOutOfRangeException("length", "Length must me >= 0.");

			if (start + length > s.Length)
				throw new ArgumentOutOfRangeException("length", "Start + length is greater than the string's length.");

			Vector2 size = new Vector2();

			size.Y = this.LineHeight;

			int lineWidth = 0;
			for (int i = start; i < length; i++)
			{
				if (s[i] == '\n')
				{
					if (lineWidth > size.X)
						size.X = lineWidth;

					lineWidth = 0;

					size.Y += this.LineHeight;
				}
				else
				{
					lineWidth += this.rectangles[s[i]].Width + this.CharacterSpacing;
				}
			}

			if (lineWidth > size.X)
				size.X = lineWidth;

			return size;
		}
	}
}
