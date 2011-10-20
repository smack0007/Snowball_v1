﻿using System;
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
		/// Creates a new RenderTarget.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		RenderTarget CreateRenderTarget(int width, int height);
	}
}