using System;
using System.IO;

namespace Snowball.Sound
{
	public class SoundManager : ISoundManager, IDisposable
	{
		SlimDX.XAudio2.XAudio2 device;
		SlimDX.XAudio2.MasteringVoice masteringVoice;
		
		/// <summary>
		/// Whether or not the sound device has been created.
		/// </summary>
		public bool IsDeviceCreated
		{
			get { return this.device != null; }
		}

		public SoundManager()
		{
		}

		~SoundManager()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{				
				if(this.masteringVoice != null)
				{
					this.masteringVoice.Dispose();
					this.masteringVoice = null;
				}

				if(this.device != null)
				{
					this.device.Dispose();
					this.device = null;
				}
			}
		}

		public void CreateDevice()
		{
			this.device = new SlimDX.XAudio2.XAudio2();
			this.masteringVoice = new SlimDX.XAudio2.MasteringVoice(this.device);
		}

		public SoundEffect LoadSoundEffect(string fileName)
		{
			if(!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file " + fileName + ".");

			SoundEffect sound = null;

			using(FileStream file = File.OpenRead(fileName))
			{
				SlimDX.Multimedia.WaveStream waveStream = new SlimDX.Multimedia.WaveStream(file);
				sound = new SoundEffect(this.device, waveStream);
			}

			return sound;
		}
	}
}
