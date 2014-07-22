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
	public class GameWindow : IHostControl, IDisposable
	{
		Game game;
		Win32GameWindow window;
	
		/// <summary>
		/// Gets the window handle.
		/// </summary>
		public IntPtr Handle
		{
			get { return this.window.Handle; }
		}

		/// <summary>
		/// Gets or sets the title of the window.
		/// </summary>
		public string Title
		{
			get { return this.window.Title; }
			set { this.window.Title = value; }
		}

		/// <summary>
		/// Gets or sets the icon displayed by the window.
		/// </summary>
		public Icon Icon
		{
			get { return this.window.Icon; }
			set { this.window.Icon = value; }
		}

		/// <summary>
		/// Gets or sets the width of the window.
		/// </summary>
		public int DisplayWidth
		{
			get { return this.window.DisplayWidth; }
			set { this.window.DisplayWidth = value; }
		}

		/// <summary>
		/// Gets or sets the height of the window.
		/// </summary>
		public int DisplayHeight
		{
			get { return this.window.DisplayHeight; }
			set { this.window.DisplayHeight = value; }
		}
						
		/// <summary>
		/// Triggered when the game window is being closed.
		/// </summary>
		public event EventHandler<CancelEventArgs> Closing;

		/// <summary>
		/// Triggered when a key is pressed.
		/// </summary>
		public event EventHandler<GameWindowKeyPressEventArgs> KeyPress;

		internal GameWindow(Game game)
		{
			this.game = game;
			this.window = new Win32GameWindow();

			this.window.Tick += this.Window_Tick;
			this.window.Resume += this.Window_Resume;
			this.window.Pause += this.Window_Pause;
			this.window.Closing += this.Window_Closing;
			this.window.KeyPress += this.Window_KeyPress;
		}

		public void Dispose()
		{
			this.window.Tick -= this.Window_Tick;
			this.window.Resume -= this.Window_Resume;
			this.window.Pause -= this.Window_Pause;
			this.window.Closing -= this.Window_Closing;
			this.window.KeyPress -= this.Window_KeyPress;
		}
				
		public void Run()
		{
			this.window.Run();
		}

		public void Exit()
		{
			this.window.Exit();
		}

		private void Window_Tick(object sender, EventArgs e)
		{
			this.game.Tick();
		}

		private void Window_Resume(object sender, EventArgs e)
		{
			this.game.Resume();
		}

		private void Window_Pause(object sender, EventArgs e)
		{
			this.game.Pause();
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if (this.Closing != null)
				this.Closing(this, e);
		}

		private void Window_KeyPress(object sender, GameWindowKeyPressEventArgs e)
		{
			if (this.KeyPress != null)
				this.KeyPress(this, e);
		}
	}
}
