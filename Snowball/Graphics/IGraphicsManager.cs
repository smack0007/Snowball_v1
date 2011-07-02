using System;

namespace Snowball.Graphics
{
	public interface IGraphicsManager
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
		/// Creates the graphics device.
		/// </summary>
		/// <param name="window">The game window.</param>
		void CreateDevice(IGameWindow window);

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
	}
}
