using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// A surface which can be drawn onto.
	/// </summary>
	public class RenderTarget : Texture
	{
		internal SlimDX.Direct3D9.RenderToSurface renderToSurface;
		
		internal RenderTarget(SlimDX.Direct3D9.RenderToSurface renderToSurface, SlimDX.Direct3D9.Texture texture, int width, int height)
			: base(texture, width, height)
		{
			if(renderToSurface == null)
				throw new ArgumentNullException("renderToSurface");

			this.renderToSurface = renderToSurface;
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
