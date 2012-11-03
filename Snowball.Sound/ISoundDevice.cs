using System;
using System.IO;

namespace Snowball.Sound
{
	public interface ISoundDevice
	{
		/// <summary>
		/// Whether or not the sound device has been created.
		/// </summary>
		bool IsDeviceCreated { get; }

		SoundEffect LoadSoundEffect(Stream stream);
	}
}
