using System;
using System.IO;
using System.Reflection;

namespace Snowball.Graphics
{
	/// <summary>
	/// Wrapper for the standard Effect used.
	/// </summary>
	public sealed class BasicEffect : IEffectWrapper
	{
		/// <summary>
		/// The wrapped Effect.
		/// </summary>
		public Effect Effect
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		public BasicEffect(GraphicsDevice graphicsDevice)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			graphicsDevice.EnsureDeviceCreated();

			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Snowball.Graphics.BasicEffect.fx");

			if (stream == null)
				throw new FileNotFoundException("Failed to load BasicEffect.fx.");

			this.Effect = Effect.FromStream(graphicsDevice, stream);
		}
	}
}
