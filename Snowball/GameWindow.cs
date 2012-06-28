using System;
using System.Drawing;
using System.Windows.Forms;
using Snowball.Input;
using Snowball.Win32;

namespace Snowball
{
	/// <summary>
	/// Implementation of the game window.
	/// </summary>
	public class GameWindow : IGameWindow
	{
		/// <summary>
		/// Handle to the last GameWindow which was created.
		/// </summary>
		public static GameWindow Current = null;

		bool isRunning;

		GameWindowKeyPressEventArgs keyPressEventArgs;

		System.Drawing.Point oldFormLocation;

		/// <summary>
		/// The Form which hosts the Game.
		/// </summary>
		protected GameForm Form
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Handle to the game host control.
		/// </summary>
		public IntPtr Handle
		{
			get { return this.Form.Handle; }
		}

		/// <summary>
		/// Gets or sets the text of the window.
		/// </summary>
		public string Title
		{
			get { return this.Form.Text; }
			set { this.Form.Text = value; }
		}

		/// <summary>
		/// Gets or sets the icon of the window.
		/// </summary>
		public Icon Icon
		{
			get { return this.Form.Icon; }
			set { this.Form.Icon = value; }
		}

		/// <summary>
		/// Gets the width of the host.
		/// </summary>
		public int Width
		{
			get { return this.Form.Width; }
		}

		/// <summary>
		/// Gets the height of the game host.
		/// </summary>
		public int Height
		{
			get { return this.Form.Height; }
		}

		/// <summary>
		/// The width of the game host client area.
		/// </summary>
		public virtual int ClientWidth
		{
			get { return this.Form.ClientSize.Width; }
			set { this.Form.ClientSize = new Size(value, this.Form.ClientSize.Height); }
		}

		/// <summary>
		/// Tthe height of the game host client area.
		/// </summary>
		public virtual int ClientHeight
		{
			get { return this.Form.ClientSize.Height; }
			set { this.Form.ClientSize = new Size(this.Form.ClientSize.Width, value); }
		}
				
		/// <summary>
		/// Triggered when idle time is available.
		/// </summary>
		public event EventHandler Idle;
				
		/// <summary>
		/// Triggered when the game window is minimized.
		/// </summary>
		public event EventHandler Activate;

		/// <summary>
		/// Triggered when the game window is restored.
		/// </summary>
		public event EventHandler Deactivate;

		/// <summary>
		/// Triggered just before a shutdown occurs.
		/// </summary>
		public event EventHandler Exiting;

		/// <summary>
		/// Triggered when a key is pressed.
		/// </summary>
		public event EventHandler<GameWindowKeyPressEventArgs> KeyPress;

		/// <summary>
		/// Triggered when the size of the client area of the window changes.
		/// </summary>
		public event EventHandler ClientSizeChanged;

		/// <summary>
		/// Triggered when before the window begins to show a dialog.
		/// </summary>
		public event EventHandler DialogOpen;

		/// <summary>
		/// Triggered after the window has shown a dialog.
		/// </summary>
		public event EventHandler DialogClose;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameWindow()
		{
			this.Form = new GameForm();
			this.Form.Minimize += this.Form_Minimize;
			this.Form.Restore += this.Form_Restore;
			this.Form.MoveBegin += this.Form_MoveBegin;
			this.Form.MoveEnd += this.Form_MoveEnd;
			this.Form.FormClosed += this.Form_FormClosed;
			this.Form.ClientSizeChanged += this.Form_ClientSizeChanged;

			this.keyPressEventArgs = new GameWindowKeyPressEventArgs();

			GameWindow.Current = this;
		}

		/// <summary>
		/// Begins the message pump.
		/// </summary>
		public void Run()
		{
			this.isRunning = true;

			this.Form.Show();

			Win32Message message;
			
			while(this.isRunning)
			{
				if (Win32Methods.PeekMessage(out message, IntPtr.Zero, 0, 0, Win32Constants.PM_REMOVE))
				{
					switch(message.msg)
					{
						case Win32Constants.WM_CHAR:
						case Win32Constants.WM_UNICHAR:
							this.keyPressEventArgs.KeyChar = (char)message.wParam;
							
							if (this.KeyPress != null)
								this.KeyPress(this, this.keyPressEventArgs);
							
							break;
					}

					Win32Methods.TranslateMessage(ref message);
					Win32Methods.DispatchMessage(ref message);
				}
				else
				{
					this.OnIdle(EventArgs.Empty);
				}
			}
						
			if (this.Exiting != null)
				this.Exiting(this, EventArgs.Empty);
		}

		/// <summary>
		/// Forces the game to shutdown.
		/// </summary>
		public void Exit()
		{
			this.isRunning = false;
		}

		/// <summary>
		/// Called when idle time is available.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnIdle(EventArgs e)
		{
			if (this.Idle != null)
				this.Idle(this, e);
		}

		/// <summary>
		/// Called when the form is minimized.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_Minimize(object sender, EventArgs e)
		{
			if (this.Deactivate != null)
				this.Deactivate(this, e);
		}

		/// <summary>
		/// Called when the form is restored.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_Restore(object sender, EventArgs e)
		{
			if (this.Activate != null)
				this.Activate(this, e);
		}

		/// <summary>
		/// Called when the form begins to move.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_MoveBegin(object sender, EventArgs e)
		{
			if (this.Deactivate != null)
				this.Deactivate(this, e);
		}

		/// <summary>
		/// Called when the form ends moving.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_MoveEnd(object sender, EventArgs e)
		{
			if (this.Activate != null)
				this.Activate(this, e);
		}

		/// <summary>
		/// Called when the form is closed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_FormClosed(object sender, EventArgs e)
		{
			this.Exit();
		}

		/// <summary>
		/// Called when the ClientSize property of the form changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_ClientSizeChanged(object sender, EventArgs e)
		{
			if (this.ClientSizeChanged != null)
				this.ClientSizeChanged(this, e);
		}

		/// <summary>
		/// Tells the window that fullscreen is about to be toggled.
		/// </summary>
		/// <param name="isFullScreen">Whether or not the game is currently running fullscreen.</param>
		public void BeforeToggleFullscreen(bool isFullscreen)
		{
			if (isFullscreen)
			{
				this.oldFormLocation = this.Form.Location;
			}
		}

		/// <summary>
		/// Tells the window that fullscreen has been toggled.
		/// </summary>
		/// <param name="isFullScreen">Whether or not the game is currently running fullscreen.</param>
		public void AfterToggleFullscreen(bool isFullscreen)
		{
			if (!isFullscreen)
			{
				this.Form.FormBorderStyle = FormBorderStyle.Fixed3D;
				this.Form.Location = this.oldFormLocation;
			}
		}

		/// <summary>
		/// Displays a message dialog to the user.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>
		/// <param name="caption"></param>
		public void ShowMessageDialog(MessageDialogType type, string message, string caption)
		{
			MessageBoxIcon icon = MessageBoxIcon.Information;

			switch (type)
			{
				case MessageDialogType.Information:
					icon = MessageBoxIcon.Information;
					break;

				case MessageDialogType.Error:
					icon = MessageBoxIcon.Error;
					break;
			}

			if (this.DialogOpen != null)
				this.DialogOpen(this, EventArgs.Empty);

			MessageBox.Show(this.Form, message, caption, MessageBoxButtons.OK, icon);

			if (this.DialogClose != null)
				this.DialogClose(this, EventArgs.Empty);
		}

		/// <summary>
		/// Displays an open file dialog. Returns true if the user selects a file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public bool ShowOpenFileDialog(string fileTypeName, string[] fileTypeFilters, out string fileName)
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Filter = fileTypeName + "|" + string.Join(";", fileTypeFilters)
			};

			if (this.DialogOpen != null)
				this.DialogOpen(this, EventArgs.Empty);

			DialogResult result = dialog.ShowDialog();
			fileName = dialog.FileName;

			if (this.DialogClose != null)
				this.DialogClose(this, EventArgs.Empty);

			return result == DialogResult.OK;
		}
	}
}
