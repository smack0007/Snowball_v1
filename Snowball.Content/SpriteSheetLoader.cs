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
        private static readonly ContentFormat[] contentFormats = new ContentFormat[] { ContentFormat.Default, ContentFormat.Xml };

        public override ContentFormat[] ValidContentFormats
        {
            get { return contentFormats; }
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public SpriteSheetLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override void EnsureContentArgs(LoadSpriteSheetArgs args)
		{
			base.EnsureContentArgs(args);

            if (args.Format == ContentFormat.Default)
                SpriteSheet.EnsureConstructorArgs(args.FrameWidth, args.FrameHeight, args.FramePaddingX, args.FramePaddingY);
		}

		protected override SpriteSheet LoadContent(Stream stream, LoadSpriteSheetArgs args)
		{
            SpriteSheet spriteSheet = null;

            if (args.Format == ContentFormat.Default)
            {
                spriteSheet = new SpriteSheet(this.GetGraphicsDevice().LoadTexture(stream, args.ColorKey), args.FrameWidth, args.FrameHeight, args.FramePaddingX, args.FramePaddingY);
            }
            else if (args.Format == ContentFormat.Xml)
            {
                this.GetGraphicsDevice().LoadSpriteSheet(
                    stream,
				    (fileName, colorKey) =>
				    {
					    return this.ContentLoader.Load<Texture>(new LoadTextureArgs()
					    {
						    FileName = Path.Combine(Path.GetDirectoryName(args.FileName), fileName),
						    ColorKey = colorKey
					    });
				    });
            }

			if (args.CacheColorData)
				spriteSheet.CacheColorData();

			return spriteSheet;
		}
	}
}
