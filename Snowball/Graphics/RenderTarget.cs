using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which can be drawn onto.
	/// </summary>
	public sealed class RenderTarget : GameResource
	{
		GraphicsDevice graphicsManager;

		internal SharpDX.Direct3D9.Texture InternalTexture;
		internal SharpDX.Direct3D9.RenderToSurface InternalRenderToSurface;

		/// <summary>
		/// The width of the render target.
		/// </summary>
		public int Width
		{
			get;
			private set;
		}

		/// <summary>
		/// The height of the render target.
		/// </summary>
		public int Height
		{
			get;
			private set;
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

			//this.InternalRenderToSurface = new SharpDX.Direct3D9.RenderToSurface(this.graphicsManager.InternalDevice, this.Width, this.Height, SharpDX.Direct3D9.Format.A8R8G8B8);

			this.InternalTexture = new SharpDX.Direct3D9.Texture(this.graphicsManager.InternalDevice, this.Width, this.Height, 0, SharpDX.Direct3D9.Usage.RenderTarget,
																SharpDX.Direct3D9.Format.A8R8G8B8, SharpDX.Direct3D9.Pool.Default);
		}
			
				
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
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
		}
	}
}
