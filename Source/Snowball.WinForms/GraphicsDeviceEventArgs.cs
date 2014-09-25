using System;
using Snowball.Graphics;

namespace Snowball.WinForms
{
	public class GraphicsDeviceEventArgs : EventArgs
	{
		public GraphicsDevice GraphicsDevice
		{
			get;
			private set;
		}

		public GraphicsDeviceEventArgs(GraphicsDevice graphicsDevice)
			: base()
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			this.GraphicsDevice = graphicsDevice;
		}
	}
}
