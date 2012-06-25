using System;
using System.IO;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public sealed class Effect : GameResource
	{
		internal D3D.Effect InternalEffect;

		private Effect(D3D.Effect effect)
		{
			if (effect == null)
				throw new ArgumentNullException("effect");

			this.InternalEffect = effect;
		}

		protected override void Dispose(bool disposing)
		{
			if (this.InternalEffect != null)
			{
				this.InternalEffect.Dispose();
				this.InternalEffect = null;
			}
		}
		
		/// <summary>
		/// Loads an Effect from a file.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="fileName"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public static Effect FromFile(GraphicsDevice graphicsDevice, string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");
			
			if (!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file \"" + fileName + "\".");

			using (Stream stream = File.OpenRead(fileName))
				return FromStream(graphicsDevice, stream);
		}

		/// <summary>
		/// Loads a Texture from a stream.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="stream"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public static Effect FromStream(GraphicsDevice graphicsDevice, Stream stream)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			if (stream == null)
				throw new ArgumentNullException("stream");

			graphicsDevice.EnsureDeviceCreated();

			string source = null;
			using (StreamReader sr = new StreamReader(stream))
				source = sr.ReadToEnd();

			// TODO: Using D3D.Effect.FromStream seems to throw an exception for no reason.

			D3D.Effect effect = D3D.Effect.FromString(graphicsDevice.InternalDevice, source, D3D.ShaderFlags.None);			
			return new Effect(effect);
		}

		/// <summary>
		/// Gets a value from the effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T GetValue<T>(string name) where T: struct
		{
			return this.InternalEffect.GetValue<T>(name);
		}

		/// <summary>
		/// Sets a value in the effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetValue<T>(string name, T value) where T: struct
		{
			this.InternalEffect.SetValue(name, value);
		}
	}
}
