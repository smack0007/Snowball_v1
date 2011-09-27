using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which can be drawn onto.
	/// </summary>
	public class RenderTarget : Texture
	{
		GraphicsManager graphicsManager;
		internal SlimDX.Direct3D9.RenderToSurface renderToSurface;

		internal RenderTarget(GraphicsManager graphicsManager, int width, int height)
			: base()
		{
			if(graphicsManager == null)
			{
				throw new ArgumentNullException("graphicsManager");
			}

			this.graphicsManager = graphicsManager;
			this.Width = width;
			this.Height = height;

			this.CreateResources();

			graphicsManager.DeviceLost += this.GraphicsManager_DeviceLost;
			graphicsManager.DeviceReset += this.GraphicsManager_DeviceReset;
		}

		private void CreateResources()
		{
			this.renderToSurface = new SlimDX.Direct3D9.RenderToSurface(this.graphicsManager.device, this.Width, this.Height, SlimDX.Direct3D9.Format.A8R8G8B8);

			this.texture = new SlimDX.Direct3D9.Texture(graphicsManager.device, this.Width, this.Height, 0, SlimDX.Direct3D9.Usage.RenderTarget,
														SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Default);
		}
				
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if(disposing)
			{
				this.DestroyResources();
			}
		}

		private void DestroyResources()
		{
			if(this.renderToSurface != null)
			{
				this.renderToSurface.Dispose();
				this.renderToSurface = null;
			}

			if(this.texture != null)
			{
				this.texture.Dispose();
				this.texture = null;
			}
		}

		private void GraphicsManager_DeviceLost(object sender, EventArgs e)
		{
			this.DestroyResources();
		}

		private void GraphicsManager_DeviceReset(object sender, EventArgs e)
		{
			this.CreateResources();
		}
	}
}
