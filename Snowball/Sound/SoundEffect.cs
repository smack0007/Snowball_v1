using System;
using System.IO;

namespace Snowball.Sound
{
	public sealed class SoundEffect : GameResource
	{
		SlimDX.Multimedia.WaveStream waveStream;
		SlimDX.XAudio2.AudioBuffer audioBuffer;
		SlimDX.XAudio2.SourceVoice sourceVoice;

		internal SoundEffect(SlimDX.XAudio2.XAudio2 device, SlimDX.Multimedia.WaveStream waveStream)
		{
			if(waveStream == null)
				throw new ArgumentNullException("waveStream");

			this.waveStream = waveStream;
			
			this.audioBuffer = new SlimDX.XAudio2.AudioBuffer();
			this.audioBuffer.AudioData = this.waveStream;
			this.audioBuffer.AudioBytes = (int)this.waveStream.Length;

			this.sourceVoice = new SlimDX.XAudio2.SourceVoice(device, this.waveStream.Format);
		}
				
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.sourceVoice != null)
				{
					this.sourceVoice.Dispose();
					this.sourceVoice = null;
				}

				if(this.audioBuffer != null)
				{
					this.audioBuffer.Dispose();
					this.audioBuffer = null;
				}

				if(this.waveStream != null)
				{
					this.audioBuffer.Dispose();
					this.audioBuffer = null;
				}
			}
		}

		public static SoundEffect FromFile(SoundDevice soundDevice, string fileName)
		{
			if(!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file \"" + fileName + "\".");

			using(Stream stream = File.OpenRead(fileName))
				return FromStream(soundDevice, stream);
		}

		public static SoundEffect FromStream(SoundDevice soundDevice, Stream stream)
		{
			SlimDX.Multimedia.WaveStream waveStream = new SlimDX.Multimedia.WaveStream(stream);
			return new SoundEffect(soundDevice.InternalDevice, waveStream);
		}

		public void Play()
		{
			this.sourceVoice.Stop();
			this.sourceVoice.FlushSourceBuffers();

			this.sourceVoice.SubmitSourceBuffer(this.audioBuffer);
			this.sourceVoice.Start();
		}
	}
}
