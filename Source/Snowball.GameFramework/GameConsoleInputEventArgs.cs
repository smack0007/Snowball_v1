using System;

namespace Snowball.GameFramework
{
	/// <summary>
	/// Event args associated with entering input in the GameConsole class.
	/// </summary>
	public class GameConsoleInputEventArgs : EventArgs
	{
		/// <summary>
		/// The text entered into the GameConsole.
		/// </summary>
		public string Text
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameConsoleInputEventArgs()
			: base()
		{
		}
	}
}
