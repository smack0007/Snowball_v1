using System;

namespace Snowball
{
	/// <summary>
	/// Contains values for the different kinds of GameWindow styles.
	/// </summary>
	public enum GameWindowStyle
	{
		/// <summary>
		/// The GameWindow will not be modified.
		/// </summary>
		None = 0,

		/// <summary>
		/// The GameWindow will be automatically sized.
		/// </summary>
		Sized,

		/// <summary>
		///  The GameWindow will be made fullscreen.
		/// </summary>
		Fullscreen
	}
}
