﻿using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for TextureFont(s).
	/// </summary>
	public class TextureFontLoader : GraphicsContentTypeLoader<TextureFont, LoadTextureFontArgs>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public TextureFontLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override TextureFont LoadContent(Stream stream, LoadTextureFontArgs args)
		{
			this.EnsureContentLoader();

			return this.GetGraphicsDevice().LoadTextureFont(
				stream,
				(fileName, colorKey) =>
				{
					return this.ContentLoader.Load<Texture>(new LoadTextureArgs()
					{
						FileName = Path.Combine(Path.GetDirectoryName(args.FileName), fileName),
						ColorKey = colorKey
					});
				});
		}
	}
}
