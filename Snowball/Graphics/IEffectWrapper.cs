using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// Interface for classes which wrap an Effect.
	/// </summary>
	public interface IEffectWrapper
	{
		/// <summary>
		/// The Effect being wrapped.
		/// </summary>
		Effect Effect { get; }
	}
}
