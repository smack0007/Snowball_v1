using System;
using Snowball.Content;
using Snowball.Graphics;
using System.ComponentModel;

namespace Snowball.GameFramework
{
	/// <summary>
	/// Base class for games.
	/// </summary>
	public abstract class Game : IDisposable
	{
		GameClock gameClock;
		GameTime gameTime;

		public int DesiredUpdatesPerSecond
		{
			get { return this.gameClock.DesiredUpdatesPerSecond; }
			set { this.gameClock.DesiredUpdatesPerSecond = value; }
		}

		public int DesiredDrawsPerSecond
		{
			get { return this.gameClock.DesiredDrawsPerSecond; }
			set { this.gameClock.DesiredDrawsPerSecond = value; }
		}
	
		/// <summary>
		/// The window the game is running in.
		/// </summary>
		public GameWindow Window
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
		/// Constructor.
		/// </summary>
		public Game()
		{						
			this.Services = new GameServicesContainer();
			
			this.Window = new GameWindow(this);
			this.Services.AddService(typeof(GameWindow), this.Window);
												
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
			if (disposing)
			{
                if (this.Window != null)
				{					
                    this.Window.Dispose();
                    this.Window = null;
				}
			}
		}
						
		/// <summary>
		/// Triggers the main loop for the game.
		/// </summary>
		public void Run()
		{
			this.Initialize();

            this.Window.Run();

			this.Shutdown();
		}

		/// <summary>
		/// Triggers an exit request for the game.
		/// </summary>
		public void Exit()
		{
			this.Window.Exit();
		}

		/// <summary>
		/// Called when the window has idle time.
		/// </summary>
		internal void Tick()
		{
			this.gameClock.Tick();

			this.gameTime.ElapsedTime = this.gameClock.ElapsedTimeSinceUpdate;
			this.gameTime.TotalTime += this.gameClock.ElapsedTimeSinceTick;

			if (this.gameClock.ShouldUpdate)
			{
				this.Update(this.gameTime);
				this.gameClock.ResetShouldUpdate();
			}

			if (this.gameClock.ShouldDraw)
			{
				this.Draw(this.gameTime);
				this.gameClock.ResetShouldDraw();
			}
		}
		
		/// <summary>
		/// Called when the window is activated.
		/// </summary>
		internal void Resume()
		{
			this.gameClock.Resume();
		}

		/// <summary>
		/// Called when the window is deactivated.
		/// </summary>
		internal void Pause()
		{
			this.gameClock.Pause();
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
		/// Called when the Game should draw.
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Draw(GameTime gameTime)
		{
		}

		/// <summary>
		/// Called when the Game shuts down.
		/// </summary>
		protected virtual void Shutdown()
		{
		}
	}
}
