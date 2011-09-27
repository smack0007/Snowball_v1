using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which can be drawn onto.
	/// </summary>
	public class RenderTarget : Texture
	{
		internal SlimDX.Direct3D9.RenderToSurface renderToSurface;

		internal RenderTarget(GraphicsManager graphicsManager, int width, int height)
			: base()
		{
			if(graphicsManager == null)
			{
				throw new ArgumentNullException("graphicsManager");
			}

			this.renderToSurface = new SlimDX.Direct3D9.RenderToSurface(graphicsManager.device, width, height, SlimDX.Direct3D9.Format.A8R8G8B8);

			SlimDX.Direct3D9.Texture texture = new SlimDX.Direct3D9.Texture(graphicsManager.device, width, height, 0, SlimDX.Direct3D9.Usage.RenderTarget,
																			SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Default);

			this.ConstructTexture(texture, width, height);
		}
				
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if(disposing)
			{
				if(this.renderToSurface != null)
				{
					this.renderToSurface.Dispose();
					this.renderToSurface = null;
				}
			}
		}
	}
}
