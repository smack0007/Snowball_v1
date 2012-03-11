using System;
using System.Collections.Generic;
using Snowball.Graphics;
using Snowball.Sound;
using Snowball.Storage;

namespace Snowball.Content
{
	/// <summary>
	/// Performs loading of content in a game.
	/// </summary>
	public class ContentLoader : IContentLoader
	{
		IServiceProvider services;
		IStorage storage;

		Dictionary<Type, object> contentTypeLoaders;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public ContentLoader(IServiceProvider services)
			: this(services, new FileSystemStorage())
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="storage"></param>
		public ContentLoader(IServiceProvider services, IStorage storage)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			if (storage == null)
				throw new ArgumentNullException("storage");

			this.services = services;
			this.storage = storage;

			this.contentTypeLoaders = new Dictionary<Type, object>();
			this.contentTypeLoaders[typeof(Texture)] = new TextureLoader(this.services);
			this.contentTypeLoaders[typeof(TextureFont)] = new TextureFontLoader(this.services);
			this.contentTypeLoaders[typeof(SpriteSheet)] = new SpriteSheetLoader(this.services);
			this.contentTypeLoaders[typeof(SoundEffect)] = new SoundEffectLoader(this.services);
		}

		private IContentTypeLoader<T> GetContentTypeLoader<T>()
		{
			Type contentType = typeof(T);

			if (!this.contentTypeLoaders.ContainsKey(contentType))
				throw new InvalidOperationException("No content loader registered for type " + contentType.FullName + ".");

			return (IContentTypeLoader<T>)this.contentTypeLoaders[contentType];
		}

		/// <summary>
		/// Registers content for loading later.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="args"></param>
		public void Register<T>(string key, LoadContentArgs args)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			loader.Register(key, args);
		}

		/// <summary>
		/// Returns true if content has been registered under the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsRegistered<T>(string key)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			return loader.IsRegistered<T>(key);
		}

		/// <summary>
		/// Loads a previously registered content.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Load<T>(string key)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			return loader.Load(this.storage, key);
		}

		/// <summary>
		/// Loads content.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		public T Load<T>(LoadContentArgs args)
		{
			if (args == null)
				throw new ArgumentNullException("args");

			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			return loader.Load(this.storage, args);
		}
	}
}
