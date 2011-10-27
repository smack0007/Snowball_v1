using System;
using System.Collections.Generic;
using Snowball.Graphics;
using Snowball.Sound;

namespace Snowball.Content
{
	/// <summary>
	/// Performs loading of content in a game.
	/// </summary>
	public class ContentLoader : IContentLoader
	{
		IServiceProvider services;
		IContentStorageSystem storage;

		Dictionary<Type, object> contentTypeLoaders;

		public ContentLoader(IServiceProvider services)
			: this(services, new FileStorageSystem())
		{
		}

		public ContentLoader(IServiceProvider services, IContentStorageSystem storage)
		{
			if(services == null)
				throw new ArgumentNullException("services");

			if(storage == null)
				throw new ArgumentNullException("storage");

			this.services = services;
			this.storage = storage;

			this.contentTypeLoaders = new Dictionary<Type, object>();
			this.contentTypeLoaders[typeof(Texture)] = new TextureLoader(this.services);
			this.contentTypeLoaders[typeof(TextureFont)] = new TextureFontLoader(this.services);
			this.contentTypeLoaders[typeof(SpriteSheet)] = new SpriteSheetLoader(this.services);
			//this.contentTypeLoaders[typeof(SoundEffect)] = new SoundEffectLoader(this.services);
		}

		private IContentTypeLoader<T> GetContentTypeLoader<T>()
		{
			Type contentType = typeof(T);

			if(!this.contentTypeLoaders.ContainsKey(contentType))
				throw new InvalidOperationException("No content loader registered for type " + contentType.FullName + ".");

			return (IContentTypeLoader<T>)this.contentTypeLoaders[contentType];
		}

		public void Register<T>(string key, LoadContentArgs args)
		{
			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			loader.Register(key, args);
		}

		public T Load<T>(string key)
		{
			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			return loader.Load(this.storage, key);
		}
	}
}
