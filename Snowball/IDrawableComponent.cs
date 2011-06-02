using System;

namespace Snowball
{
	/// <summary>
	/// Interface for game components which need to be drawn.
	/// </summary>
	public interface IDrawableComponent : IGameComponent
	{
		/// <summary>
		/// Whether or not the component is should be drawn.
		/// </summary>
		bool Visible { get; }

		/// <summary>
		/// Allows the component to draw itself.
		/// </summary>
		/// <param name="gameTime"></param>
		void Draw(GameTime gameTime);
	}
}
