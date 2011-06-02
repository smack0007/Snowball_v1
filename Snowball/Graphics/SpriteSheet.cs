using System;
using System.Collections.Generic;

namespace Snowball.Graphics
{
	/// <summary>
	/// Wraps a texture and keeps track of the frames within it.
	/// </summary>
	public class SpriteSheet
	{
		List<Rectangle> rectangles;
		
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
		/// The width of frames in the SpriteSheet.
		/// </summary>
		public int FrameWidth
		{
			get;
			private set;
		}

		/// <summary>
		/// The height of frames in the SpriteSheet.
		/// </summary>
		public int FrameHeight
		{
			get;
			private set;
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
		{
			if(texture == null)
				throw new ArgumentNullException("texture");

			this.Texture = texture;
			
			this.rectangles = new List<Rectangle>();

			for(int y = 0; y < texture.Height; y += frameHeight)
			{
				for(int x = 0; x < texture.Width; x += frameWidth)
				{
					this.rectangles.Add(new Rectangle(x, y, frameWidth, frameHeight));
				}
			}
		}
	}
}
