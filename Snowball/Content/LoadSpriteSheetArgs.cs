using System;

namespace Snowball.Content
{
	/// <summary>
	/// Args for loading SpriteSheet(s).
	/// </summary>
	public class LoadSpriteSheetArgs : LoadTextureArgs
	{
		/// <summary>
		/// The width of each frame in the sheet.
		/// </summary>
		public int FrameWidth
		{
			get;
			set;
		}

		/// <summary>
		/// The height of each frame in the sheet.
		/// </summary>
		public int FrameHeight
		{
			get;
			set;
		}

		/// <summary>
		/// The padding in the horizontal direction between each frame in the sheet.
		/// </summary>
		public int FramePaddingX
		{
			get;
			set;
		}

		/// <summary>
		/// The padding in the vertical direction between each frame in the sheet.
		/// </summary>
		public int FramePaddingY
		{
			get;
			set;
		}

		/// <summary>
		/// If true, the color data for the underlying Texture from the SpriteSheet will be cached when the
		/// SpriteSheet is loaded.
		/// </summary>
		public bool CacheColorData
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public LoadSpriteSheetArgs()
			: base()
		{
		}
	}
}
