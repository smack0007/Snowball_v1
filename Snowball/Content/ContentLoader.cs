using System;
using System.Collections.Generic;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Performs loading of content in a game.
	/// </summary>
	public class ContentLoader : IContentLoader
	{
		class TextureInformation
		{
			public string FileName;
			public Color? ColorKey;
		}

		IServiceProvider services;
		IContentStorageSystem storage;

		IGraphicsDevice graphicsDevice;

		Dictionary<string, TextureInformation> textures;

		public ContentLoader(IServiceProvider services)
			: this(services, new FileStorageSystem())
		{
		}

		public ContentLoader(IServiceProvider services, IContentStorageSystem storage)
		{
			if(services == null)
			{
				throw new ArgumentNullException("services");
			}

			if(storage == null)
			{
				throw new ArgumentNullException("storage");
			}

			this.services = services;
			this.storage = storage;

			this.textures = new Dictionary<string, TextureInformation>();
		}

		private IGraphicsDevice GetGraphicsDevice()
		{
			if(this.graphicsDevice == null)
				this.graphicsDevice = (IGraphicsDevice)this.services.GetRequiredGameService(typeof(IGraphicsDevice));

			return this.graphicsDevice;
		}

		public void RegisterTexture(string key, string fileName, Color? colorKey)
		{
			if(this.textures.ContainsKey(key))
			{
				throw new InvalidOperationException("A texture is already registered under the key \"" + key + "\".");
			}

			if(string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			this.textures.Add(key, new TextureInformation()
			{
				FileName = fileName,
				ColorKey = colorKey
			});
		}

		public Texture LoadTexture(string key)
		{
			if(!this.textures.ContainsKey(key))
			{
				throw new InvalidOperationException("No texture is registered under the key \"" + key + "\".");
			}

			TextureInformation info = this.textures[key];
			return this.GetGraphicsDevice().LoadTexture(this.storage.GetStream(info.FileName), info.ColorKey);
		}
	}
}
