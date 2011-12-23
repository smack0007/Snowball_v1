using System;
using System.IO;
using SlimDX.Direct3D9;
using System.Drawing;
using System.Drawing.Imaging;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which contains an image.
	/// </summary>
	public sealed class Texture : GameResource
	{
		internal SlimDX.Direct3D9.Texture InternalTexture;
				
		/// <summary>
		/// The width of the Texture in pixels.
		/// </summary>
		public int Width
		{
			get;
			protected set;
		}

		/// <summary>
		/// The height of the Texture in pixels.
		/// </summary>
		public int Height
		{
			get;
			protected set;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Texture(GraphicsDevice graphicsDevice, int width, int height)
			: base()
		{
			if (graphicsDevice == null)
			{
				throw new ArgumentNullException("graphicsDevice");
			}

			this.InternalTexture = new SlimDX.Direct3D9.Texture(graphicsDevice.InternalDevice, width, height, 0, SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Managed);
			this.Width = width;
			this.Height = height;
		}

		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		internal Texture(SlimDX.Direct3D9.Texture texture, int width, int height)
			: base()
		{
			if (texture == null)
			{
				throw new ArgumentNullException("texture");
			}

			this.InternalTexture = texture;
			this.Width = width;
			this.Height = height;
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
			if (!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file \"" + fileName + "\".");

			using(Stream stream = File.OpenRead(fileName))
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

			graphicsDevice.EnsureDeviceCreated();

			int width;
			int height;
						
			using(Image image = Image.FromStream(stream))
			{
				width = image.Width;
				height = image.Height;
			}

			stream.Position = 0;

			int argb = 0;
			if (colorKey != null)
				argb = colorKey.Value.ToArgb();

			SlimDX.Direct3D9.Texture texture = SlimDX.Direct3D9.Texture.FromStream(graphicsDevice.InternalDevice, stream, width, height, 0,
																				   SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8,
																				   SlimDX.Direct3D9.Pool.Managed, SlimDX.Direct3D9.Filter.Point,
																				   SlimDX.Direct3D9.Filter.Point, argb);

			return new Texture(texture, width, height);
		}
				
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.InternalTexture != null)
				{
					this.InternalTexture.Dispose();
					this.InternalTexture = null;
				}
			}
		}

		/// <summary>
		/// Gets the pixels of the Texture.
		/// </summary>
		/// <returns></returns>
		public Color[] GetColorData()
		{
			Color[] colorData = new Color[this.Width * this.Height];

			SlimDX.DataRectangle dataRectangle = this.InternalTexture.LockRectangle(0, LockFlags.ReadOnly);

			for(int i = 0; i < colorData.Length; i++)
			{
				byte b = (byte)dataRectangle.Data.ReadByte();
				byte g = (byte)dataRectangle.Data.ReadByte();
				byte r = (byte)dataRectangle.Data.ReadByte();
				byte a = (byte)dataRectangle.Data.ReadByte();

				colorData[i] = new Color(r, g, b, a);
			}

			this.InternalTexture.UnlockRectangle(0);

			return colorData;
		}
	}
}
