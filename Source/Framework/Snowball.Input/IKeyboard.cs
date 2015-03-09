using System;

namespace Snowball.Input
{
	/// <summary>
	/// Interface for keyboard input devices.
	/// </summary>
	public interface IKeyboard
	{
		/// <summary>
		/// Returns true if the given key is currently down.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsKeyDown(Keys key);

		/// <summary>
		/// Returns true if the given key is currently down and last update was not.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsKeyPressed(Keys key);

		/// <summary>
		/// Returns true if the given key is currently up.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsKeyUp(Keys key);

		/// <summary>
		/// Returns true if the given key is currently up and last update was not.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsKeyReleased(Keys key);
	}
}
