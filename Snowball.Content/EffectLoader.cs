using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for Effect(s).
	/// </summary>
	public class EffectLoader : GraphicsContentTypeLoader<Effect, LoadEffectArgs>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public EffectLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override Effect LoadContent(Stream stream, LoadEffectArgs args)
		{
			return this.GetGraphicsDevice().LoadEffect(stream);
		}
	}
}
