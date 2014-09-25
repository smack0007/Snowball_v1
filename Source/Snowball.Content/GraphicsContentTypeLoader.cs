using System;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Base class for content type loaders which load graphics based content.
	/// </summary>
	/// <typeparam name="TContent"></typeparam>
	/// <typeparam name="TLoadArgs"></typeparam>
	public abstract class GraphicsContentTypeLoader<TContent> : ContentTypeLoader<TContent>
		where TContent : class
	{
		IGraphicsDevice graphicsDevice;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public GraphicsContentTypeLoader(IServiceProvider services)
			: base(services)
		{
		}

		/// <summary>
		/// Gets a handle to IGraphicsDevice.
		/// </summary>
		/// <returns></returns>
		protected IGraphicsDevice GetGraphicsDevice()
		{
			if (this.graphicsDevice == null)
				this.graphicsDevice = (IGraphicsDevice)this.Services.GetRequiredService<IGraphicsDevice>();

			return this.graphicsDevice;
		}
	}
}
