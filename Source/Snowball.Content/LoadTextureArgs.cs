using System;

namespace Snowball.Content
{
	/// <summary>
	/// Args for loading a Texture.
	/// </summary>
	public class LoadTextureArgs : LoadContentArgs
	{
		/// <summary>
		/// The color to use as transparency when loading the Texture.
		/// </summary>
		public Color? ColorKey
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public LoadTextureArgs()
			: base()
		{
		}
	}
}
