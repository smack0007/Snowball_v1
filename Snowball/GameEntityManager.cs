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
		List<IGameEntity> entities;
		bool isInitialized;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameEntityManager()
		{
			this.entities = new List<IGameEntity>();
		}

		/// <summary>
		/// Adds an object to be managed.
		/// </summary>
		/// <param name="entity"></param>
		public void Add(IGameEntity entity)
		{
			this.entities.Add(entity);

			if(this.isInitialized && !entity.IsInitialized)
				entity.Initialize();
		}

		/// <summary>
		/// Removes an object.
		/// </summary>
		/// <param name="entity"></param>
		public void Remove(IGameEntity entity)
		{
			this.entities.Remove(entity);
		}

		/// <summary>
		/// Initializes all uninitialized entities.
		/// </summary>
		public void Initialize()
		{
			this.isInitialized = true;

			foreach(IGameEntity entity in this.entities)
				if(!entity.IsInitialized)
					entity.Initialize();
		}

		/// <summary>
		/// Updates all enabled entities.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			foreach(IGameEntity entity in this.entities)
				if(entity.IsActive)
					entity.Update(gameTime);
		}

		/// <summary>
		/// Draws all visible entities.
		/// </summary>
		/// <param name="renderer"></param>
		public void Draw(IRenderer renderer)
		{
			foreach(IGameEntity entity in this.entities)
				if(entity.IsVisible)
					entity.Draw(renderer);
		}
	}
}
