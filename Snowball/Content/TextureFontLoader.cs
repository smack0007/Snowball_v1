using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for TextureFont(s).
	/// </summary>
	public class TextureFontLoader : GraphicsContentTypeLoader<TextureFont, LoadTextureFontArgs>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
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
