using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Snowball.Input;
using Snowball.Win32;

namespace Snowball
{
	internal sealed class GameWindow : Form, IGameWindow
	{
		bool isRunning;

		int keyCode;
		event EventHandler<GameWindowKeyPressEventArgs> gameWindowKeyPressEvent;
		GameWindowKeyPressEventArgs gameWindowKeyPressEventArgs;

		CancelEventArgs closeEventArgs;				

		/// <summary>
		/// Gets or sets the text of the window.
		/// </summary>
		public string Title
		{
			get { return this.Text; }
			set { this.Text = value; }
		}
		
		/// <summary>
		/// The width of the game display area.
		/// </summary>
		public int DisplayWidth
		{
			get { return this.ClientSize.Width; }
			set { this.ClientSize = new Size(value, this.ClientSize.Height); }
		}

		/// <summary>
		/// Tthe height of the game display area.
		/// </summary>
		public int DisplayHeight
		{
			get { return this.ClientSize.Height; }
			set { this.ClientSize = new Size(this.ClientSize.Width, value); }
		}

		/// <summary>
		/// Triggered when idle time is available.
		/// </summary>
		public event EventHandler Tick;

		/// <summary>
		/// Triggered when the game window is minimized.
		/// </summary>
		public event EventHandler Resume;

		/// <summary>
		/// Triggered when the game window is restored.
		/// </summary>
		public event EventHandler Pause;

		/// <summary>
		/// Triggered when the game window is being closed.
		/// </summary>
		public event EventHandler<CancelEventArgs> Close;

		/// <summary>
		/// Triggered just before a shutdown occurs.
		/// </summary>
		public event EventHandler Exiting;

		/// <summary>
		/// Triggered when a key is pressed.
		/// </summary>
		event EventHandler<GameWindowKeyPressEventArgs> IGameWindow.KeyPress
		{
			add { this.gameWindowKeyPressEvent += value; }
			remove { this.gameWindowKeyPressEvent -= value; }
		}

		/// <summary>
		/// Triggered when the size of the client area of the window changes.
		/// </summary>
		public event EventHandler DisplaySizeChanged;

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
			: base()
		{			
			this.FormBorderStyle = FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.ClientSize = new Size(800, 600);
			this.KeyPreview = true;

			this.Icon = Snowball.Properties.Resources.Icon;
			
			this.gameWindowKeyPressEventArgs = new GameWindowKeyPressEventArgs();

			this.closeEventArgs = new CancelEventArgs();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyValue == 18) // Disable alt key menu activation
				e.Handled = true;

			base.OnKeyDown(e);
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
 			 base.OnClientSizeChanged(e);

			 if (this.DisplaySizeChanged != null)
				 this.DisplaySizeChanged(this, e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				this.closeEventArgs.Cancel = false;

				if (this.Close != null)
					this.Close(this, this.closeEventArgs);

				e.Cancel = this.closeEventArgs.Cancel;
			}

			base.OnFormClosing(e);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			this.closeEventArgs.Cancel = false;

			if (this.Close != null)
				this.Close(this, this.closeEventArgs);

			base.OnFormClosed(e);
			this.Exit();
		}

		private void TriggerPause()
		{
			if (this.Pause != null)
				this.Pause(this, EventArgs.Empty);
		}

		private void TriggerResume()
		{
			if (this.Resume != null)
				this.Resume(this, EventArgs.Empty);
		}

		/// <summary>
		/// Begins the message pump.
		/// </summary>
		public void Run()
		{
			this.isRunning = true;

			this.Show();

			Win32Message message;

			while (this.isRunning)
			{
				if (Win32Methods.PeekMessage(out message, IntPtr.Zero, 0, 0, Win32Constants.PM_REMOVE))
				{
					switch (message.msg)
					{
						case Win32Constants.WM_KEYDOWN:
							this.keyCode = (int)message.wParam;
							break;

						case Win32Constants.WM_CHAR:
						case Win32Constants.WM_UNICHAR:
							this.gameWindowKeyPressEventArgs.KeyCode = this.keyCode;
							this.gameWindowKeyPressEventArgs.KeyChar = (char)message.wParam;

							if (this.gameWindowKeyPressEvent != null)
								this.gameWindowKeyPressEvent(this, this.gameWindowKeyPressEventArgs);

							break;

						case Win32Constants.WM_ENTERSIZEMOVE:
							this.TriggerPause();
							break;

						case Win32Constants.WM_EXITSIZEMOVE:
							this.TriggerResume();
							break;

						case Win32Constants.WM_SYSCOMMAND:
							if (message.wParam == (IntPtr)Win32Constants.SC_MINIMIZE)
							{
								this.TriggerPause();
							}
							else if (message.wParam == (IntPtr)Win32Constants.SC_RESTORE)
							{
								this.TriggerResume();
							}
							break;

						case Win32Constants.WM_SYSKEYDOWN:
						case Win32Constants.WM_SYSKEYUP:

							break;
					}

					Win32Methods.TranslateMessage(ref message);
					Win32Methods.DispatchMessage(ref message);
				}
				else
				{
					if (this.Tick != null)
						this.Tick(this, EventArgs.Empty);
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

		private void TriggerDialogOpen()
		{
			if (this.DialogOpen != null)
				this.DialogOpen(this, EventArgs.Empty);
		}

		private void TriggerDialogClose()
		{
			if (this.DialogClose != null)
				this.DialogClose(this, EventArgs.Empty);
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

			this.TriggerDialogOpen();

			MessageBox.Show(this, message, caption, MessageBoxButtons.OK, icon);

			this.TriggerDialogClose();
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

			this.TriggerDialogOpen();

			DialogResult result = dialog.ShowDialog();
			fileName = dialog.FileName;

			this.TriggerDialogClose();

			return result == DialogResult.OK;
		}
	}
}
