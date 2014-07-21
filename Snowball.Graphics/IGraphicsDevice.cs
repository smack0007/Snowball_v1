using System;
using System.IO;

namespace Snowball.Graphics
{
	public interface IGraphicsDevice
	{	
		/// <summary>
		/// The width of the display area.
		/// </summary>
		int BackBufferWidth { get; }

		/// <summary>
		/// The height of the display area.
		/// </summary>
		int BackBufferHeight { get; }

		/// <summary>
		/// Loads an Effect.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		Effect LoadEffect(Stream stream);
	
		/// <summary>
		/// Loads a texture.
		/// </summary>
		/// <param name="stream">The stream to load from.</param>
		/// <param name="colorKey">A color which should be used for transparency.</param>
		/// <returns></returns>
		Texture LoadTexture(Stream stream, Color? colorKey);
		
        /// <summary>
        /// Loads a SpriteSheet from an XML file.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="loadTextureFunc"></param>
        /// <returns></returns>
        SpriteSheet LoadSpriteSheet(Stream stream, Func<string, Color?, Texture> loadTextureFunc);

		/// <summary>
		/// Loads a TextureFont from an XML file.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="loadTextureFunc"></param>
		/// <returns></returns>
		TextureFont LoadTextureFont(Stream stream, Func<string, Color?, Texture> loadTextureFunc);
	}
}
