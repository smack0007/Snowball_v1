using System;
using System.Drawing;
using System.Windows.Forms;
using Snowball;
using Snowball.Graphics;

namespace Snowball.WinForms
{
	public class GraphicsDeviceControl : Control, IHostControl
	{
		static GraphicsDeviceEventArgs graphicsDeviceEventArgs;

		public static GraphicsDevice GraphicsDevice
		{
			get;
			private set;
		}

		public int DisplayWidth
		{
			get { return this.ClientSize.Width; }
		}

		public int DisplayHeight
		{
			get { return this.ClientSize.Height; }
		}		

		public event EventHandler<GraphicsDeviceEventArgs> Initialize;

		public event EventHandler<GraphicsDeviceEventArgs> Draw;

		public GraphicsDeviceControl()
			: base()
		{
			this.BackColor = System.Drawing.Color.CornflowerBlue;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		private void DoDraw()
		{
			if (GraphicsDevice.BeginDraw())
			{
				GraphicsDevice.Clear(new Snowball.Color(this.BackColor.R, this.BackColor.G, this.BackColor.B, this.BackColor.A));

				this.OnDraw(graphicsDeviceEventArgs);

				GraphicsDevice.EndDraw();

				Snowball.Rectangle rect = new Snowball.Rectangle(0, 0, this.Width, this.Height);
				GraphicsDevice.Present(rect, rect, this.Handle);
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (GraphicsDevice == null)
			{
				GraphicsDevice = new GraphicsDevice(this, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, false);
				graphicsDeviceEventArgs = new GraphicsDeviceEventArgs(GraphicsDevice);
			}

			this.OnInitialize(graphicsDeviceEventArgs);

			this.DoDraw();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!this.DesignMode && GraphicsDevice != null)
			{
				this.DoDraw();
			}
		}

		protected virtual void OnInitialize(GraphicsDeviceEventArgs e)
		{
			if (this.Initialize != null)
				this.Initialize(this, e);
		}

		protected virtual void OnDraw(GraphicsDeviceEventArgs e)
		{
			if (this.Draw != null)
				this.Draw(this, e);
		}
	}
}
