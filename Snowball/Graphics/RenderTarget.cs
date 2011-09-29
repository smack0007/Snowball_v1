using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which can be drawn onto.
	/// </summary>
	public class RenderTarget : GameResource
	{
		GraphicsDevice graphicsManager;

		internal SlimDX.Direct3D9.Texture InternalTexture;
		internal SlimDX.Direct3D9.RenderToSurface InternalRenderToSurface;

		public int Width
		{
			get;
			protected set;
		}

		public int Height
		{
			get;
			protected set;
		}

		internal RenderTarget(GraphicsDevice graphicsManager, int width, int height)
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

			this.graphicsManager.DeviceLost += this.GraphicsManager_DeviceLost;
			this.graphicsManager.DeviceReset += this.GraphicsManager_DeviceReset;
		}

		private void CreateResources()
		{
			this.InternalRenderToSurface = new SlimDX.Direct3D9.RenderToSurface(this.graphicsManager.InternalDevice, this.Width, this.Height, SlimDX.Direct3D9.Format.A8R8G8B8);

			this.InternalTexture = new SlimDX.Direct3D9.Texture(this.graphicsManager.InternalDevice, this.Width, this.Height, 0, SlimDX.Direct3D9.Usage.RenderTarget,
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
			if(this.InternalRenderToSurface != null)
			{
				this.InternalRenderToSurface.Dispose();
				this.InternalRenderToSurface = null;
			}

			if(this.InternalTexture != null)
			{
				this.InternalTexture.Dispose();
				this.InternalTexture = null;
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
