using System;
using System.Collections.Generic;
using System.IO;
using Snowball.Storage;

namespace Snowball.Content
{
	/// <summary>
	/// Base class for loaders of specific content types.
	/// </summary>
	/// <typeparam name="TContent"></typeparam>
	/// <typeparam name="TLoadContentArgs"></typeparam>
	public abstract class ContentTypeLoader<TContent, TLoadContentArgs> : IContentTypeLoader<TContent>
		where TLoadContentArgs : LoadContentArgs
	{
		Dictionary<string, TLoadContentArgs> contentArgs;
		Dictionary<string, TContent> contentCache;

		/// <summary>
		/// The service container.
		/// </summary>
		public IServiceProvider Services
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public ContentTypeLoader(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.Services = services;
			
			this.contentArgs = new Dictionary<string, TLoadContentArgs>();
			this.contentCache = new Dictionary<string, TContent>();
		}

		/// <summary>
		/// Registers content for loading later.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="args"></param>
		public void Register(string key, LoadContentArgs args)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			if (args == null)
				throw new ArgumentNullException("args");

			if (this.contentArgs.ContainsKey(key))
				throw new InvalidOperationException("A " + typeof(TContent).FullName + " is already registered under the key \"" + key + "\".");

			if (!(args is TLoadContentArgs))
				throw new ArgumentException("Args must be of type " + typeof(TLoadContentArgs).FullName + ".");

			this.EnsureArgs((TLoadContentArgs)args);
			this.contentArgs.Add(key, (TLoadContentArgs)args);
		}

		/// <summary>
		/// Returns true if content has been registered under the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsRegistered(string key)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			return this.contentArgs.ContainsKey(key);
		}

		/// <summary>
		/// Allows the content type loader to ensure that all necessary args are provided. An exception should
		/// be thrown if the provided args are not sufficient.
		/// </summary>
		/// <param name="args"></param>
		protected virtual void EnsureArgs(TLoadContentArgs args)
		{
			if (args.RequiresStream && string.IsNullOrEmpty(args.FileName))
				throw new ContentLoadException("FileName must be provided.");
		}

		/// <summary>
		/// Loads previously registered content using the given storage container.
		/// </summary>
		/// <param name="storage"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public TContent Load(IStorage storage, string key)
		{
			if (storage == null)
				throw new ArgumentNullException("storage");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			if (!this.contentArgs.ContainsKey(key))
				throw new ContentLoadException("No " + typeof(TContent).FullName + " is registered under the key \"" + key + "\".");

			TLoadContentArgs args = this.contentArgs[key];

			if (args.UseCache && this.contentCache.ContainsKey(key))
				return this.contentCache[key];

			Stream stream = null;
			if (args.RequiresStream)
				stream = storage.GetStream(args.FileName);

			TContent content = this.LoadContent(stream, args);

			if (content == null)
				throw new ContentLoadException("Failed to load content type " + typeof(TContent) + " registered under the key \"" + key + "\".");

			if (args.UseCache)
				this.contentCache[key] = content;

			return content;
		}

		/// <summary>
		/// Loads content using the given storage container.
		/// </summary>
		/// <param name="storage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public TContent Load(IStorage storage, LoadContentArgs args)
		{
			if (storage == null)
				throw new ArgumentNullException("storage");

			if (args == null)
				throw new ArgumentNullException("args");

			if (!(args is TLoadContentArgs))
				throw new ArgumentException("Args must be of type " + typeof(TLoadContentArgs).FullName + ".");

			TLoadContentArgs typedArgs = (TLoadContentArgs)args;

			this.EnsureArgs(typedArgs);

			Stream stream = null;
			if (args.RequiresStream)
				stream = storage.GetStream(args.FileName);
						
			TContent content = this.LoadContent(stream, typedArgs);

			if (content == null)
				throw new ContentLoadException("Failed to load content type " + typeof(TContent) + ".");

			return content;
		}

		/// <summary>
		/// Impelemented in individual content type loaders.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		protected abstract TContent LoadContent(Stream stream, TLoadContentArgs args);
	}
}
