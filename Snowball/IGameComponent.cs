using System;
using Snowball.Graphics;

namespace Snowball
{
	public interface IGameComponent
	{
		void Initialize();

		void Update(GameTime gameTime);

		void Draw(IRenderer renderer);
	}
}
