using System;
using System.ComponentModel;
using System.Drawing;
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
		/// Gets or sets the icon displayed by the window.
		/// </summary>
		Icon Icon { get; set; }

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
		int DisplayWidth { get; set; }

		/// <summary>
		/// Gets or sets the height of the game display area of the window.
		/// </summary>
		int DisplayHeight { get; set; }

		/// <summary>
		/// The cursor displayed when the window has focus.
		/// </summary>
		GameWindowCursor Cursor { get; set; }
				
		/// <summary>
		/// Triggered when the host control has is idle.
		/// </summary>
		event EventHandler Tick;

		/// <summary>
		/// Triggered just before a shutdown occurs.
		/// </summary>
		event EventHandler Exiting;

		/// <summary>
		/// Triggered when the window signals the game should resume.
		/// </summary>
		event EventHandler Resume;

		/// <summary>
		/// Triggered when the window signals the game should pause.
		/// </summary>
		event EventHandler Pause;

		/// <summary>
		/// Triggered when the game window is being closed.
		/// </summary>
		event EventHandler<CancelEventArgs> Close;

		/// <summary>
		/// Triggered when a key is pressed.
		/// </summary>
		event EventHandler<GameWindowKeyPressEventArgs> KeyPress;

		/// <summary>
		/// Triggered when the size of the client area changes.
		/// </summary>
		event EventHandler DisplaySizeChanged;
				
		/// <summary>
		/// Tells the window to begin the message pump.
		/// </summary>
		void Run();
		
		/// <summary>
		/// Tells the window to end the message pump.
		/// </summary>
		void Exit();
	}
}
