using System;
using Snowball.Graphics;

namespace Snowball.Content
{
	public abstract class GraphicsContentTypeLoader<TContent, TLoadArgs> : ContentTypeLoader<TContent, TLoadArgs>
		where TLoadArgs : LoadContentArgs
	{
		IGraphicsDevice graphicsDevice;

		public GraphicsContentTypeLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected IGraphicsDevice GetGraphicsDevice()
		{
			if(this.graphicsDevice == null)
				this.graphicsDevice = (IGraphicsDevice)this.Services.GetRequiredGameService(typeof(IGraphicsDevice));

			return this.graphicsDevice;
		}
	}
}
