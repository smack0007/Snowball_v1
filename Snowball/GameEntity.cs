using System;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Base class for entities in a game.
	/// </summary>
	public class GameEntity : IGameEntity
	{
		/// <summary>
		/// Whether or not the entity has been initialized.
		/// </summary>
		public bool IsInitialized
		{
			get;
			protected set;
		}

		/// <summary>
		/// Whether or not the entity should be updated.
		/// </summary>
		public bool IsActive
		{
			get;
			set;
		}

		/// <summary>
		/// Whether or not the entity should be drawn.
		/// </summary>
		public bool IsVisible
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameEntity()
		{
			this.IsInitialized = false;
			this.IsActive = true;
			this.IsVisible = true;
		}

		/// <summary>
		/// Allows the entity to initialize itself.
		/// </summary>
		public virtual void Initialize()
		{
			this.IsInitialized = true;
		}

		/// <summary>
		/// Allows the entity to update itself.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
		}

		/// <summary>
		/// Allows the entity to draw itself.
		/// </summary>
		/// <param name="renderer"></param>
		public virtual void Draw(IRenderer renderer)
		{
		}
	}
}
