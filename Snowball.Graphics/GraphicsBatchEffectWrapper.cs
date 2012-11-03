using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Graphics
{
	/// <summary>
	/// Wraps a normal Effect class to make it compatible with GraphicsBatch.
	/// </summary>
	public class GraphicsBatchEffectWrapper : IEffect, IGraphicsBatchEffect
	{
		Effect effect;

		public Matrix TransformMatrix
		{
			set { this.effect.SetValue("TransformMatrix", value); }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="effect"></param>
		public GraphicsBatchEffectWrapper(Effect effect)
		{
			if (effect == null)
				throw new ArgumentNullException("effect");

			this.effect = effect;
		}

		public void Begin(int technique, int pass)
		{
			this.effect.Begin(technique, pass);
		}

		public void End()
		{
			this.effect.End();
		}

		/// <summary>
		/// Gets a value from the effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T GetValue<T>(string name) where T : struct
		{
			return this.effect.GetValue<T>(name);
		}

		/// <summary>
		/// Sets a value in the effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetValue<T>(string name, T value) where T : struct
		{
			this.effect.SetValue<T>(name, value);
		}
	}
}
