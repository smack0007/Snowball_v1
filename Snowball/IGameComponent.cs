using System;
using Snowball.Content;
using Snowball.Graphics;

namespace Snowball
{
	public interface IGameComponent
	{
		bool IsInitialized { get; }

		bool IsContentLoaded { get; }

		bool Enabled { get; }

		bool Visible { get; }

		void Initialize();

		void LoadContent(IContentLoader contentLoader);

		void UnloadContent();

		void Update(GameTime gameTime);

		void Draw(IRenderer renderer);
	}
}
