using System;
using System.Collections.Generic;
using System.IO;

namespace Snowball.Content
{
	/// <summary>
	/// Manages the loading and unloading of content.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	public class ContentManager<TKey> : IContentManager<TKey>, IDisposable
	{
		class ContentInfo
		{
			public Type Type;

			public LoadContentArgs Args;

			public object Content;
		}

		IContentLoader contentLoader;
		
		Dictionary<TKey, ContentInfo> contentData;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public ContentManager(IServiceProvider services)
			: this(new ContentLoader(services))
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="contentLoader"></param>
		public ContentManager(IContentLoader contentLoader)
		{
			if (contentLoader == null)
				throw new ArgumentNullException("contentLoader");

			this.contentLoader = contentLoader;

			this.contentData = new Dictionary<TKey, ContentInfo>();
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~ContentManager()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				this.UnloadAll();
		}

		/// <summary>
		/// Registers content for loading later.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="args"></param>
		public void Register<T>(TKey key, LoadContentArgs args)
			where T : class
		{
			if (key == null)
				throw new ArgumentNullException("key");

			if (args == null)
				throw new ArgumentNullException("args");

			if (this.contentData.ContainsKey(key))
				throw new InvalidOperationException(string.Format("The key \"{1}\" is already registered.", key));

			this.contentLoader.EnsureArgs<T>(args);
						
			this.contentData.Add(key, new ContentInfo()
			{
				Type = typeof(T),
				Args = args
			});
		}

		/// <summary>
		/// Returns true if content has been registered under the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsRegistered(TKey key)
		{			
			return this.contentData.ContainsKey(key);
		}

		private ContentInfo GetContentInfo<T>(TKey key)
			where T : class
		{
			if (!this.contentData.ContainsKey(key))
				throw new ContentLoadException(string.Format("No content is registered under the key \"{1}\".", key));

			ContentInfo contentInfo = this.contentData[key];

			if (typeof(T) != contentInfo.Type)
				throw new ContentLoadException(string.Format("The key \"{0}\" is registered as content type \"{1}\".", key, contentInfo.Type));

			return contentInfo;
		}

		/// <summary>
		/// Loads previously registered content.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Load<T>(TKey key)
			where T : class
		{			
			ContentInfo contentInfo = this.GetContentInfo<T>(key);

			if (contentInfo.Content != null)
				return (T)contentInfo.Content;

			T content = this.contentLoader.Load<T>(contentInfo.Args);

			contentInfo.Content = content;

			return content;
		}

		private void UnloadContent(ContentInfo contentInfo)
		{
			if (contentInfo.Content != null)
			{
				if (contentInfo.Content is IDisposable)
					((IDisposable)contentInfo.Content).Dispose();

				contentInfo.Content = null;
			}
		}

		/// <summary>
		/// Unloads registered content if it is loaded.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public void Unload<T>(TKey key)
			where T : class
		{
			ContentInfo contentInfo = this.GetContentInfo<T>(key);
			this.UnloadContent(contentInfo);			
		}

		/// <summary>
		/// Unloads all registered content if it is loaded.
		/// </summary>
		public void UnloadAll()
		{
			foreach (ContentInfo contentInfo in this.contentData.Values)
				this.UnloadContent(contentInfo);
		}
	}
}
