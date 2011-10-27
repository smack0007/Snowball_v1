using System;
using System.IO;

namespace Snowball.Sound
{
	public sealed class SoundDevice : ISoundDevice, IDisposable
	{
		internal SlimDX.XAudio2.XAudio2 InternalDevice;
		internal SlimDX.XAudio2.MasteringVoice InternalMasteringVoice;
		
		/// <summary>
		/// Whether or not the sound device has been created.
		/// </summary>
		public bool IsDeviceCreated
		{
			get { return this.InternalDevice != null; }
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
			if(disposing)
			{				
				if(this.InternalMasteringVoice != null)
				{
					this.InternalMasteringVoice.Dispose();
					this.InternalMasteringVoice = null;
				}

				if(this.InternalDevice != null)
				{
					this.InternalDevice.Dispose();
					this.InternalDevice = null;
				}
			}
		}

		public void CreateDevice()
		{
			this.InternalDevice = new SlimDX.XAudio2.XAudio2();
			this.InternalMasteringVoice = new SlimDX.XAudio2.MasteringVoice(this.InternalDevice);
		}

		public SoundEffect LoadSoundEffect(Stream stream)
		{
			return SoundEffect.FromStream(this, stream);
		}
	}
}
