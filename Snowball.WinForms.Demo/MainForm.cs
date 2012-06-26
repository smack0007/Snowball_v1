using System;
using System.Windows.Forms;
using Snowball;
using Snowball.Graphics;
using Snowball.WinForms;

namespace Snowball.WinForms.Demo
{
	public partial class MainForm : Form
	{
		GraphicsBatch graphics;

		public MainForm()
		{
			InitializeComponent();

			this.graphicsDeviceControl.Initialize += this.GraphicsDeviceControl_Initialize;
			this.graphicsDeviceControl.Draw += this.GraphicsDeviceControl_Draw;
		}

		private void GraphicsDeviceControl_Initialize(object sender, GraphicsDeviceEventArgs e)
		{
			this.graphics = new GraphicsBatch(e.GraphicsDevice);
		}

		private void GraphicsDeviceControl_Draw(object sender, GraphicsDeviceEventArgs e)
		{
			this.graphics.Begin();
			this.graphics.DrawFilledRectangle(new Rectangle(10, 10, 100, 100), Color.Red);
			this.graphics.End();
		}
	}
}
