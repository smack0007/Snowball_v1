using System;
using Snowball.Sound;

namespace Snowball.Content
{
	public abstract class SoundContentTypeLoader<TContent, TLoadArgs> : ContentTypeLoader<TContent, TLoadArgs>
		where TLoadArgs : LoadContentArgs
	{
		ISoundDevice soundDevice;

		public SoundContentTypeLoader(IServiceProvider services)
			: base(services)
		{
		}

		protected ISoundDevice GetSoundDevice()
		{
			if(this.soundDevice == null)
				this.soundDevice = (ISoundDevice)this.Services.GetRequiredGameService(typeof(ISoundDevice));

			return this.soundDevice;
		}
	}
}
