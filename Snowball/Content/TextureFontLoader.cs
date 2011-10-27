using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	public class TextureFontLoader : GraphicsContentTypeLoader<TextureFont, LoadTextureFontArgs>
	{
		public TextureFontLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override TextureFont LoadContent(Stream stream, LoadTextureFontArgs args)
		{
			return this.GetGraphicsDevice().LoadTextureFont(stream, args.ColorKey);
		}
	}
}
