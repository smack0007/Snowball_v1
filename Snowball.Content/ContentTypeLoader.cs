using System;
using System.Collections.Generic;
using System.IO;

namespace Snowball.Content
{
	/// <summary>
	/// Base class for loaders of specific content types.
	/// </summary>
	/// <typeparam name="TContent"></typeparam>
	/// <typeparam name="TLoadContentArgs"></typeparam>
	public abstract class ContentTypeLoader<TContent, TLoadContentArgs> : IContentTypeLoader<TContent>
		where TContent : class
		where TLoadContentArgs : LoadContentArgs
	{
        private static readonly ContentFormat[] contentFormats = new ContentFormat[] { ContentFormat.Default };

        /// <summary>
        /// The list of content formats which can be loaded.
        /// </summary>
        public virtual ContentFormat[] ValidContentFormats
        {
            get { return contentFormats; }
        }
	
		/// <summary>
		/// The service container.
		/// </summary>
		public IServiceProvider Services
		{
			get;
			private set;
		}

		/// <summary>
		/// The parent content loader which is managing the type loader.
		/// </summary>
		public IContentLoader ContentLoader
		{
			get;
			set;
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
		}
				
		/// <summary>
		/// Allows the content type loader to ensure that all necessary args are provided. An exception should
		/// be thrown if the provided args are not sufficient.
		/// </summary>
		/// <param name="args"></param>
		public virtual void EnsureArgs(LoadContentArgs args)
		{
			if (!(args is TLoadContentArgs))
				throw new ArgumentException(string.Format("Args must be of type {0}.", typeof(TLoadContentArgs)));

			this.EnsureContentArgs((TLoadContentArgs)args);
		}

		protected virtual void EnsureContentArgs(TLoadContentArgs args)
		{
			if (string.IsNullOrEmpty(args.FileName))
				throw new ContentLoadException("FileName must be provided.");
		}

		/// <summary>
		/// Loads content from the given storage container using the given args.
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

			this.EnsureArgs(args);
						
			Stream stream = storage.GetStream(args.FileName);
						
			TContent content = this.LoadContent(stream, (TLoadContentArgs)args);

			if (content == null)
				throw new ContentLoadException(string.Format("Failed while loading content type {0}.", typeof(TContent)));

			return content;
		}

		/// <summary>
		/// Helper method which simply throws an exception if the ContentLoader property is not set.
		/// </summary>
		protected void EnsureContentLoader()
		{
			if (this.ContentLoader == null)
				throw new InvalidOperationException(string.Format("The ContentLoader property must be set for {0} to load content.", this.GetType()));
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
