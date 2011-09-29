using System;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Base class for components in your game.
	/// </summary>
	public class GameComponent : IGameComponent
	{
		public GameComponent()
		{
		}

		public virtual void Initialize()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(IRenderer renderer)
		{
		}
	}
}
