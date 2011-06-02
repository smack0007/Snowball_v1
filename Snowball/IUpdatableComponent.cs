using System;

namespace Snowball
{
	/// <summary>
	/// Interface for game components which need to be updated.
	/// </summary>
	public interface IUpdatableComponent : IGameComponent
	{
		/// <summary>
		/// Whether or not the component is should be updated.
		/// </summary>
		bool Enabled { get; }

		/// <summary>
		/// Allows the component to update itself.
		/// </summary>
		/// <param name="gameTime"></param>
		void Update(GameTime gameTime);
	}
}
