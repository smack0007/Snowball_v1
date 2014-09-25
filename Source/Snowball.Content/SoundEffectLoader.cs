using System;
using System.IO;
using Snowball.Sound;

namespace Snowball.Content
{
	/// <summary>
	/// Content type loader for SoundEffect(s).
	/// </summary>
	public class SoundEffectLoader : SoundContentTypeLoader<SoundEffect, LoadSoundEffectArgs>
	{
        private static readonly Type[] loadContentArgsTypes = new Type[] { typeof(LoadSoundEffectArgs) };

        protected override Type[] LoadContentArgsTypes
        {
            get { return loadContentArgsTypes; }
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public SoundEffectLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override SoundEffect LoadContent(Stream stream, LoadContentArgs args)
		{
			return this.GetSoundDevice().LoadSoundEffect(stream);
		}
	}
}
