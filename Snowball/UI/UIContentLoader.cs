using System;
using System.Reflection;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Storage;

namespace Snowball.UI
{
	public class UIContentLoader : IUIContentLoader
	{
		ContentLoader contentLoader;

		public UIContentLoader(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.contentLoader = new ContentLoader(services, new EmbeddedResourcesStorage(Assembly.GetExecutingAssembly(), "Snowball.UI"));

			this.contentLoader.Register<TextureFont>("Font", new LoadTextureFontArgs()
			{
				FileName = "Font.xml",
				UseCache = true
			});

			this.contentLoader.Register<Texture>("Button", new LoadTextureArgs()
			{
				FileName = "Button.png",
				UseCache = true
			});
		}

		public TextureFont LoadFont()
		{
			return this.contentLoader.Load<TextureFont>("Font");
		}

		public Texture LoadButtonTexture()
		{
			return this.contentLoader.Load<Texture>("Button");
		}
	}
}
