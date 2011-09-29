using System;

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
		int DisplayWidth { get; }

		/// <summary>
		/// The height of the display area.
		/// </summary>
		int DisplayHeight { get; }
				
		/// <summary>
		/// Informs the manager drawing is beginning.
		/// </summary>
		void BeginDraw();

		/// <summary>
		/// Informs the manager drawing is ending.
		/// </summary>
		void EndDraw();

		/// <summary>
		/// Clears the back buffer by filling it with the given color.
		/// </summary>
		/// <param name="color"></param>
		void Clear(Color color);

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		void Present();

		/// <summary>
		/// Creates a new texture.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		Texture CreateTexture(int width, int height);

		/// <summary>
		/// Loads a texture.
		/// </summary>
		/// <param name="fileName">The file name to load.</param>
		/// <param name="colorKey">A color which should be used for transparency.</param>
		/// <returns></returns>
		Texture LoadTexture(string fileName, Color? colorKey);

		/// <summary>
		/// Creates a TextureFont.
		/// </summary>
		/// <param name="fontName"></param>
		/// <param name="fontSize"></param>
		/// <param name="antialias"></param>
		/// <returns></returns>
		TextureFont CreateTextureFont(string fontName, int fontSize, bool antialias);

		/// <summary>
		/// Loads a TextureFont from an XML file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		TextureFont LoadTextureFont(string fileName, Color? colorKey);

		/// <summary>
		/// Creates a new RenderTarget.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		RenderTarget CreateRenderTarget(int width, int height);
	}
}
