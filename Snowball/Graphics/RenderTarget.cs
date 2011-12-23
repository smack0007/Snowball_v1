using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which can be drawn onto.
	/// </summary>
	public sealed class RenderTarget : GameResource
	{
		GraphicsDevice graphicsManager;

		internal SlimDX.Direct3D9.Texture InternalTexture;
		internal SlimDX.Direct3D9.RenderToSurface InternalRenderToSurface;

		/// <summary>
		/// The width of the render target.
		/// </summary>
		public int Width
		{
			get;
			protected set;
		}

		/// <summary>
		/// The height of the render target.
		/// </summary>
		public int Height
		{
			get;
			protected set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public RenderTarget(GraphicsDevice graphicsDevice, int width, int height)
			: base()
		{
			if (graphicsDevice == null)
			{
				throw new ArgumentNullException("graphicsDevice");
			}

			this.graphicsManager = graphicsDevice;
			this.Width = width;
			this.Height = height;

			this.CreateResources();

			this.graphicsManager.DeviceLost += this.GraphicsDevice_DeviceLost;
			this.graphicsManager.DeviceReset += this.GraphicsDevice_DeviceReset;
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

			if (disposing)
			{
				this.DestroyResources();

				if (this.graphicsManager != null)
				{
					this.graphicsManager.DeviceLost -= this.GraphicsDevice_DeviceLost;
					this.graphicsManager.DeviceReset -= this.GraphicsDevice_DeviceReset;
					this.graphicsManager = null;
				}
			}
		}

		private void DestroyResources()
		{
			if (this.InternalRenderToSurface != null)
			{
				this.InternalRenderToSurface.Dispose();
				this.InternalRenderToSurface = null;
			}

			if (this.InternalTexture != null)
			{
				this.InternalTexture.Dispose();
				this.InternalTexture = null;
			}
		}

		private void GraphicsDevice_DeviceLost(object sender, EventArgs e)
		{
			this.DestroyResources();
		}

		private void GraphicsDevice_DeviceReset(object sender, EventArgs e)
		{
			this.CreateResources();
		}
	}
}
