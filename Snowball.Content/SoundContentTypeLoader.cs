using System;
using Snowball.Sound;

namespace Snowball.Content
{
	/// <summary>
	/// Base class for content type loaders which load sound based content.
	/// </summary>
	/// <typeparam name="TContent"></typeparam>
	/// <typeparam name="TLoadArgs"></typeparam>
	public abstract class SoundContentTypeLoader<TContent, TLoadArgs> : ContentTypeLoader<TContent, TLoadArgs>
		where TLoadArgs : LoadContentArgs
	{
		ISoundDevice soundDevice;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="services"></param>
		public SoundContentTypeLoader(IServiceProvider services)
			: base(services)
		{
		}

		/// <summary>
		/// Gets a handle to ISoundDevice.
		/// </summary>
		/// <returns></returns>
		protected ISoundDevice GetSoundDevice()
		{
			if (this.soundDevice == null)
				this.soundDevice = (ISoundDevice)this.Services.GetRequiredService<ISoundDevice>();

			return this.soundDevice;
		}
	}
}
