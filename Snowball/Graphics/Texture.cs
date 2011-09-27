using System;
using System.IO;
using SlimDX.Direct3D9;
using System.Drawing;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which contains an image.
	/// </summary>
	public class Texture : GameResource
	{
		internal SlimDX.Direct3D9.Texture texture;

		int width;
		int height;

		Color[] colorData;

		public int Width
		{
			get { return this.width; }
		}

		public int Height
		{
			get { return this.height; }
		}

		internal Texture(GraphicsManager graphics, int width, int height)
			: base()
		{
			if(graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}

			this.texture = new SlimDX.Direct3D9.Texture(graphics.device, width, height, 0, SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Managed);
			this.width = width;
			this.height = height;
		}

		internal Texture(SlimDX.Direct3D9.Texture texture, int width, int height)
		{
			if(texture == null)
			{
				throw new ArgumentNullException("texture");
			}

			this.texture = texture;
			this.width = width;
			this.height = height;
		}

		internal static Texture Load(GraphicsManager graphics, string fileName, Color? colorKey)
		{
			if(!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file \"" + fileName + "\".");

			Image image = Image.FromFile(fileName);
			int width = image.Width;
			int height = image.Height;
			image.Dispose();

			int argb = 0;
			if(colorKey != null)
				argb = colorKey.Value.ToArgb();

			SlimDX.Direct3D9.Texture texture = SlimDX.Direct3D9.Texture.FromFile(graphics.device, fileName, width, height, 0,
																				 SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8,
																				 SlimDX.Direct3D9.Pool.Managed, SlimDX.Direct3D9.Filter.Point,
																				 SlimDX.Direct3D9.Filter.Point, argb);

			return new Texture(texture, width, height);
		}
				
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.texture != null)
				{
					this.texture.Dispose();
					this.texture = null;
				}
			}
		}

		public Color[] GetColorData()
		{
			if(this.colorData == null)
			{
				this.colorData = new Color[this.width * this.height];

				SlimDX.DataRectangle dataRectangle = this.texture.LockRectangle(0, LockFlags.ReadOnly);

				for(int i = 0; i < this.colorData.Length; i++)
				{
					byte b = (byte)dataRectangle.Data.ReadByte();
					byte g = (byte)dataRectangle.Data.ReadByte();
					byte r = (byte)dataRectangle.Data.ReadByte();
					byte a = (byte)dataRectangle.Data.ReadByte();

					this.colorData[i] = new Color(r, g, b, a);
				}

				this.texture.UnlockRectangle(0);
			}

			return this.colorData;
		}
	}
}
