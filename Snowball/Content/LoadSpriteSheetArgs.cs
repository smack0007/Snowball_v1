using System;

namespace Snowball.Content
{
	public class LoadSpriteSheetArgs : LoadTextureArgs
	{
		public int FrameWidth
		{
			get;
			set;
		}

		public int FrameHeight
		{
			get;
			set;
		}

		public int FramePaddingX
		{
			get;
			set;
		}

		public int FramePaddingY
		{
			get;
			set;
		}

		public LoadSpriteSheetArgs()
			: base()
		{
		}
	}
}
