using System;
using System.Drawing;
using System.Windows.Forms;
using Snowball.Win32;

namespace Snowball
{
	/// <summary>
	/// Main form of the host.
	/// </summary>
	public class GameForm : Form
	{
		/// <summary>
		/// Triggered when the form is minimized.
		/// </summary>
		public event EventHandler Minimize;

		/// <summary>
		/// Triggered when the form is restored.
		/// </summary>
		public event EventHandler Restore;

		/// <summary>
		/// Triggered when the form begins moving.
		/// </summary>
		public event EventHandler MoveBegin;

		/// <summary>
		/// Triggered when the form ends moving.
		/// </summary>
		public event EventHandler MoveEnd;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="host"></param>
		public GameForm()
			: base()
		{
			this.FormBorderStyle = FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.ClientSize = new Size(800, 600);
			this.KeyPreview = true;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyValue == 18) // Disable alt key menu activation
				e.Handled = true;

			base.OnKeyDown(e);
		}
	
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch(m.Msg)
			{
				case Win32Constants.WM_ENTERSIZEMOVE:
					if (this.MoveBegin != null)
						this.MoveBegin(this, EventArgs.Empty);
					break;

				case Win32Constants.WM_EXITSIZEMOVE:
					if (this.MoveEnd != null)
						this.MoveEnd(this, EventArgs.Empty);
					break;

				case Win32Constants.WM_SYSCOMMAND:
					if (m.WParam == (IntPtr)Win32Constants.SC_MINIMIZE)
					{
						if (this.Minimize != null)
							this.Minimize(this, EventArgs.Empty);
					}
					else if (m.WParam == (IntPtr)Win32Constants.SC_RESTORE)
					{
						if (this.Restore != null)
							this.Restore(this, EventArgs.Empty);
					}
					break;

				case Win32Constants.WM_SYSKEYDOWN:
				case Win32Constants.WM_SYSKEYUP:
					
					break;
			}
		}
	}
}
