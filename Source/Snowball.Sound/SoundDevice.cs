using System;
using System.IO;

namespace Snowball.Sound
{
	public sealed class SoundDevice : ISoundDevice, IDisposable
	{
		internal SharpDX.XAudio2.XAudio2 InternalDevice;
		internal SharpDX.XAudio2.MasteringVoice InternalMasteringVoice;
				
		public SoundDevice()
		{
			this.InternalDevice = new SharpDX.XAudio2.XAudio2();
			this.InternalMasteringVoice = new SharpDX.XAudio2.MasteringVoice(this.InternalDevice);
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
				if (this.InternalMasteringVoice != null)
				{
					this.InternalMasteringVoice.Dispose();
					this.InternalMasteringVoice = null;
				}

				if (this.InternalDevice != null)
				{
					this.InternalDevice.Dispose();
					this.InternalDevice = null;
				}
			}
		}

		public SoundEffect LoadSoundEffect(Stream stream)
		{
			return SoundEffect.FromStream(this, stream);
		}
	}
}
