using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for SpriteSheet(s).
	/// </summary>
	public class SpriteSheetLoader : GraphicsContentTypeLoader<SpriteSheet>
	{
        private static readonly Type[] loadContentArgsTypes = new Type[] { typeof(LoadSpriteSheetArgs), typeof(LoadSpriteSheetXmlArgs) };

        protected override Type[] LoadContentArgsTypes
        {
            get { return loadContentArgsTypes; }
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public SpriteSheetLoader(IServiceProvider services)
			: base(services)
		{
		}

		public override void EnsureArgs(LoadContentArgs args)
		{
            base.EnsureArgs(args);

            if (args is LoadSpriteSheetArgs)
            {
                LoadSpriteSheetArgs spriteSheetArgs = (LoadSpriteSheetArgs)args;
                SpriteSheet.EnsureConstructorArgs(spriteSheetArgs.FrameWidth, spriteSheetArgs.FrameHeight, spriteSheetArgs.FramePaddingX, spriteSheetArgs.FramePaddingY);
            }
		}

		protected override SpriteSheet LoadContent(Stream stream, LoadContentArgs args)
		{
            SpriteSheet spriteSheet = null;

            if (args is LoadSpriteSheetArgs)
            {
                LoadSpriteSheetArgs spriteSheetArgs = (LoadSpriteSheetArgs)args;
                
                spriteSheet = new SpriteSheet(
                    this.GetGraphicsDevice().LoadTexture(stream, spriteSheetArgs.ColorKey),
                    spriteSheetArgs.FrameWidth,
                    spriteSheetArgs.FrameHeight,
                    spriteSheetArgs.FramePaddingX,
                    spriteSheetArgs.FramePaddingY);
            }
            else if (args is LoadSpriteSheetXmlArgs)
            {
                spriteSheet = this.GetGraphicsDevice().LoadSpriteSheet(
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

			return spriteSheet;
		}
	}
}
