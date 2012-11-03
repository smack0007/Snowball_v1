using Snowball.Input;
using System;

namespace Snowball.GameFramework
{
	public static class MouseExtensions
	{
		public static void Update(this Mouse mouse, GameTime gameTime)
		{
			if (mouse == null)
				throw new ArgumentNullException("mouse");

			if (gameTime == null)
				throw new ArgumentNullException("gameTime");

			mouse.Update(gameTime.ElapsedTotalSeconds);
		}
	}
}
