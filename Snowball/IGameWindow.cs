﻿using System;
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
		event EventHandler<GameWindowKeyPressEventArgs> KeyPress;

		/// <summary>
		/// Triggered when the size of the client area changes.
		/// </summary>
		event EventHandler ClientSizeChanged;

		/// <summary>
		/// Triggered when before the window begins to show a dialog.
		/// </summary>
		event EventHandler DialogOpen;

		/// <summary>
		/// Triggered after the window has shown a dialog.
		/// </summary>
		event EventHandler DialogClose;

		/// <summary>
		/// Tells the window to begin the message pump.
		/// </summary>
		void Run();
		
		/// <summary>
		/// Tells the window to end the message pump.
		/// </summary>
		void Exit();

		/// <summary>
		/// Tells the window that fullscreen is about to be toggled.
		/// </summary>
		/// <param name="isFullScreen">Whether or not the game is currently running fullscreen.</param>
		void BeforeToggleFullscreen(bool isFullscreen);

		/// <summary>
		/// Tells the window that fullscreen has been toggled.
		/// </summary>
		/// <param name="isFullScreen">Whether or not the game is currently running fullscreen.</param>
		void AfterToggleFullscreen(bool isFullscreen);

		/// <summary>
		/// Displays a message dialog to the user.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>
		/// <param name="caption"></param>
		void ShowMessageDialog(MessageDialogType type, string message, string caption);
	}
}
