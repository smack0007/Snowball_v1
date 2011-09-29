using System;
using System.IO;
using SlimDX.Direct3D9;
using System.Drawing;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which contains an image.
	/// </summary>
	public sealed class Texture : GameResource
	{
		internal SlimDX.Direct3D9.Texture InternalTexture;
				
		Color[] colorData;

		public int Width
		{
			get;
			protected set;
		}

		public int Height
		{
			get;
			protected set;
		}
				
		internal Texture(GraphicsDevice graphicsManager, int width, int height)
			: base()
		{
			if(graphicsManager == null)
			{
				throw new ArgumentNullException("graphicsManager");
			}

			this.InternalTexture = new SlimDX.Direct3D9.Texture(graphicsManager.InternalDevice, width, height, 0, SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Managed);
			this.Width = width;
			this.Height = height;
		}

		internal Texture(SlimDX.Direct3D9.Texture texture, int width, int height)
			: base()
		{
			if(texture == null)
			{
				throw new ArgumentNullException("texture");
			}

			this.InternalTexture = texture;
			this.Width = width;
			this.Height = height;
		}
		
		internal static Texture Load(GraphicsDevice graphicsManager, string fileName, Color? colorKey)
		{
			if(graphicsManager == null)
			{
				throw new ArgumentNullException("graphicsManager");
			}

			if(!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file \"" + fileName + "\".");

			Image image = Image.FromFile(fileName);
			int width = image.Width;
			int height = image.Height;
			image.Dispose();

			int argb = 0;
			if(colorKey != null)
				argb = colorKey.Value.ToArgb();

			SlimDX.Direct3D9.Texture texture = SlimDX.Direct3D9.Texture.FromFile(graphicsManager.InternalDevice, fileName, width, height, 0,
																				 SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8,
																				 SlimDX.Direct3D9.Pool.Managed, SlimDX.Direct3D9.Filter.Point,
																				 SlimDX.Direct3D9.Filter.Point, argb);

			return new Texture(texture, width, height);
		}
				
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.InternalTexture != null)
				{
					this.InternalTexture.Dispose();
					this.InternalTexture = null;
				}
			}
		}

		public Color[] GetColorData()
		{
			if(this.colorData == null)
			{
				this.colorData = new Color[this.Width * this.Height];

				SlimDX.DataRectangle dataRectangle = this.InternalTexture.LockRectangle(0, LockFlags.ReadOnly);

				for(int i = 0; i < this.colorData.Length; i++)
				{
					byte b = (byte)dataRectangle.Data.ReadByte();
					byte g = (byte)dataRectangle.Data.ReadByte();
					byte r = (byte)dataRectangle.Data.ReadByte();
					byte a = (byte)dataRectangle.Data.ReadByte();

					this.colorData[i] = new Color(r, g, b, a);
				}

				this.InternalTexture.UnlockRectangle(0);
			}

			return this.colorData;
		}
	}
}
