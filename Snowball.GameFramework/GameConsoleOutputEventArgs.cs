using System;

namespace Snowball.GameFramework
{
	/// <summary>
	/// Event args associated with displaying output in the GameConsole class.
	/// </summary>
	public class GameConsoleOutputEventArgs : EventArgs
	{
		/// <summary>
		/// The text to be displayed in the GameConsole.
		/// </summary>
		public string Text
		{
			get;
			set;
		}

		/// <summary>
		/// The color of the text to be displayed in the GameConsole.
		/// </summary>
		public Color Color
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameConsoleOutputEventArgs()
			: base()
		{
		}
	}
}
