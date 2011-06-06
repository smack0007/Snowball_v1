using System;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Interface for game components.
	/// </summary>
	public interface IGameEntity
	{
		bool IsInitialized { get; }

		bool IsActive { get; }

		bool IsVisible { get; }

		void Initialize();

		void Update(GameTime gameTime);

		void Draw(IRenderer renderer);
	}
}
