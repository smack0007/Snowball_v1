using System;
using System.IO;

namespace Snowball.Sound
{
	public interface ISoundDevice
	{		
		SoundEffect LoadSoundEffect(Stream stream);
	}
}
