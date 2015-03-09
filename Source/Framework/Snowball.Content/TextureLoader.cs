using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for Texture(s).
	/// </summary>
	public class TextureLoader : GraphicsContentTypeLoader<Texture>
	{
        private static readonly Type[] loadContentArgsTypes = new Type[] { typeof(LoadTextureArgs) };

        protected override Type[] LoadContentArgsTypes
        {
            get { return loadContentArgsTypes; }
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public TextureLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override Texture LoadContent(Stream stream, LoadContentArgs args)
		{
            LoadTextureArgs textureArgs = (LoadTextureArgs)args;
            return this.GetGraphicsDevice().LoadTexture(stream, textureArgs.ColorKey);
		}
	}
}
