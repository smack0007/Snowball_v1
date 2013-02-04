using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Snowball.Content
{
	/// <summary>
	/// Base class for loaders of specific content types.
	/// </summary>
	/// <typeparam name="TContent"></typeparam>
	/// <typeparam name="TLoadContentArgs"></typeparam>
	public abstract class ContentTypeLoader<TContent> : IContentTypeLoader<TContent>
		where TContent : class
	{        
        /// <summary>
        /// The list of LoadContentArgs used by this ContentTypeLoader.
        /// </summary>
        protected abstract Type[] LoadContentArgsTypes { get; }
	
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
			if (!this.LoadContentArgsTypes.Contains(args.GetType()))
				throw new ArgumentException(string.Format("Args must be one of the following types: {0}.", string.Join(", ", this.LoadContentArgsTypes.Select(x => x.FullName))));

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
						
			TContent content = this.LoadContent(stream, args);

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
		protected abstract TContent LoadContent(Stream stream, LoadContentArgs args);
	}
}
