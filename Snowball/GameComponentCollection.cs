using System;
using System.Collections.Generic;
using Snowball.Graphics;
using Snowball.Content;

namespace Snowball
{
	public class GameComponentCollection
	{
		List<GameComponent> components;

		/// <summary>
		/// Initializes a new GameComponentCollection.
		/// </summary>
		public GameComponentCollection()
		{
			this.components = new List<GameComponent>();
		}

		public void AddComponent(GameComponent component)
		{
			this.components.Add(component);
		}

		public void RemoveComponent(GameComponent component)
		{
			this.components.Remove(component);
		}

		public void Initialize()
		{
			foreach (GameComponent component in this.components)
				if (!component.IsInitialized)
					component.Initialize();
		}

		public void LoadContent(IContentLoader contentLoader)
		{
			if (contentLoader == null)
				throw new ArgumentNullException("contentLoader");

			foreach (GameComponent component in this.components)
				if (!component.IsContentLoaded)
					component.LoadContent(contentLoader);
		}

		public void UnloadContent()
		{
			foreach (GameComponent component in this.components)
				if (component.IsContentLoaded)
					component.UnloadContent();
		}

		public void Update(GameTime gameTime)
		{
			foreach (GameComponent component in this.components)
				if (component.Enabled)
					component.Update(gameTime);
		}

		public void Draw(IRenderer renderer)
		{
			foreach (GameComponent component in this.components)
				if (component.Visible)
					component.Draw(renderer);
		}
	}
}
