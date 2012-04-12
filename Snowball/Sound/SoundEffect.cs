using System;
using System.IO;

namespace Snowball.Sound
{
	public sealed class SoundEffect : GameResource
	{
		SharpDX.Multimedia.SoundStream waveStream;
		SharpDX.XAudio2.AudioBuffer audioBuffer;
		SharpDX.XAudio2.SourceVoice sourceVoice;

		internal SoundEffect(SharpDX.XAudio2.XAudio2 device, SharpDX.Multimedia.SoundStream waveStream)
		{
			if (waveStream == null)
				throw new ArgumentNullException("waveStream");

			this.waveStream = waveStream;
			
			this.audioBuffer = new SharpDX.XAudio2.AudioBuffer();
			this.audioBuffer.Stream = this.waveStream;
			this.audioBuffer.AudioBytes = (int)this.waveStream.Length;

			this.sourceVoice = new SharpDX.XAudio2.SourceVoice(device, this.waveStream.Format);
		}
				
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.sourceVoice != null)
				{
					this.sourceVoice.Dispose();
					this.sourceVoice = null;
				}

				//if (this.audioBuffer != null)
				//{
				//    this.audioBuffer.Dispose();
				//    this.audioBuffer = null;
				//}

				//if (this.waveStream != null)
				//{
				//    this.audioBuffer.Dispose();
				//    this.audioBuffer = null;
				//}
			}
		}

		public static SoundEffect FromFile(SoundDevice soundDevice, string fileName)
		{
			if (!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file \"" + fileName + "\".");

			using(Stream stream = File.OpenRead(fileName))
				return FromStream(soundDevice, stream);
		}

		public static SoundEffect FromStream(SoundDevice soundDevice, Stream stream)
		{
			SharpDX.Multimedia.SoundStream waveStream = new SharpDX.Multimedia.SoundStream(stream);
			return new SoundEffect(soundDevice.InternalDevice, waveStream);
		}

		public void Play()
		{
			this.sourceVoice.Stop();
			this.sourceVoice.FlushSourceBuffers();

			// TODO: Fix playing of SoundEffect.
			//this.sourceVoice.SubmitSourceBuffer(this.audioBuffer);
			this.sourceVoice.Start();
		}
	}
}
