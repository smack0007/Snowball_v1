using System;
using System.Collections.Generic;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Manages a collection of GameObject(s).
	/// </summary>
	public class GameComponentManager : GameComponent
	{
		List<IGameComponent> components;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public GameComponentManager()
		{
			this.components = new List<IGameComponent>();
		}

		/// <summary>
		/// Adds an object to be managed.
		/// </summary>
		/// <param name="component"></param>
		public void Add(IGameComponent component)
		{
			this.components.Add(component);

			if(this.IsInitialized && component is IInitializableComponent)
			{
				IInitializableComponent initializable = (IInitializableComponent)component;

				if(!initializable.IsInitialized)
					initializable.Initialize();
			}
		}

		/// <summary>
		/// Removes an object.
		/// </summary>
		/// <param name="component"></param>
		public void Remove(IGameComponent component)
		{
			this.components.Remove(component);
		}

		/// <summary>
		/// Initializes all unitialized managed objects.
		/// </summary>
		public override void Initialize()
		{
			this.IsInitialized = true;

			foreach(IGameComponent component in this.components)
			{
				if(component is IInitializableComponent)
				{
					IInitializableComponent initializable = (IInitializableComponent)component;

					if(!initializable.IsInitialized)
						initializable.Initialize();
				}
			}
		}

		/// <summary>
		/// Updates all enabled managed objects.
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			foreach(IGameComponent component in this.components)
			{
				if(component is IUpdatableComponent)
				{
					IUpdatableComponent updatable = (IUpdatableComponent)component;

					if(updatable.Enabled)
						updatable.Update(gameTime);
				}
			}
		}

		/// <summary>
		/// Draws all visible managed objects.
		/// </summary>
		/// <param name="renderer"></param>
		public override void Draw(GameTime gameTime)
		{
			foreach(IGameComponent component in this.components)
			{
				if(component is IDrawableComponent)
				{
					IDrawableComponent drawable = (IDrawableComponent)component;

					if(drawable.Visible)
						drawable.Draw(gameTime);
				}
			}
		}
	}
}
