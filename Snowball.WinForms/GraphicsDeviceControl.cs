using System;
using System.Drawing;
using System.Windows.Forms;
using Snowball;
using Snowball.Graphics;

namespace Snowball.WinForms
{
	public class GraphicsDeviceControl : Control
	{
		static GraphicsDeviceEventArgs graphicsDeviceEventArgs;

		public static GraphicsDevice GraphicsDevice
		{
			get;
			private set;
		}

		public event EventHandler<GraphicsDeviceEventArgs> Initialize;

		public event EventHandler<GraphicsDeviceEventArgs> Draw;

		static GraphicsDeviceControl()
		{
			GraphicsDevice = new GraphicsDevice();
		}

		public GraphicsDeviceControl()
			: base()
		{
			this.BackColor = System.Drawing.Color.CornflowerBlue;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (!GraphicsDevice.IsDeviceCreated)
			{
				GraphicsDevice.CreateDevice(this.Handle, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
				graphicsDeviceEventArgs = new GraphicsDeviceEventArgs(GraphicsDevice);
			}

			if (this.Initialize != null)
			{
				this.Initialize(this, graphicsDeviceEventArgs);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!this.DesignMode && GraphicsDevice != null && GraphicsDevice.IsDeviceCreated)
			{
				GraphicsDevice.Clear(new Snowball.Color(this.BackColor.R, this.BackColor.G, this.BackColor.B, this.BackColor.A));
				GraphicsDevice.BeginDraw();

				OnDraw(graphicsDeviceEventArgs);

				GraphicsDevice.EndDraw();

				Snowball.Rectangle rect = new Snowball.Rectangle(0, 0, this.Width, this.Height);
				GraphicsDevice.Present(rect, rect, this.Handle);
			}
		}

		protected virtual void OnDraw(GraphicsDeviceEventArgs e)
		{
			if (this.Draw != null)
				this.Draw(this, e);
		}
	}
}
