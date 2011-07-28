using System;

namespace Snowball
{
	public interface IGameConsole
	{
		/// <summary>
		/// Triggered whenever a command is entered.
		/// </summary>
		event EventHandler<GameConsoleCommandEventArgs> CommandEntered;

		void WriteLine(string text);

		void WriteLine(string text, Color color);
	}
}
