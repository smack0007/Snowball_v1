using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using D3D = SlimDX.Direct3D9;

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

            this.Width = width;
			this.Height = height;

            this.InternalWidth = (int)MathHelper.NextPowerOf2((uint)width);
            this.InternalHeight = (int)MathHelper.NextPowerOf2((uint)height);

			this.InternalTexture = new D3D.Texture(
                graphicsDevice.InternalDevice,
                this.InternalWidth,
                this.InternalHeight,
                0,
                D3D.Usage.None,
                D3D.Format.A8R8G8B8,
                D3D.Pool.Managed);
			
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
			{
				throw new ArgumentNullException("texture");
			}

            this.Width = width;
            this.Height = height;

            this.InternalTexture = ResizeToPowerOf2(graphicsDevice.InternalDevice, texture, width, height, out this.InternalWidth, out this.InternalHeight);
		}

        private static D3D.Texture ResizeToPowerOf2(D3D.Device graphicsDevice, D3D.Texture input, int width, int height, out int newWidth, out int newHeight)
        {
            newWidth = (int)MathHelper.NextPowerOf2((uint)width);
            newHeight = (int)MathHelper.NextPowerOf2((uint)height);

            // Check if we need to resize
            if (newWidth == width && newHeight == height)
                return input;

            D3D.Texture output = new D3D.Texture(
                graphicsDevice,
                newWidth,
                newHeight,
                0,
                D3D.Usage.None,
                D3D.Format.A8R8G8B8,
                D3D.Pool.Managed);

            SlimDX.DataRectangle inputData = input.LockRectangle(0, D3D.LockFlags.None);
            SlimDX.DataRectangle outputData = output.LockRectangle(0, D3D.LockFlags.None);

            for (int y = 0; y < height; y++)
            {
                outputData.Data.Seek(outputData.Pitch * y, SeekOrigin.Begin);

                for (int x = 0; x < width; x++)
                {
                    outputData.Data.WriteByte((byte)inputData.Data.ReadByte()); // B
                    outputData.Data.WriteByte((byte)inputData.Data.ReadByte()); // G
                    outputData.Data.WriteByte((byte)inputData.Data.ReadByte()); // R
                    outputData.Data.WriteByte((byte)inputData.Data.ReadByte()); // A
                }
            }

            input.UnlockRectangle(0);
            output.UnlockRectangle(0);

            input.Dispose(); // Get rid of old texture
            return output;
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

			D3D.Texture texture = D3D.Texture.FromStream(
                graphicsDevice.InternalDevice,
                stream,
                width,
                height, 
                0,
				D3D.Usage.None,
                D3D.Format.A8R8G8B8,
				D3D.Pool.Managed,
                D3D.Filter.Point,
				D3D.Filter.Point,
                argb);
                 
			return new Texture(graphicsDevice, texture, width, height);
		}

		/// <summary>
		/// Gets the pixels of the Texture.
		/// </summary>
		/// <returns></returns>
		public Color[] GetColorData()
		{
			Color[] colorData = new Color[this.Width * this.Height];

			SlimDX.DataRectangle dataRectangle = this.InternalTexture.LockRectangle(0, D3D.LockFlags.ReadOnly);

            int x = 0;
            int y = 0;

			for(int i = 0; i < colorData.Length; i++)
			{
                if (x <= this.Width && y <= this.Height)
                {
                    byte b = (byte)dataRectangle.Data.ReadByte();
                    byte g = (byte)dataRectangle.Data.ReadByte();
                    byte r = (byte)dataRectangle.Data.ReadByte();
                    byte a = (byte)dataRectangle.Data.ReadByte();

                    colorData[i] = new Color(r, g, b, a);
                }
                else
                {
                    // Throw away 4 bytes.
                    dataRectangle.Data.ReadByte();
                    dataRectangle.Data.ReadByte();
                    dataRectangle.Data.ReadByte();
                    dataRectangle.Data.ReadByte();
                }

                x++;
                if (x >= this.Width)
                {
                    x = 0;
                    y++;
                }
			}

			this.InternalTexture.UnlockRectangle(0);

			return colorData;
		}
	}
}
