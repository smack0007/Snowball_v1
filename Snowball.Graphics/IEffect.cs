using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// Interface for Effect(s).
	/// </summary>
	public interface IEffect
	{
		void Begin(int technique, int pass);

		void End();
	}
}
