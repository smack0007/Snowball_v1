using System;
using System.Collections.Generic;

namespace Snowball
{
	/// <summary>
	/// Manager for subsystems in a game.
	/// </summary>
	public class GameSubsystemManager
	{
		List<GameSubsystem> subsystems;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameSubsystemManager()
		{
			this.subsystems = new List<GameSubsystem>();
		}

		/// <summary>
		/// Adds a subsystem to be managed.
		/// </summary>
		/// <param name="subsystem"></param>
		public void Add(GameSubsystem subsystem)
		{
			if(!this.subsystems.Contains(subsystem))
				this.subsystems.Add(subsystem);
		}

		/// <summary>
		/// Removes a subsystem.
		/// </summary>
		/// <param name="subsystem"></param>
		public void Remove(GameSubsystem subsystem)
		{
			this.subsystems.Remove(subsystem);
		}

		/// <summary>
		/// Updates all managed subsystems.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			foreach(GameSubsystem subsystem in this.subsystems)
				if(subsystem.Enabled)
					subsystem.Update(gameTime);
		}
	}
}
