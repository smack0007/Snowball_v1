using System;
using Snowball.Graphics;

namespace Snowball.Content
{
	public abstract class GraphicsContentTypeLoader<TContent, TLoadInformation> : ContentTypeLoader<TContent, TLoadInformation>
		where TLoadInformation : LoadContentArgs
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
