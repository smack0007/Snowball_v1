using System;
using Snowball.Graphics;

namespace Snowball.UI
{
	public interface IUIContentLoader
	{
		/// <summary>
		/// Loads the font used throughout the user interface.
		/// </summary>
		/// <returns></returns>
		ITextureFont LoadFont();

		/// <summary>
		/// Loads the texture used by the Button class.
		/// </summary>
		/// <returns></returns>
		Texture LoadButtonTexture();
	}
}
