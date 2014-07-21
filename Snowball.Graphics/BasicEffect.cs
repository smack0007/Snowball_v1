using System;
using System.IO;
using System.Reflection;

namespace Snowball.Graphics
{
	/// <summary>
	/// Wrapper for the standard Effect used.
	/// </summary>
	public sealed class BasicEffect : IEffect
	{
		Effect effect;
		
		public Matrix TransformMatrix
		{
			set { this.effect.SetValue<Matrix>("TransformMatrix", value); }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		public BasicEffect(GraphicsDevice graphicsDevice)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Snowball.Graphics.BasicEffect.fx");

			if (stream == null)
				throw new FileNotFoundException("Failed to load BasicEffect.fx.");

			this.effect = Effect.FromStream(graphicsDevice, stream);
		}

		public void Begin(int technique, int pass)
		{
			this.effect.Begin(technique, pass);
		}

		public void End()
		{
			this.effect.End();
		}
	}
}
