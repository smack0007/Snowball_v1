using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Snowball.Graphics
{
	/// <summary>
	/// Wraps a texture and keeps track of the frames within it.
	/// </summary>
	public sealed class SpriteSheet
	{
		IList<Rectangle> rectangles;
		Color[] colorData;

		/// <summary>
		/// The texture of the SpriteSheet.
		/// </summary>
		public Texture Texture
		{
			get;
			private set;
		}

		/// <summary>
		/// The width of the SpriteSheet.
		/// </summary>
		public int Width
		{
			get { return this.Texture.Width; }
		}

		/// <summary>
		/// The height of the SpriteSheet.
		/// </summary>
		public int Height
		{
			get { return this.Texture.Height; }
		}

		/// <summary>
		/// The number of frames in the SpriteSheet.
		/// </summary>
		public int FrameCount
		{
			get { return this.rectangles.Count; }
		}
				
		/// <summary>
		/// Retrieves a rectangle from the SpriteSheet.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Rectangle this[int i]
		{
			get { return this.rectangles[i]; }
		}

		/// <summary>
		/// Initializes a new SpriteSheet.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="frameWidth"></param>
		/// <param name="frameHeight"></param>
		public SpriteSheet(Texture texture, int frameWidth, int frameHeight)
			: this(texture, frameWidth, frameHeight, 0, 0)
		{
		}

		/// <summary>
		/// Initializes a new SpriteSheet.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="frameWidth"></param>
		/// <param name="frameHeight"></param>
		/// <param name="framePaddingX"></param>
		/// <param name="framePaddingY"></param>
		public SpriteSheet(Texture texture, int frameWidth, int frameHeight, int framePaddingX, int framePaddingY)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			EnsureConstructorArgs(frameWidth, frameHeight, framePaddingX, framePaddingY);

			this.Texture = texture;

			this.rectangles = new List<Rectangle>();

			for (int y = framePaddingY; y < texture.Height; y += frameHeight + framePaddingY)
			{
				for (int x = framePaddingX; x < texture.Width; x += frameWidth + framePaddingX)
				{
					this.rectangles.Add(new Rectangle(x, y, frameWidth, frameHeight));
				}
			}
		}

		/// <summary>
		/// Initializes a new SpriteSheet.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="rectangles"></param>
		public SpriteSheet(Texture texture, IList<Rectangle> rectangles)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			if (rectangles == null)
				throw new ArgumentNullException("rectangles");

			this.Texture = texture;
			this.rectangles = rectangles;

			for (int i = 0; i < this.rectangles.Count; i++)
			{
				if (this.rectangles[i].Left < 0 ||
					this.rectangles[i].Top < 0 ||
					this.rectangles[i].Right > this.Width ||
					this.rectangles[i].Bottom > this.Height)
				{
					throw new InvalidOperationException(string.Format("Rectangle {0} is outside of the bounds of the sprite texture.", this.rectangles[i]));
				}
			}
		}

		/// <summary>
		/// Ensures that provided constructor args are correct.
		/// </summary>
		/// <param name="frameWidth"></param>
		/// <param name="frameHeight"></param>
		/// <param name="framePaddingX"></param>
		/// <param name="framePaddingY"></param>
		public static void EnsureConstructorArgs(int frameWidth, int frameHeight, int framePaddingX, int framePaddingY)
		{
			if (frameWidth <= 0)
				throw new ArgumentOutOfRangeException("frameWidth", "frameWidth must be > 0.");

			if (frameHeight <= 0)
				throw new ArgumentOutOfRangeException("frameHeight", "frameHeight must be > 0.");

			if (framePaddingX < 0)
				throw new ArgumentOutOfRangeException("framePaddingX", "framePaddingX must be >= 0.");

			if (framePaddingY < 0)
				throw new ArgumentOutOfRangeException("framePaddingY", "framePaddingY must be >= 0.");
		}

		/// <summary>
		/// Loads a SpriteSheet from a file.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="fileName"></param>
		/// <param name="loadTextureFunc"></param>
		/// <returns></returns>
		public static SpriteSheet FromFile(GraphicsDevice graphicsDevice, string fileName, Func<string, Color?, Texture> loadTextureFunc)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			if (!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file " + fileName + ".");

			using (Stream stream = File.OpenRead(fileName))
				return FromStream(graphicsDevice, stream, loadTextureFunc);
		}

		/// <summary>
		/// Loads a TextureFont from a stream.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="stream"></param>
		/// <param name="loadTextureFunc"></param>
		/// <returns></returns>
		public static SpriteSheet FromStream(GraphicsDevice graphicsDevice, Stream stream, Func<string, Color?, Texture> loadTextureFunc)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			if (stream == null)
				throw new ArgumentNullException("stream");

			if (loadTextureFunc == null)
				throw new ArgumentNullException("loadTextureFunc");

			List<Rectangle> rectangles = new List<Rectangle>();
			string textureFile = null;
			Color colorKey = Color.Transparent;
			
			try
			{
				using (var xml = new XmlTextReader(stream))
				{
					xml.WhitespaceHandling = WhitespaceHandling.None;

					xml.Read();

					if (xml.NodeType == XmlNodeType.XmlDeclaration)
						xml.Read();

					if (xml.NodeType != XmlNodeType.Element && xml.Name != "SpriteSheet")
						throw new XmlException("Invalid SpriteSheet xml file.");

					textureFile = xml.ReadRequiredAttributeValue("Texture");
					colorKey = Color.FromHexString(xml.ReadAttributeValueOrDefault("BackgroundColor", "00000000"));
					
					xml.Read();
					while (xml.Name == "Frame")
					{
						Rectangle rectangle = new Rectangle(
							xml.ReadRequiredAttributeValue<int>("X"),
							xml.ReadRequiredAttributeValue<int>("Y"),
							xml.ReadRequiredAttributeValue<int>("Width"),
							xml.ReadRequiredAttributeValue<int>("Height"));

						rectangles.Add(rectangle);
						xml.Read();
					}
				}
			}
			catch (XmlException ex)
			{
				throw new GraphicsException("An error occured while parsing the SpriteSheet xml file.", ex);
			}

			Texture texture = loadTextureFunc(textureFile, colorKey);

			if (texture == null)
				throw new InvalidOperationException("loadTextureFunc returned null.");

			return new SpriteSheet(texture, rectangles);
		}

		/// <summary>
		/// Fetches color data from the underlying Texture and caches it for later use. Useful for doing things like Collision detection.
		/// </summary>
		public void CacheColorData()
		{
			if (this.colorData == null)
				this.colorData = this.Texture.GetColorData();
		}

		/// <summary>
		/// Gets color data from the underlying texture which was previously cached.
		/// </summary>
		/// <returns></returns>
		public Color[] GetColorData()
		{
			if (this.colorData == null)
				throw new InvalidOperationException("CacheColorData() was never called.");

			return this.colorData;
		}
	}
}
