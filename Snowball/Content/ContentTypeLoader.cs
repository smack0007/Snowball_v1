using System;
using System.Collections.Generic;
using System.IO;
using Snowball.Storage;

namespace Snowball.Content
{
	public abstract class ContentTypeLoader<TContent, TLoadContentArgs> : IContentTypeLoader<TContent>
		where TLoadContentArgs : LoadContentArgs
	{
		Dictionary<string, TLoadContentArgs> contentInformation;

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
			if(services == null)
				throw new ArgumentNullException("services");

			this.Services = services;
			this.contentInformation = new Dictionary<string, TLoadContentArgs>();
		}

		public void Register(string key, LoadContentArgs args)
		{
			if(string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			if(args == null)
				throw new ArgumentNullException("args");

			if(this.contentInformation.ContainsKey(key))
				throw new InvalidOperationException("A " + typeof(TContent).FullName + " is already registered under the key \"" + key + "\".");

			if(!(args is TLoadContentArgs))
				throw new ArgumentException("Args must be of type " + typeof(TLoadContentArgs).FullName + ".");

			this.EnsureArgs((TLoadContentArgs)args);
			this.contentInformation.Add(key, (TLoadContentArgs)args);
		}

		protected virtual void EnsureArgs(TLoadContentArgs args)
		{
		}

		public TContent Load(IStorage storage, string key)
		{
			if(storage == null)
				throw new ArgumentNullException("storage");

			if(string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			if(!this.contentInformation.ContainsKey(key))
				throw new InvalidOperationException("No " + typeof(TContent).FullName + " is registered under the key \"" + key + "\".");

			TLoadContentArgs info = this.contentInformation[key];
			return this.LoadContent(storage.GetStream(info.FileName), info);
		}

		protected abstract TContent LoadContent(Stream stream, TLoadContentArgs information);
	}
}
