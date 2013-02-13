using System;
using System.IO;
using NAudio.Wave;

namespace Snowball.Sound
{
	public sealed class SoundDevice : ISoundDevice, IDisposable
	{						
		/// <summary>
		/// Whether or not the sound device has been created.
		/// </summary>
		public bool IsDeviceCreated
		{
            get { return true; }
		}

		public SoundDevice()
		{
		}

		~SoundDevice()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}

		public void CreateDevice()
		{
		}

		public SoundEffect LoadSoundEffect(Stream stream)
		{
			return SoundEffect.FromStream(this, stream);
		}
	}
}
