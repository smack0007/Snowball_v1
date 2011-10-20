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

		class TextureFontInformation : TextureInformation
		{
		}

		class SpriteSheetInformation : TextureInformation
		{
			public int FrameWidth;
			public int FrameHeight;
			public int FramePaddingX;
			public int FramePaddingY;
		}

		IServiceProvider services;
		IContentStorageSystem storage;

		IGraphicsDevice graphicsDevice;

		Dictionary<string, TextureInformation> textures;
		Dictionary<string, TextureFontInformation> textureFonts;
		Dictionary<string, SpriteSheetInformation> spriteSheets;

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

			this.textures = new Dictionary<string, TextureInformation>();
			this.textureFonts = new Dictionary<string, TextureFontInformation>();
			this.spriteSheets = new Dictionary<string, SpriteSheetInformation>();
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
				throw new InvalidOperationException("A Texture is already registered under the key \"" + key + "\".");

			if(string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			this.textures.Add(key, new TextureInformation()
			{
				FileName = fileName,
				ColorKey = colorKey
			});
		}

		public Texture LoadTexture(string key)
		{
			if(!this.textures.ContainsKey(key))
				throw new InvalidOperationException("No Texture is registered under the key \"" + key + "\".");

			TextureInformation info = this.textures[key];
			return this.GetGraphicsDevice().LoadTexture(this.storage.GetStream(info.FileName), info.ColorKey);
		}

		public void RegisterTextureFont(string key, string fileName, Color? colorKey)
		{
			if(this.textureFonts.ContainsKey(key))
				throw new InvalidOperationException("A TextureFont is already registered under the key \"" + key + "\".");

			if(string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			this.textureFonts.Add(key, new TextureFontInformation()
			{
				FileName = fileName,
				ColorKey = colorKey
			});
		}

		public TextureFont LoadTextureFont(string key)
		{
			if(!this.textureFonts.ContainsKey(key))
				throw new InvalidOperationException("No TextureFont is registered under the key \"" + key + "\".");

			TextureFontInformation info = this.textureFonts[key];
			return this.GetGraphicsDevice().LoadTextureFont(this.storage.GetStream(info.FileName), info.ColorKey);
		}

		public void RegisterSpriteSheet(string key, string fileName, Color? colorKey, int frameWidth, int frameHeight, int framePaddingX, int framePaddingY)
		{
			if(this.spriteSheets.ContainsKey(key))
			{
				throw new InvalidOperationException("A SpriteSheet is already registered under the key \"" + key + "\".");
			}

			if(string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			SpriteSheet.EnsureConstructorParams(frameWidth, frameHeight, framePaddingX, framePaddingY);

			this.spriteSheets.Add(key, new SpriteSheetInformation()
			{
				FileName = fileName,
				ColorKey = colorKey,
				FrameWidth = frameWidth,
				FrameHeight = frameHeight,
				FramePaddingX = framePaddingX,
				FramePaddingY = framePaddingY
			});
		}

		public SpriteSheet LoadSpriteSheet(string key)
		{
			if(!this.spriteSheets.ContainsKey(key))
				throw new InvalidOperationException("No SpriteSheet is registered under the key \"" + key + "\".");

			SpriteSheetInformation info = this.spriteSheets[key];
			return new SpriteSheet(this.GetGraphicsDevice().LoadTexture(this.storage.GetStream(info.FileName), info.ColorKey), info.FrameWidth, info.FrameHeight, info.FramePaddingX, info.FramePaddingY);
		}
	}
}
