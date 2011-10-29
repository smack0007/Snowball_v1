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
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public SoundEffectLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected override SoundEffect LoadContent(Stream stream, LoadSoundEffectArgs information)
		{
			return this.GetSoundDevice().LoadSoundEffect(stream);		
		}
	}
}
