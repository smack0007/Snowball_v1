using System;
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

		protected override void EnsureArgs(LoadTextureFontArgs args)
		{
			base.EnsureArgs(args);

			if (args.LoadType == ContentLoadType.Construct)
			{
				if (string.IsNullOrEmpty(args.FontName))
				{
					throw new ContentLoadException("FontName is required when loading a TextureFont using the Construct LoadType.");
				}

				if (args.FontSize <= 0)
				{
					throw new ContentLoadException("FontSize must be >= 0 when loading a TextureFont using the Construct LoadType.");
				}
			}
		}

		protected override TextureFont LoadContent(Stream stream, LoadTextureFontArgs args)
		{
			if (args.LoadType == ContentLoadType.FromFile)
			{
				return this.GetGraphicsDevice().LoadTextureFont(stream, args.ColorKey);
			}
			else if (args.LoadType == ContentLoadType.Construct)
			{
				return this.GetGraphicsDevice().ConstructTextureFont(args.FontName, args.FontSize, args.Antialias);
			}

			return null;
		}
	}
}
