using System;
using System.Collections.Generic;

namespace Snowball.Graphics
{
	/// <summary>
	/// Wraps a texture and keeps track of the frames within it.
	/// </summary>
	public sealed class SpriteSheet
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
		/// The amount of padding between each frame in the horizontal direction.
		/// </summary>
		public int FramePaddingX
		{
			get;
			private set;
		}

		/// <summary>
		/// The amount of padding between each frame in the vertical direction.
		/// </summary>
		public int FramePaddingY
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
			if(texture == null)
				throw new ArgumentNullException("texture");

			EnsureConstructorParams(frameWidth, frameHeight, framePaddingX, framePaddingY);

			this.Texture = texture;
			this.FrameHeight = frameWidth;
			this.FrameHeight = frameHeight;
			this.FramePaddingX = framePaddingX;
			this.FramePaddingY = framePaddingY;

			this.rectangles = new List<Rectangle>();

			for(int y = framePaddingY; y < texture.Height; y += frameHeight + framePaddingY)
			{
				for(int x = framePaddingX; x < texture.Width; x += frameWidth + framePaddingX)
				{
					this.rectangles.Add(new Rectangle(x, y, frameWidth, frameHeight));
				}
			}
		}

		internal static void EnsureConstructorParams(int frameWidth, int frameHeight, int framePaddingX, int framePaddingY)
		{
			if(frameWidth <= 0)
				throw new ArgumentOutOfRangeException("frameWidth", "frameWidth must be > 0.");

			if(frameHeight <= 0)
				throw new ArgumentOutOfRangeException("frameHeight", "frameHeight must be > 0.");

			if(framePaddingX < 0)
				throw new ArgumentOutOfRangeException("framePaddingX", "framePaddingX must be >= 0.");

			if(framePaddingY < 0)
				throw new ArgumentOutOfRangeException("framePaddingY", "framePaddingY must be >= 0.");
		}
	}
}
