using System;

namespace Snowball
{
	/// <summary>
	/// Event args associated with entering commands into the GameConsole.
	/// </summary>
	public class GameConsoleCommandEventArgs : EventArgs
	{
		/// <summary>
		/// The command entered into the GameConsole.
		/// </summary>
		public string Command
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameConsoleCommandEventArgs()
			: base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="command"></param>
		public GameConsoleCommandEventArgs(string command)
			: base()
		{
			this.Command = command;
		}
	}
}
