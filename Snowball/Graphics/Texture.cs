using System;
using SlimDX.Direct3D9;

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

		internal Texture(SlimDX.Direct3D9.Texture texture, int width, int height)
		{
			if(texture == null)
				throw new ArgumentNullException("texture");

			this.texture = texture;
			this.width = width;
			this.height = height;
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
