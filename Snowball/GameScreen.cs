using System;
using System.Collections.Generic;
using Snowball.Graphics;

namespace Snowball
{
	public class GameScreen
	{
		/// <summary>
		/// The manager which controls the screen.
		/// </summary>
		public GameScreenManager Manager
		{
			get;
			internal set;
		}

		/// <summary>
		/// Manager for entities contained within the screen.
		/// </summary>
		public GameEntityManager Entities
		{
			get;
			private set;
		}

		/// <summary>
		/// Whether or not the screen has been initialized.
		/// </summary>
		public bool IsInitialized
		{
			get;
			protected set;
		}

		/// <summary>
		/// Whether or not the screen blocks updating of screens underneath it.
		/// </summary>
		public bool BlocksUpdate
		{
			get;
			protected set;
		}

		/// <summary>
		/// Whehter or not the screen blocks drawing of screens underneath it.
		/// </summary>
		public bool BlocksDraw
		{
			get;
			protected set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameScreen()
		{
			this.Entities = new GameEntityManager();
		}

		/// <summary>
		/// Allows the screen to initialize itself.
		/// </summary>
		public virtual void Initialize()
		{
			this.IsInitialized = true;
			this.Entities.Initialize();
		}

		/// <summary>
		/// Allows the screen to update itself.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			this.Entities.Update(gameTime);
		}

		/// <summary>
		/// Allows the screen to draw itself.
		/// </summary>
		/// <param name="renderer"></param>
		public virtual void Draw(IRenderer renderer)
		{
			this.Entities.Draw(renderer);
		}

		/// <summary>
		/// Brings the screen to the front.
		/// </summary>
		public void BringToFront()
		{
			if(this.Manager == null)
				throw new InvalidOperationException("Screen not currently managed.");

			this.Manager.BringToFront(this);
		}

		/// <summary>
		/// Sends the screen to the back.
		/// </summary>
		public void SendToBack()
		{
			if(this.Manager == null)
				throw new InvalidOperationException("Screen not currently managed.");

			this.Manager.SendToBack(this);
		}
	}
}
