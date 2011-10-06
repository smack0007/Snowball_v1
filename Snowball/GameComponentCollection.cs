using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;

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
			foreach(GameComponent component in this.components)
				component.Initialize();
		}

		public void Update(GameTime gameTime)
		{
			foreach(GameComponent component in this.components)
				component.Update(gameTime);
		}

		public void Draw(IRenderer renderer)
		{
			foreach(GameComponent component in this.components)
				component.Draw(renderer);
		}
	}
}
