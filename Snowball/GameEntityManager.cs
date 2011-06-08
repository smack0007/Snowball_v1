using System;
using System.Collections.Generic;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Manages a collection of game entities.
	/// </summary>
	public class GameEntityManager
	{
		List<GameEntity> entities;
		bool isInitialized;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameEntityManager()
		{
			this.entities = new List<GameEntity>();
		}

		/// <summary>
		/// Adds an object to be managed.
		/// </summary>
		/// <param name="entity"></param>
		public void Add(GameEntity entity)
		{
			if(!this.entities.Contains(entity))
			{
				this.entities.Add(entity);

				entity.Manager = this;

				if(this.isInitialized && !entity.IsInitialized)
					entity.Initialize();
			}
		}

		/// <summary>
		/// Removes an object.
		/// </summary>
		/// <param name="entity"></param>
		public void Remove(GameEntity entity)
		{
			if(entity.Manager != this)
				throw new InvalidOperationException("Entity is not being managed by this manager.");

			this.entities.Remove(entity);
			entity.Manager = null;
		}

		/// <summary>
		/// Initializes all uninitialized entities.
		/// </summary>
		public void Initialize()
		{
			this.isInitialized = true;

			foreach(GameEntity entity in this.entities)
				if(!entity.IsInitialized)
					entity.Initialize();
		}

		/// <summary>
		/// Updates all enabled entities.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			foreach(GameEntity entity in this.entities)
				if(entity.IsActive)
					entity.Update(gameTime);
		}

		/// <summary>
		/// Draws all visible entities.
		/// </summary>
		/// <param name="renderer"></param>
		public void Draw(IRenderer renderer)
		{
			foreach(GameEntity entity in this.entities)
				if(entity.IsVisible)
					entity.Draw(renderer);
		}
	}
}
