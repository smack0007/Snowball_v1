using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for SpriteSheet(s).
	/// </summary>
	public class SpriteSheetLoader : GraphicsContentTypeLoader<SpriteSheet, LoadSpriteSheetArgs>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public SpriteSheetLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override void EnsureArgs(LoadSpriteSheetArgs args)
		{
			base.EnsureArgs(args);
			SpriteSheet.EnsureConstructorParams(args.FrameWidth, args.FrameHeight, args.FramePaddingX, args.FramePaddingY);
		}

		protected override SpriteSheet LoadContent(Stream stream, LoadSpriteSheetArgs args)
		{
			return new SpriteSheet(this.GetGraphicsDevice().LoadTexture(stream, args.ColorKey), args.FrameWidth, args.FrameHeight, args.FramePaddingX, args.FramePaddingY);
		}
	}
}
