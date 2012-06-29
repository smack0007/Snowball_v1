using System;

namespace Snowball
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
