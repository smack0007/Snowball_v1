using System;

namespace Snowball.Sound
{
	public sealed class SoundEffect : IDisposable
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

		~SoundEffect()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		private void Dispose(bool disposing)
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

		public void Play()
		{
			this.sourceVoice.SubmitSourceBuffer(this.audioBuffer);
			this.sourceVoice.Start();
		}
	}
}
