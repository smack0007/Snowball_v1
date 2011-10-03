﻿using System;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;

namespace Snowball
{
	/// <summary>
	/// Base class for games.
	/// </summary>
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
		/// The services container for the game.
		/// </summary>
		public GameServicesContainer Services
		{
			get;
			private set;
		}

		/// <summary>
		/// The GraphicsDevice for the game.
		/// </summary>
		public GraphicsDevice Graphics
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
				throw new ArgumentNullException("window");
						
			this.Window = window;
			this.SubscribeWindowEvents();

			this.Services = new GameServicesContainer();

			this.Graphics = new GraphicsDevice();
			this.Services.AddService(typeof(IGraphicsDevice), this.Graphics);
			this.SubscribeGraphicsEvents();
			
			this.gameClock = new GameClock();
			this.gameTime = new GameTime();
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
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Called when the game is being disposed.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.Graphics != null)
				{
					this.Graphics.Dispose();
					this.UnsubscribeGraphicsEvents();
					this.Graphics = null;
				}

				if(this.Window != null)
				{
					this.UnsubscribeWindowEvents();
					this.Window = null;
				}
			}
		}

		/// <summary>
		/// Subscribes to events on the window.
		/// </summary>
		private void SubscribeWindowEvents()
		{
			this.Window.Idle += this.Window_Idle;
			this.Window.Activate += this.Window_Activate;
			this.Window.Deactivate += this.Window_Deactivate;
			this.Window.Exiting += this.Window_Exiting;
		}

		/// <summary>
		/// Subscribes to events on the window.
		/// </summary>
		private void UnsubscribeWindowEvents()
		{
			this.Window.Idle -= this.Window_Idle;
			this.Window.Activate -= this.Window_Activate;
			this.Window.Deactivate -= this.Window_Deactivate;
			this.Window.Exiting -= this.Window_Exiting;
		}

		/// <summary>
		/// Subscribes to events on the graphics manager.
		/// </summary>
		public void SubscribeGraphicsEvents()
		{
			this.Graphics.FullscreenToggled += this.Graphics_FullscreenToggled;
		}

		/// <summary>
		/// Unsubscribes to events on the graphics manager.
		/// </summary>
		public void UnsubscribeGraphicsEvents()
		{
			this.Graphics.FullscreenToggled -= this.Graphics_FullscreenToggled;
		}

		/// <summary>
		/// Triggers the main loop for the game.
		/// </summary>
		public void Run()
		{
			this.Initialize();

			if(!this.Graphics.IsDeviceCreated)
				throw new InvalidOperationException("The Graphics Device must be created after the Initialize method has finished.");

			this.Window.Run();
		}

		/// <summary>
		/// Called when the window has idle time.
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
				if(this.Graphics.EnsureDeviceNotLost())
					this.Draw(this.gameTime);

				this.gameClock.ResetShouldDraw();
			}
		}
		
		/// <summary>
		/// Called when the window is activated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Activate(object sender, EventArgs e)
		{
			this.gameClock.Resume();
		}

		/// <summary>
		/// Called when the window is deactivated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Deactivate(object sender, EventArgs e)
		{
			this.gameClock.Pause();
		}

		/// <summary>
		/// Called when the window is exiting.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Exiting(object sender, EventArgs e)
		{
			this.OnExiting();
		}

		/// <summary>
		/// Called when the graphics device has switched to or from fullscreen.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Graphics_FullscreenToggled(object sender, EventArgs e)
		{
			this.OnToggleFullscreen();
		}

		/// <summary>
		/// Called when the game should initialize.
		/// </summary>
		protected virtual void Initialize()
		{
		}

		/// <summary>
		/// Called when the game should update.
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Update(GameTime gameTime)
		{
		}

		/// <summary>
		/// Called when the game should draw.
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Draw(GameTime gameTime)
		{
		}

		/// <summary>
		/// Triggers an exit request for the game.
		/// </summary>
		public void Exit()
		{
			this.Window.Exit();
		}

		/// <summary>
		/// Called when the game switches to or from fullscreen.
		/// </summary>
		protected virtual void OnToggleFullscreen()
		{
		}

		/// <summary>
		/// Called when the game is exiting.
		/// </summary>
		protected virtual void OnExiting()
		{
		}
	}
}
