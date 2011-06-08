using System;
using System.Collections.Generic;
using Snowball.Graphics;

namespace Snowball
{
	public class GameState
	{
		/// <summary>
		/// The manager which controls the state.
		/// </summary>
		public GameStateManager Manager
		{
			get;
			internal set;
		}

		/// <summary>
		/// Manager for entities contained within the state.
		/// </summary>
		public GameEntityManager Entities
		{
			get;
			private set;
		}

		/// <summary>
		/// Whether or not the state has been initialized.
		/// </summary>
		public bool IsInitialized
		{
			get;
			protected set;
		}

		/// <summary>
		/// Whether or not the state blocks updating of states underneath it.
		/// </summary>
		public bool BlocksUpdate
		{
			get;
			protected set;
		}

		/// <summary>
		/// Whehter or not the state blocks drawing of states underneath it.
		/// </summary>
		public bool BlocksDraw
		{
			get;
			protected set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameState()
		{
			this.Entities = new GameEntityManager();
		}

		/// <summary>
		/// Allows the state to initialize itself.
		/// </summary>
		public virtual void Initialize()
		{
			this.IsInitialized = true;
			this.Entities.Initialize();
		}

		/// <summary>
		/// Allows the state to update itself.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			this.Entities.Update(gameTime);
		}

		/// <summary>
		/// Allows the state to draw itself.
		/// </summary>
		/// <param name="renderer"></param>
		public virtual void Draw(IRenderer renderer)
		{
			this.Entities.Draw(renderer);
		}
	}
}
