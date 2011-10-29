using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for Texture(s).
	/// </summary>
	public class TextureLoader : GraphicsContentTypeLoader<Texture, LoadTextureArgs>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
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
