using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	public class TextureLoader : GraphicsContentTypeLoader<Texture, LoadTextureArgs>
	{
		public TextureLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override Texture LoadContent(Stream stream, LoadTextureArgs args)
		{
			return this.GetGraphicsDevice().LoadTexture(stream, args.ColorKey);
		}
	}
}
