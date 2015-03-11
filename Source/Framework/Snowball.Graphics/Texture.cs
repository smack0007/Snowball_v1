using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which contains an image.
	/// </summary>
	public sealed partial class Texture : DisposableObject
	{
		/// <summary>
		/// The width of the Texture in pixels.
		/// </summary>
		public int Width
		{
			get;
			private set;
		}

		/// <summary>
		/// The height of the Texture in pixels.
		/// </summary>
		public int Height
		{
			get;
			private set;
		}

		public TextureUsage Usage
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Texture(GraphicsDevice graphicsDevice, int width, int height)
			: this(graphicsDevice, width, height, TextureUsage.None)
		{
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="usage"></param>
		public Texture(GraphicsDevice graphicsDevice, int width, int height, TextureUsage usage)
			: base()
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

            this.Width = width;
			this.Height = height;
			this.Usage = usage;

			this.Construct(graphicsDevice);
		}
		
		/// <summary>
		/// Loads a Texture from a file.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="fileName"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public static Texture FromFile(GraphicsDevice graphicsDevice, string fileName, Color? colorKey)
		{
			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			if (!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file \"" + fileName + "\".");

			using (Stream stream = File.OpenRead(fileName))
				return FromStream(graphicsDevice, stream, colorKey);
		}

		/// <summary>
		/// Loads a Texture from a Stream.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="stream"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public static Texture FromStream(GraphicsDevice graphicsDevice, Stream stream, Color? colorKey)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

            if (stream == null)
                throw new ArgumentNullException("stream");

			int width;
			int height;
						
			using (Image image = Image.FromStream(stream))
			{
				width = image.Width;
				height = image.Height;
			}
            
			stream.Position = 0;

            int argb = 0;
			if (colorKey != null)
				argb = colorKey.Value.ToArgb();

			return FromStreamInternal(graphicsDevice, stream, width, height, argb);
		}

		/// <summary>
		/// Gets the pixels of the Texture.
		/// </summary>
		/// <returns></returns>
		public Color[] GetPixels()
		{
			return this.GetPixelsInternal();
		}

		public void SaveToFile(string fileName)
		{
			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");
						
			using (Stream stream = File.OpenRead(fileName))
				this.SaveToStream(stream);
		}

		public void SaveToStream(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			Color[] data = this.GetPixels();

			Bitmap bitmap = new Bitmap(this.Width, this.Height);
			
			int i = 0;
			for (int y = 0; y < this.Height; y++)
			{
				for (int x = 0; x < this.Width; x++)
				{
					bitmap.SetPixel(x, y, TypeConverter.Convert(data[i]));
					i++;
				}
			}

			bitmap.Save(stream, ImageFormat.Png);
		}
	}
}
