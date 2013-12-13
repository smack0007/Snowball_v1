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
        GameWindow gameWindow;
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
		public IGameWindow Window
		{
            get { return this.gameWindow; }
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
			
			this.gameWindow = new GameWindow();
			this.SubscribeGameWindowEvents();
			this.Services.AddService(typeof(IGameWindow), this.Window);
												
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
                if (this.gameWindow != null)
				{
					this.UnsubscribeGameWindowEvents();
                    this.gameWindow.Dispose();
                    this.gameWindow = null;
				}
			}
		}

		/// <summary>
		/// Subscribes to events on the Window.
		/// </summary>
		private void SubscribeGameWindowEvents()
		{
            this.gameWindow.Tick += this.Window_Idle;
            this.gameWindow.Resume += this.GameWindow_Resume;
            this.gameWindow.Pause += this.GameWindow_Pause;
            this.gameWindow.Closing += this.GameWindow_Close;
            this.gameWindow.Exiting += this.GameWindow_Exiting;
		}

		/// <summary>
		/// Subscribes to events on the Window.
		/// </summary>
		private void UnsubscribeGameWindowEvents()
		{
            this.gameWindow.Tick -= this.Window_Idle;
            this.gameWindow.Resume -= this.GameWindow_Resume;
            this.gameWindow.Pause -= this.GameWindow_Pause;
            this.gameWindow.Closing -= this.GameWindow_Close;
            this.gameWindow.Exiting -= this.GameWindow_Exiting;
		}
				
		/// <summary>
		/// Triggers the main loop for the game.
		/// </summary>
		public void Run()
		{
			this.Initialize();

            this.gameWindow.Run();

			this.Shutdown();
		}

		/// <summary>
		/// Called when the window has idle time.
		/// </summary>
		private void Window_Idle(object sender, EventArgs e)
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
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameWindow_Resume(object sender, EventArgs e)
		{
			this.gameClock.Resume();
		}

		/// <summary>
		/// Called when the window is deactivated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameWindow_Pause(object sender, EventArgs e)
		{
			this.gameClock.Pause();
		}

        private void GameWindow_Close(object sender, CancelEventArgs e)
        {

        }

		/// <summary>
		/// Called when the window is exiting.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameWindow_Exiting(object sender, EventArgs e)
		{
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

		/// <summary>
		/// Triggers an exit request for the game.
		/// </summary>
		public void Exit()
		{
            this.gameWindow.Exit();
		}
	}
}
