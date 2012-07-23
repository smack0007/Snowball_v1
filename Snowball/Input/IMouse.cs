using System;

namespace Snowball.Input
{
	/// <summary>
	/// Interface for Mouse input devices.
	/// </summary>
	public interface IMouse
	{
		/// <summary>
		/// Returns true if the mouse is within the game window client area.
		/// </summary>
		bool IsWithinDisplayArea { get; }

		/// <summary>
		/// The position of the cursor.
		/// </summary>
		Point Position { get; }

		/// <summary>
		/// The X position of the cursor.
		/// </summary>
		int X { get; }

		/// <summary>
		/// The Y position of the cursor.
		/// </summary>
		int Y { get; }

		/// <summary>
		/// Returns true if the left mouse button is currently down.
		/// </summary>
		bool LeftButton { get; }

		/// <summary>
		/// Returns true if the right mouse button is currently down.
		/// </summary>
		bool RightButton { get; }

		/// <summary>
		/// Returns true if the middle mouse button is currently down.
		/// </summary>
		bool MiddleButton { get; }

		/// <summary>
		/// Returns true if XButton1 mouse button is currently down.
		/// </summary>
		bool XButton1 { get; }

		/// <summary>
		/// Returns true if XButton2 mouse button is currently down.
		/// </summary>
		bool XButton2 { get; }

		/// <summary>
		/// Returns true if the given mouse button is currently down.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonDown(MouseButtons button);

		/// <summary>
		/// Returns true if the given mouse button is currently down and was not on the last update. Method is same as IsButtonClicked().
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonPressed(MouseButtons button);

		/// <summary>
		/// Returns true if the given mouse button is currently down and was not on the last update. Method is same as IsButtonPressed().
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonClicked(MouseButtons button);

		/// <summary>
		/// Returns true if the given mouse button is clicked and was clicked twice within the time span specified by DoubleClickRate.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonDoubleClicked(MouseButtons button);

		/// <summary>
		/// Resets double click tracking for the mouse.
		/// </summary>
		void ResetDoubleClick();

		/// <summary>
		/// Returns true if the given mouse button is currently up.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonUp(MouseButtons button);

		/// <summary>
		/// Returns true if the given mouse button is currently up and was not up on the last update.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonReleased(MouseButtons button);
	}
}
