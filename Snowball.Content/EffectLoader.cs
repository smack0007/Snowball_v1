using System;
using System.IO;
using Snowball.Graphics;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for Effect(s).
	/// </summary>
	public class EffectLoader : GraphicsContentTypeLoader<Effect>
	{
        private static readonly Type[] loadContentArgsTypes = new Type[] { typeof(LoadEffectArgs) };

        protected override Type[] LoadContentArgsTypes
        {
            get { return loadContentArgsTypes; }
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public EffectLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override Effect LoadContent(Stream stream, LoadContentArgs args)
		{
			return this.GetGraphicsDevice().LoadEffect(stream);
		}
	}
}
