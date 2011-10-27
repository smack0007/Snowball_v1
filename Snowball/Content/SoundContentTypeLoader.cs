using System;
using Snowball.Sound;

namespace Snowball.Content
{
	public abstract class SoundContentTypeLoader<TContent, TLoadInformation> : ContentTypeLoader<TContent, TLoadInformation>
		where TLoadInformation : LoadContentArgs
	{
		ISoundDevice soundDevice;

		public SoundContentTypeLoader(IServiceProvider services)
			: base(services)
		{
		}

		private ISoundDevice GetSoundDevice()
		{
			if(this.soundDevice == null)
				this.soundDevice = (ISoundDevice)this.Services.GetRequiredGameService(typeof(ISoundDevice));

			return this.soundDevice;
		}
	}
}
