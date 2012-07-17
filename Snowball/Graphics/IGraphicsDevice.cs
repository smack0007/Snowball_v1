using System;
using System.IO;

namespace Snowball.Graphics
{
	public interface IGraphicsDevice
	{
		/// <summary>
		/// Whether or not the graphics device has been created.
		/// </summary>
		bool IsDeviceCreated { get; }
		
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
		/// Loads a TextureFont from an XML file.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		TextureFont LoadTextureFont(Stream stream, Color? colorKey);

		/// <summary>
		/// Constructs a TextureFont.
		/// </summary>
		/// <param name="fontName"></param>
		/// <param name="fontSize"></param>
		/// <param name="antialias"></param>
		/// <returns></returns>
		TextureFont ConstructTextureFont(string fontName, int fontSize, bool antialias);
	}
}
