using System;
using Snowball.Content;
using Snowball.Graphics;

namespace Snowball
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
		/// Constructor.
		/// </summary>
		public Game()
			: this(new GameWindow())
		{
		}
				
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="window">The window to that hosts the game.</param>
		public Game(IGameWindow window)
		{			
			if (window == null)
				throw new ArgumentNullException("window");
						
			this.Services = new GameServicesContainer();
			
			this.Window = window;
			this.SubscribeWindowEvents();
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
				if (this.Window != null)
				{
					this.UnsubscribeWindowEvents();
					this.Window = null;
				}
			}
		}

		/// <summary>
		/// Subscribes to events on the Window.
		/// </summary>
		private void SubscribeWindowEvents()
		{
			this.Window.Tick += this.Window_Idle;
			this.Window.Resume += this.Window_Resume;
			this.Window.Pause += this.Window_Pause;
			this.Window.Exiting += this.Window_Exiting;
		}

		/// <summary>
		/// Subscribes to events on the Window.
		/// </summary>
		private void UnsubscribeWindowEvents()
		{
			this.Window.Tick -= this.Window_Idle;
			this.Window.Resume -= this.Window_Resume;
			this.Window.Pause -= this.Window_Pause;
			this.Window.Exiting -= this.Window_Exiting;
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
		private void Window_Resume(object sender, EventArgs e)
		{
			this.gameClock.Resume();
		}

		/// <summary>
		/// Called when the window is deactivated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Pause(object sender, EventArgs e)
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
			this.Window.Exit();
		}
	}
}
