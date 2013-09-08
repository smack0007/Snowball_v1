using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// Interface for Effect(s).
	/// </summary>
	public interface IEffect
	{
		Matrix TransformMatrix { set; }

		void Begin(int technique, int pass);

		void End();
	}
}
