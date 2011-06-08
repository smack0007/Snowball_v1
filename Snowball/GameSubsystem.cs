using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball
{
	/// <summary>
	/// Base class for subsystems.
	/// </summary>
	public class GameSubsystem
	{
		/// <summary>
		/// If true the subsystem should be updated.
		/// </summary>
		public bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// Allows the subsystem to update itself.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
		}
	}
}
