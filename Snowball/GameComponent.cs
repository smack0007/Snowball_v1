using System;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Base class updatable and drawable components in your game.
	/// </summary>
	public class GameComponent : IGameComponent, IInitializableComponent, IUpdatableComponent, IDrawableComponent
	{
		/// <summary>
		/// Whether or not the component has been initialized.
		/// </summary>
		public bool IsInitialized
		{
			get;
			protected set;
		}

		/// <summary>
		/// Whether or not the component should be updated.
		/// </summary>
		public bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// Whether or not the component should be drawn.
		/// </summary>
		public bool Visible
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameComponent()
		{
			this.Enabled = true;
			this.Visible = true;
		}

		/// <summary>
		/// Allows the component to initialize itself.
		/// </summary>
		public virtual void Initialize()
		{
			this.IsInitialized = true;
		}

		/// <summary>
		/// Allows the component to update itself.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
		}

		/// <summary>
		/// Allows the component to draw itself.
		/// </summary>
		/// <param name="renderer"></param>
		public virtual void Draw(GameTime gameTime)
		{
		}
	}
}
