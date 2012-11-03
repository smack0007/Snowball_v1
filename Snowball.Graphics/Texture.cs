using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which contains an image.
	/// </summary>
	public sealed class Texture : GameResource
	{
		internal D3D.Texture InternalTexture;
		internal int InternalWidth;
        internal int InternalHeight;

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

			graphicsDevice.EnsureDeviceCreated();

            this.Width = width;
			this.Height = height;
			this.Usage = usage;

			this.CalculateInternalSize(graphicsDevice);

			this.InternalTexture = D3DHelper.CreateTexture(graphicsDevice.InternalDevice, this.InternalWidth, this.InternalHeight, this.Usage);
		}

		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		internal Texture(GraphicsDevice graphicsDevice, D3D.Texture texture, int width, int height)
			: base()
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			graphicsDevice.EnsureDeviceCreated();

            this.Width = width;
            this.Height = height;
			this.Usage = TextureUsage.None;

			this.CreateInternalTexture(graphicsDevice, texture);
		}

		private void CalculateInternalSize(GraphicsDevice graphicsDevice)
		{
			this.InternalWidth = this.Width;
			this.InternalHeight = this.Height;

			if (graphicsDevice.TexturesMustBePowerOf2)
			{
				this.InternalWidth = (int)MathHelper.NextPowerOf2((uint)this.InternalWidth);
				this.InternalHeight = (int)MathHelper.NextPowerOf2((uint)this.InternalHeight);
			}

			if (graphicsDevice.TexturesMustBeSquare)
			{
				if (this.InternalWidth > this.InternalHeight)
					this.InternalHeight = this.InternalWidth;
				else if (this.InternalHeight > this.InternalWidth)
					this.InternalWidth = this.InternalHeight;
			}
		}

        private void CreateInternalTexture(GraphicsDevice graphicsDevice, D3D.Texture texture)
        {
			this.CalculateInternalSize(graphicsDevice);

            // Check if we need to resize
			if (this.InternalWidth == this.Width && this.InternalHeight == this.Height)
			{
				this.InternalTexture = texture;
				return;
			}

            this.InternalTexture = D3DHelper.CreateTexture(graphicsDevice.InternalDevice, this.InternalWidth, this.InternalHeight, TextureUsage.None);

			SharpDX.DataRectangle input = texture.LockRectangle(0, D3D.LockFlags.ReadOnly);
			SharpDX.DataStream inputStream = new SharpDX.DataStream(input.DataPointer, this.Height * input.Pitch, true, false);

			SharpDX.DataRectangle output = this.InternalTexture.LockRectangle(0, D3D.LockFlags.None);
			SharpDX.DataStream outputStream = new SharpDX.DataStream(output.DataPointer, this.InternalHeight * output.Pitch, true, true);
			
			byte[] buffer = new byte[4];

            for (int y = 0; y < this.Height; y++)
			{
				for (int x = 0; x < this.Width; x++)
				{
					inputStream.Seek((y * input.Pitch) + (x * 4), SeekOrigin.Begin);
					inputStream.Read(buffer, 0, 4);

					outputStream.Seek((y * output.Pitch) + (x * 4), SeekOrigin.Begin);
					outputStream.Write(buffer, 0, 4);
				}
			}

            texture.UnlockRectangle(0);
            this.InternalTexture.UnlockRectangle(0);

            texture.Dispose(); // Get rid of old texture
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

			graphicsDevice.EnsureDeviceCreated();

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

			D3D.Texture texture = D3DHelper.TextureFromStream(graphicsDevice.InternalDevice, stream, width, height, argb);
			return new Texture(graphicsDevice, texture, width, height);
		}

		/// <summary>
		/// Gets the pixels of the Texture.
		/// </summary>
		/// <returns></returns>
		public Color[] GetColorData()
		{
			Color[] colorData = new Color[this.Width * this.Height];

			SharpDX.DataRectangle dataRectangle = this.InternalTexture.LockRectangle(0, D3D.LockFlags.ReadOnly);
			
			using (SharpDX.DataStream dataStream = new SharpDX.DataStream(dataRectangle.DataPointer, this.InternalWidth * dataRectangle.Pitch, true, false))
			{
				int x = 0;
				int y = 0;

				for (int i = 0; i < colorData.Length; i++)
				{
					dataStream.Seek((y * dataRectangle.Pitch) + (x * 4), SeekOrigin.Begin);

					byte b = (byte)dataStream.ReadByte();
					byte g = (byte)dataStream.ReadByte();
					byte r = (byte)dataStream.ReadByte();
					byte a = (byte)dataStream.ReadByte();

					colorData[i] = new Color(r, g, b, a);

					x++;
					if (x >= this.Width)
					{
						x = 0;
						y++;
					}
				}
			}

			this.InternalTexture.UnlockRectangle(0);

			return colorData;
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

			Color[] data = this.GetColorData();

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
