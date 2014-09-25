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
		IStorage storage;

		Dictionary<Type, IContentTypeLoader> contentTypeLoaders;

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

			this.contentTypeLoaders = new Dictionary<Type, IContentTypeLoader>();
			this.contentTypeLoaders[typeof(Effect)] = new EffectLoader(this.services) { ContentLoader = this };
			this.contentTypeLoaders[typeof(SoundEffect)] = new SoundEffectLoader(this.services) { ContentLoader = this };
			this.contentTypeLoaders[typeof(SpriteSheet)] = new SpriteSheetLoader(this.services) { ContentLoader = this };
			this.contentTypeLoaders[typeof(Texture)] = new TextureLoader(this.services) { ContentLoader = this };
			this.contentTypeLoaders[typeof(TextureFont)] = new TextureFontLoader(this.services) { ContentLoader = this };
		}

		/// <summary>
		/// Adds a ContentTypeLoader.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="contentTypeLoader"></param>
		public void AddContentTypeLoader<T>(IContentTypeLoader<T> contentTypeLoader)
			where T : class
		{
			if (contentTypeLoader == null)
				throw new ArgumentNullException("contentTypeLoader");

			Type contentType = typeof(T);

			if (this.contentTypeLoaders.ContainsKey(contentType))
				throw new InvalidOperationException(string.Format("A content loader is already registered for the content type \"{0}\".", contentType));

			this.contentTypeLoaders[contentType] = contentTypeLoader;
			contentTypeLoader.ContentLoader = this;
		}

		/// <summary>
		/// Removes a ContentTypeLoader.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private void RemoveContentTypeLoader<T>()
		{
			this.contentTypeLoaders.Remove(typeof(T));
		}
		
		private IContentTypeLoader<T> GetContentTypeLoader<T>()
			where T : class
		{
			Type contentType = typeof(T);

			if (!this.contentTypeLoaders.ContainsKey(contentType))
				throw new InvalidOperationException(string.Format("No content loader registered for content type \"{0}\".", contentType));

			return (IContentTypeLoader<T>)this.contentTypeLoaders[contentType];
		}

		/// <summary>
		/// Allows the content loader to ensure the given args are valid for the given content type.
		/// </summary>
		/// <param name="args"></param>
		public void EnsureArgs<T>(LoadContentArgs args)
			where T : class
		{
			if (args == null)
				throw new ArgumentNullException("args");

			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			loader.EnsureArgs(args);
		}

		/// <summary>
		/// Loads content.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		public T Load<T>(LoadContentArgs args)
			where T : class
		{
			if (args == null)
				throw new ArgumentNullException("args");

			IContentTypeLoader<T> loader = this.GetContentTypeLoader<T>();
			return loader.Load(this.storage, args);
		}
	}
}
