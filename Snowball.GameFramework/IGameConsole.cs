using System;

namespace Snowball.GameFramework
{
	/// <summary>
	/// Interface for game consoles.
	/// </summary>
	public interface IGameConsole
	{
		/// <summary>
		/// Triggered whenever a command is entered.
		/// </summary>
		event EventHandler<GameConsoleInputEventArgs> InputReceived;

		/// <summary>
		/// Triggered whenever something is given to be displayed in the console.
		/// </summary>
		event EventHandler<GameConsoleOutputEventArgs> OutputReceived;

		/// <summary>
		/// Writes output to the console.
		/// </summary>
		/// <param name="text"></param>
		void WriteLine(string text);

		/// <summary>
		/// Writes output to the console in the specified color.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="color"></param>
		void WriteLine(string text, Color color);
	}
}
