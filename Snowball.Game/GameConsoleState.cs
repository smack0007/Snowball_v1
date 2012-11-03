using System;

namespace Snowball
{
	/// <summary>
	/// The different possible states of the console.
	/// </summary>
	public enum GameConsoleState
	{
		/// <summary>
		/// The console is hidden.
		/// </summary>
		Hidden = 0,

		/// <summary>
		/// The console is sliding down.
		/// </summary>
		SlideDown,

		/// <summary>
		/// The console is completely visible.
		/// </summary>
		Visible,

		/// <summary>
		/// The console is sliding up.
		/// </summary>
		SlideUp
	}
}
