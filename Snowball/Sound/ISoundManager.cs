using System;

namespace Snowball.Sound
{
	public interface ISoundManager
	{
		/// <summary>
		/// Whether or not the sound device has been created.
		/// </summary>
		bool IsDeviceCreated { get; }
		
		void CreateDevice();

		SoundEffect LoadSoundEffect(string fileName);
	}
}
