using Snowball.Graphics;
using Snowball.Input;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Snowball.GameFramework
{
	/// <summary>
	/// Interface for the window containing the game.
	/// </summary>
	public interface IGameWindow : IHostControl
	{		
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
	}
}
