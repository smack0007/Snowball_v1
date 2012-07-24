using System;
using System.ComponentModel;
using System.Windows.Forms;
using Snowball;

namespace CustomGameWindowSample
{
	public class CustomGameWindow : IGameWindow
	{
		CustomGameWindowForm form;
		Timer timer;

		public IntPtr Handle
		{
			get
			{
				// Return the handle to the picture box in the form.
				return this.form.GameBox.Handle;
			}
		}

		public string Title
		{
			get { return this.form.Text; }
			set { this.form.Text = value; }
		}

		public System.Drawing.Icon Icon
		{
			get { return this.form.Icon; }
			set { this.form.Icon = value; }
		}

		public int Width
		{
			get { return this.form.Width; }
		}

		public int Height
		{
			get { return this.form.Height; }
		}

		public int DisplayWidth
		{
			get { return this.form.GameBox.Width; }
			set { /* Just ignore it. */ }
		}

		public int DisplayHeight
		{
			get { return this.form.GameBox.Height; }
			set { /* Just ignore it. */ }
		}

		public event EventHandler Tick;

		public event EventHandler Exiting;

		public event EventHandler Resume;

		public event EventHandler Pause;

		public event EventHandler<CancelEventArgs> Close;

		public event EventHandler<GameWindowKeyPressEventArgs> KeyPress;

		public event EventHandler DisplaySizeChanged;

		public CustomGameWindow()
		{
			this.form = new CustomGameWindowForm();
			this.form.MenuFileExit.Click += (s, e) => { this.Exit(); };

			this.timer = new Timer();
			this.timer.Interval = 10;
			this.timer.Tick += this.Timer_Tick;
		}

		public void Run()
		{
			this.timer.Start();
			Application.Run(this.form);
		}

		public void Exit()
		{
			this.timer.Stop();
			Application.Exit();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (this.Tick != null)
				this.Tick(this, EventArgs.Empty);
		}
				
		public void ShowMessageDialog(MessageDialogType type, string message, string caption)
		{
		}

		public bool ShowOpenFileDialog(string fileTypeName, string[] fileTypeFilters, out string fileName)
		{
			fileName = string.Empty;
			return false;
		}
	}
}
