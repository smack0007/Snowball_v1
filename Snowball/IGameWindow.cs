using System;
using Snowball.Input;

namespace Snowball
{
	/// <summary>
	/// Interface for the window containing the game.
	/// </summary>
	public interface IGameWindow
	{
		/// <summary>
		/// Gets a handle to the window.
		/// </summary>
		IntPtr Handle { get; }

		/// <summary>
		/// Gets or sets the title of the window.
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// Gets the width of the window.
		/// </summary>
		int Width { get; }

		/// <summary>
		/// Gets the height of the window.
		/// </summary>
		int Height { get; }

		/// <summary>
		/// Gets or sets the width of the game display area of the window.
		/// </summary>
		int ClientWidth { get; set; }

		/// <summary>
		/// Gets or sets the height of the game display area of the window.
		/// </summary>
		int ClientHeight { get; set; }
				
		/// <summary>
		/// Triggered when the host control has is idle.
		/// </summary>
		event EventHandler Idle;

		/// <summary>
		/// Triggered just before a shutdown occurs.
		/// </summary>
		event EventHandler Exiting;

		/// <summary>
		/// Triggered when the window is minimized.
		/// </summary>
		event EventHandler Activate;

		/// <summary>
		/// Triggered when the window is restored.
		/// </summary>
		event EventHandler Deactivate;

		/// <summary>
		/// Triggered when a key is pressed.
		/// </summary>
		event EventHandler<KeyCodeEventArgs> KeyPress;

		/// <summary>
		/// Triggered when the size of the client area changes.
		/// </summary>
		event EventHandler ClientSizeChanged;

		/// <summary>
		/// Tells the host to begin the message pump.
		/// </summary>
		void Run();
	}
}
