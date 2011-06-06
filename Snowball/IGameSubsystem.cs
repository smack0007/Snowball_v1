using System;

namespace Snowball
{
	/// <summary>
	/// Interface for subsystems in a game.
	/// </summary>
	public interface IGameSubsystem
	{
		/// <summary>
		/// Whether or not the subsystem should be updated.
		/// </summary>
		bool Enabled { get; }

		/// <summary>
		/// Allows the subsystem to update itself.
		/// </summary>
		/// <param name="gameTime"></param>
		void Update(GameTime gameTime);
	}
}
