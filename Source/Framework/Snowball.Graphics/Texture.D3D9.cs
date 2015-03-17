using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using D3D9 = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public partial class Texture
	{
		internal D3D9.Texture d3d9Texture;
		internal int d3d9Width;
		internal int d3d9Height;

		internal static D3D9.Texture CreateD3D9Texture(D3D9.Device device, int width, int height, TextureUsage usage)
		{
			D3D9.Usage d3d9Usage = D3D9.Usage.None;
			D3D9.Pool d3d9Pool = D3D9.Pool.Managed;

			switch (usage)
			{
				case TextureUsage.RenderTarget:
					d3d9Usage = D3D9.Usage.RenderTarget;
					d3d9Pool = D3D9.Pool.Default;
					break;
			}

			return new D3D9.Texture(
				device,
				width,
				height,
				1,
				d3d9Usage,
				D3D9.Format.A8R8G8B8,
				d3d9Pool);
		}

		private void Construct(GraphicsDevice graphicsDevice)
		{
			this.CalculateD3D9Size(graphicsDevice);

			this.d3d9Texture = CreateD3D9Texture(graphicsDevice.d3d9Device, this.d3d9Width, this.d3d9Height, this.Usage);
		}

		internal Texture(GraphicsDevice graphicsDevice, D3D9.Texture texture, int width, int height)
			: base()
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			this.Width = width;
			this.Height = height;
			this.Usage = TextureUsage.None;

			this.CreateD3D9Texture(graphicsDevice, texture);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.d3d9Texture != null)
				{
					this.d3d9Texture.Dispose();
					this.d3d9Texture = null;
				}
			}
		}

		private void CalculateD3D9Size(GraphicsDevice graphicsDevice)
		{
			this.d3d9Width = this.Width;
			this.d3d9Height = this.Height;

			if (graphicsDevice.TexturesMustBePowerOf2)
			{
				this.d3d9Width = (int)MathHelper.NextPowerOf2((uint)this.d3d9Width);
				this.d3d9Height = (int)MathHelper.NextPowerOf2((uint)this.d3d9Height);
			}

			if (graphicsDevice.TexturesMustBeSquare)
			{
				if (this.d3d9Width > this.d3d9Height)
					this.d3d9Height = this.d3d9Width;
				else if (this.d3d9Height > this.d3d9Width)
					this.d3d9Width = this.d3d9Height;
			}
		}

		private void CreateD3D9Texture(GraphicsDevice graphicsDevice, D3D9.Texture texture)
		{
			this.CalculateD3D9Size(graphicsDevice);

			// Check if we need to resize
			if (this.d3d9Width == this.Width && this.d3d9Height == this.Height)
			{
				this.d3d9Texture = texture;
				return;
			}

			this.d3d9Texture = CreateD3D9Texture(graphicsDevice.d3d9Device, this.d3d9Width, this.d3d9Height, TextureUsage.None);

			SharpDX.DataRectangle input = texture.LockRectangle(0, D3D9.LockFlags.ReadOnly);
			SharpDX.DataStream inputStream = new SharpDX.DataStream(input.DataPointer, this.Height * input.Pitch, true, false);

			SharpDX.DataRectangle output = this.d3d9Texture.LockRectangle(0, D3D9.LockFlags.None);
			SharpDX.DataStream outputStream = new SharpDX.DataStream(output.DataPointer, this.d3d9Height * output.Pitch, true, true);

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
			this.d3d9Texture.UnlockRectangle(0);

			texture.Dispose(); // Get rid of old texture
		}

		private static Texture FromStreamInternal(GraphicsDevice graphicsDevice, Stream stream, int width, int height, int colorKey)
		{
			D3D9.Texture texture = D3D9.Texture.FromStream(
				graphicsDevice.d3d9Device,
				stream,
				width,
				height,
				1,
				D3D9.Usage.None,
				D3D9.Format.A8R8G8B8,
				D3D9.Pool.Managed,
				D3D9.Filter.Point,
				D3D9.Filter.Point,
				colorKey);

			return new Texture(graphicsDevice, texture, width, height);
		}

		private Color[] GetPixelsInternal()
		{
			Color[] pixels = new Color[this.Width * this.Height];

			SharpDX.DataRectangle dataRectangle = this.d3d9Texture.LockRectangle(0, D3D9.LockFlags.ReadOnly);

			using (SharpDX.DataStream dataStream = new SharpDX.DataStream(dataRectangle.DataPointer, this.d3d9Width * dataRectangle.Pitch, true, false))
			{
				int x = 0;
				int y = 0;

				for (int i = 0; i < pixels.Length; i++)
				{
					dataStream.Seek((y * dataRectangle.Pitch) + (x * 4), SeekOrigin.Begin);

					byte b = (byte)dataStream.ReadByte();
					byte g = (byte)dataStream.ReadByte();
					byte r = (byte)dataStream.ReadByte();
					byte a = (byte)dataStream.ReadByte();

					pixels[i] = new Color(r, g, b, a);

					x++;
					if (x >= this.Width)
					{
						x = 0;
						y++;
					}
				}
			}

			this.d3d9Texture.UnlockRectangle(0);

			return pixels;
		}
	}
}
