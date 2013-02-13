using System;
using System.IO;
using NAudio.Wave;

namespace Snowball.Sound
{
	public sealed class SoundEffect : DisposableObject
	{
        SoundDevice soundDevice;
        WaveStream waveStream;
        WaveOutEvent waveOut;

		internal SoundEffect(SoundDevice soundDevice, WaveStream waveStream)
		{
            this.soundDevice = soundDevice;
            this.waveStream = waveStream;
            
            this.waveOut = new WaveOutEvent();
            this.waveOut.Init(this.waveStream);
		}
				
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
                if (this.waveOut != null)
                {
                    this.waveOut.Dispose();
                    this.waveOut = null;
                }

				if (this.waveStream != null)
				{
					this.waveStream.Dispose();
					this.waveStream = null;
				}
			}
		}

		public static SoundEffect FromFile(SoundDevice soundDevice, string fileName)
		{
            if (soundDevice == null)
                throw new ArgumentNullException("soundDevice");

            if (fileName == null)
                throw new ArgumentNullException("fileName");

			if (!File.Exists(fileName))
				throw new FileNotFoundException(string.Format("Unable to load file \"{0}\".", fileName));

			using(Stream stream = File.OpenRead(fileName))
				return FromStream(soundDevice, stream);
		}

		public static SoundEffect FromStream(SoundDevice soundDevice, Stream stream)
		{
            if (soundDevice == null)
                throw new ArgumentNullException("soundDevice");

            if (stream == null)
                throw new ArgumentNullException("stream");
                        
            return new SoundEffect(soundDevice, CreateInputStream(stream));
		}

        private static WaveStream CreateInputStream(Stream stream)
        {                        
            WaveStream readerStream = new WaveFileReader(stream);
            
            if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
            {
                readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
                readerStream = new BlockAlignReductionStream(readerStream);
            }
            
            if (readerStream.WaveFormat.BitsPerSample != 16)
            {
                WaveFormat format = new WaveFormat(readerStream.WaveFormat.SampleRate, 16, readerStream.WaveFormat.Channels);
                readerStream = new WaveFormatConversionStream(format, readerStream);
            }
            
            return new WaveChannel32(readerStream);
        }

		public void Play()
		{
            if (this.waveOut.PlaybackState == PlaybackState.Playing)
                this.waveOut.Stop();

            this.waveStream.Position = 0;
            this.waveOut.Play();
		}
	}
}
