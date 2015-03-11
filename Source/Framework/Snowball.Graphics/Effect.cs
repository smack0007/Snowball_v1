using System;
using System.IO;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public sealed class Effect : DisposableObject, IEffect
	{
		internal D3D.Effect InternalEffect;

		GraphicsDevice graphicsDevice;

		public Matrix TransformMatrix
		{
			get { return this.GetValue<Matrix>("TransformMatrix"); }
			set { this.SetValue<Matrix>("TransformMatrix", value); }
		}

		private Effect(D3D.Effect effect, GraphicsDevice graphicsDevice)
		{
			if (effect == null)
				throw new ArgumentNullException("effect");

			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			this.InternalEffect = effect;
			this.graphicsDevice = graphicsDevice;

			this.graphicsDevice.DeviceLost += this.GraphicsDevice_DeviceLost;
		}

		protected override void Dispose(bool disposing)
		{
			if (this.InternalEffect != null)
			{
				this.InternalEffect.Dispose();
				this.InternalEffect = null;
			}

			if (this.graphicsDevice != null)
			{
				this.graphicsDevice.DeviceLost -= this.GraphicsDevice_DeviceLost;
				this.graphicsDevice = null;
			}
		}

		private void GraphicsDevice_DeviceLost(object sender, EventArgs e)
		{
			this.InternalEffect.OnLostDevice();
		}
		
		/// <summary>
		/// Loads an Effect from a file.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="fileName"></param>
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

			string source = null;
			using (StreamReader sr = new StreamReader(stream))
				source = sr.ReadToEnd();

			// TODO: Using D3D.Effect.FromStream seems to throw an exception for no reason.

			D3D.Effect effect = null;

			try
			{
				effect = D3D.Effect.FromString(graphicsDevice.d3d9Device, source, D3D.ShaderFlags.None);
			}
			catch (Exception ex)
			{
				throw new GraphicsException(ex.Message);
			}

			return new Effect(effect, graphicsDevice);
		}

		public void Begin(int technique, int pass)
		{
			D3D.EffectHandle techniqueHandle = this.InternalEffect.GetTechnique(technique);
			this.InternalEffect.Technique = techniqueHandle;
			this.InternalEffect.Begin();
			this.InternalEffect.BeginPass(pass);
		}

		public void End()
		{
			this.InternalEffect.EndPass();
			this.InternalEffect.End();
		}

		/// <summary>
		/// Gets a value from the effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T GetValue<T>(string name) where T: struct
		{
			try
			{
				return this.InternalEffect.GetValue<T>(name);
			}
			catch (Exception)
			{
				throw new GraphicsException("Failed to get effect parameter value for \"" + name + "\".");
			}
		}

		/// <summary>
		/// Sets a value in the effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetValue<T>(string name, T value) where T: struct
		{
			try
			{
				this.InternalEffect.SetValue(name, value);
			}
			catch (Exception)
			{
				throw new GraphicsException("Failed to set effect parameter value for \"" + name + "\".");
			}
		}
	}
}
