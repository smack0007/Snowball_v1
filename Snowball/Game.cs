﻿using System;
using Snowball.Graphics;
using Snowball.Input;

namespace Snowball
{
	public abstract class Game : IDisposable
	{
		GameClock gameClock;
		GameTime gameTime;
		
		/// <summary>
		/// The window the game is running in.
		/// </summary>
		public IGameWindow Window
		{
			get;
			private set;
		}

		/// <summary>
		/// The background color of the game.
		/// </summary>
		public Color BackgroundColor
		{
			get;
			set;
		}

		/// <summary>
		/// Handle to the graphics manager.
		/// </summary>
		public GraphicsManager Graphics
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public GameComponentManager Components
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new Game instance with the default GameWindow.
		/// </summary>
		public Game()
			: this(new GameWindow())
		{
		}

		/// <summary>
		/// Initializes a new Game instance with the given IGameWindow.
		/// </summary>
		public Game(IGameWindow window)
		{
			if(window == null)
				throw new ArgumentNullException("host");
						
			this.Window = window;
			this.SubscribeHostEvents();

			this.gameClock = new GameClock();
			this.gameTime = new GameTime();

			this.Graphics = new GraphicsManager() ;
			this.Graphics.CreateDevice(this.Window);

			this.Components = new GameComponentManager();
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~Game()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Disposes of the game.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// Called when the game is being disposed.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.Window != null)
				{
					UnsubscribeHostEvents();
					this.Window = null;
				}
			}
		}

		/// <summary>
		/// Subscribes to events on the host.
		/// </summary>
		private void SubscribeHostEvents()
		{
			this.Window.Idle += this.Window_Idle;
			this.Window.Activate += this.Window_Activate;
			this.Window.Deactivate += this.Window_Deactivate;
			this.Window.Exiting += this.Window_Exiting;
		}

		/// <summary>
		/// Subscribes to events on the host.
		/// </summary>
		private void UnsubscribeHostEvents()
		{
			this.Window.Idle -= this.Window_Idle;
			this.Window.Activate -= this.Window_Activate;
			this.Window.Deactivate -= this.Window_Deactivate;
			this.Window.Exiting -= this.Window_Exiting;
		}

		/// <summary>
		/// Triggers the main loop for the game.
		/// </summary>
		public void Run()
		{
			this.Initialize();
			this.Window.Run();
		}

		/// <summary>
		/// Called when the host has idle time.
		/// </summary>
		private void Window_Idle(object sender, EventArgs e)
		{
			this.gameClock.Tick();

			this.gameTime.ElapsedTime = this.gameClock.ElapsedTimeSinceUpdate;
			this.gameTime.TotalTime += this.gameClock.ElapsedTimeSinceTick;

			if(this.gameClock.ShouldUpdate)
			{
				this.Update(this.gameTime);
				this.gameClock.ResetShouldUpdate();
			}

			if(this.gameClock.ShouldDraw)
			{
				if(this.Graphics != null && this.Graphics.IsGraphicsDeviceCreated)
				{
					this.Graphics.Clear(this.BackgroundColor);
					this.Graphics.BeginDraw();
					this.Draw(this.gameTime);
					this.Graphics.EndDraw();
					this.Graphics.Present();
				}

				this.gameClock.ResetShouldDraw();
			}
		}
		
		/// <summary>
		/// Called when the host is activated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Activate(object sender, EventArgs e)
		{
			this.gameClock.Resume();
		}

		/// <summary>
		/// Called when the host is deactivated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Deactivate(object sender, EventArgs e)
		{
			this.gameClock.Pause();
		}

		/// <summary>
		/// Called when the host is exiting.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Exiting(object sender, EventArgs e)
		{
			this.OnExiting(EventArgs.Empty);
		}

		/// <summary>
		/// Called when the game should initialize.
		/// </summary>
		public virtual void Initialize()
		{
			this.Components.Initialize();
		}

		/// <summary>
		/// Called when the game should update.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			this.Components.Update(gameTime);
		}

		/// <summary>
		/// Called when the game should draw.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Draw(GameTime gameTime)
		{
			this.Components.Draw(gameTime);
		}

		/// <summary>
		/// Called when the game is exiting.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnExiting(EventArgs e)
		{
		}
	}
}
