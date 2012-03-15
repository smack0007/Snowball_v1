using System;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Storage;

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
		/// The graphics device for the game.
		/// </summary>
		public GraphicsDevice Graphics
		{
			get;
			private set;
		}

		/// <summary>
		/// The color to clear the screen with before each draw. 
		/// </summary>
		public Color BackgroundColor
		{
			get;
			set;
		}

		/// <summary>
		/// The content loader for the game.
		/// </summary>
		public ContentLoader ContentLoader
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
			: this(window, new FileSystemStorage())
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="storage">The storage system to use for loading content.</param>
		public Game(IStorage storage)
			: this(new GameWindow(), storage)
		{
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="window">The window to that hosts the game.</param>
		/// <param name="storage">The storage system to use for loading content.</param>
		public Game(IGameWindow window, IStorage storage)
		{			
			if (window == null)
				throw new ArgumentNullException("window");

			if (storage == null)
				throw new ArgumentNullException("storage");

			this.Services = new GameServicesContainer();
			
			this.Window = window;
			this.SubscribeWindowEvents();
			this.Services.AddService(typeof(IGameWindow), this.Window);

			this.Graphics = new GraphicsDevice(this.Window);
			this.BackgroundColor = Color.CornflowerBlue;
			this.SubscribeGraphicsDeviceEvents();
			this.Services.AddService(typeof(IGraphicsDevice), this.Graphics);
			
			this.ContentLoader = new ContentLoader(this.Services, storage);
			this.Services.AddService(typeof(IContentLoader), this.ContentLoader);

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
				if (this.Graphics != null)
				{
					this.UnsubscribeGraphicsDeviceEvents();
					this.Graphics.Dispose();
					this.Graphics = null;
				}

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
			this.Window.Idle += this.Window_Idle;
			this.Window.Activate += this.Window_Activate;
			this.Window.Deactivate += this.Window_Deactivate;
			this.Window.Exiting += this.Window_Exiting;
			this.Window.DialogOpen += this.Window_DialogOpen;
			this.Window.DialogClose += this.Window_DialogClose;
		}

		/// <summary>
		/// Subscribes to events on the Window.
		/// </summary>
		private void UnsubscribeWindowEvents()
		{
			this.Window.Idle -= this.Window_Idle;
			this.Window.Activate -= this.Window_Activate;
			this.Window.Deactivate -= this.Window_Deactivate;
			this.Window.Exiting -= this.Window_Exiting;
			this.Window.DialogOpen -= this.Window_DialogOpen;
			this.Window.DialogClose -= this.Window_DialogClose;
		}

		/// <summary>
		/// Subscribes to events on the GraphicsDevice.
		/// </summary>
		private void SubscribeGraphicsDeviceEvents()
		{
			this.Graphics.FullscreenToggled += this.GraphicsDevice_FullscreenToggled;
			this.Graphics.DeviceLost += this.GraphicsDevice_DeviceLost;
			this.Graphics.DeviceReset += this.GraphicsDevice_DeviceReset;
		}

		/// <summary>
		/// Unsubscribes to events on the GraphicsDevice.
		/// </summary>
		private void UnsubscribeGraphicsDeviceEvents()
		{
			this.Graphics.FullscreenToggled -= this.GraphicsDevice_FullscreenToggled;
			this.Graphics.DeviceLost -= this.GraphicsDevice_DeviceLost;
			this.Graphics.DeviceReset -= this.GraphicsDevice_DeviceReset;
		}

		/// <summary>
		/// Triggers the main loop for the game.
		/// </summary>
		public void Run()
		{
			this.DoInitialize();
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

			if (this.gameClock.ShouldUpdate)
			{
				this.Update(this.gameTime);
				this.gameClock.ResetShouldUpdate();
			}

			if (this.gameClock.ShouldDraw)
			{
				this.DoDraw(this.gameTime);
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
		/// Called when the window is opening a dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_DialogOpen(object sender, EventArgs e)
		{
			this.gameClock.Pause();
		}

		/// <summary>
		/// Called after the window has closed a dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_DialogClose(object sender, EventArgs e)
		{
			this.gameClock.Resume();
		}

		/// <summary>
		/// Called when fullscreen mode is toggled by the GraphicsDevice.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GraphicsDevice_FullscreenToggled(object sender, EventArgs e)
		{
			this.OnFullscreenToggled();
		}

		/// <summary>
		/// Called when the GraphicsDevice is lost.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GraphicsDevice_DeviceLost(object sender, EventArgs e)
		{
			this.UnloadContent();
		}

		/// <summary>
		/// Called when the GraphicsDevice is Reset.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GraphicsDevice_DeviceReset(object sender, EventArgs e)
		{
			this.LoadContent();
		}

		/// <summary>
		/// Called internally by the Game class to perform initialization.
		/// </summary>
		private void DoInitialize()
		{
			this.InitializeDevices();

			if (!this.Graphics.IsDeviceCreated)
				throw new InvalidOperationException("The GraphicsDevice must be created after InitializeDevices().");
						
			this.LoadContent();
			this.Initialize();
		}

		/// <summary>
		/// Called when the game should initialize devices such as graphics and sound.
		/// </summary>
		protected virtual void InitializeDevices()
		{
		}

		/// <summary>
		/// Called when the game should initialize.
		/// </summary>
		protected virtual void Initialize()
		{
		}

		/// <summary>
		/// Called when the game should load it's content.
		/// </summary>
		protected virtual void LoadContent()
		{
		}

		/// <summary>
		/// Called when the game should unload it's content.
		/// </summary>
		protected virtual void UnloadContent()
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
		/// Called internally by the Game class to perform drawing.
		/// </summary>
		private void DoDraw(GameTime gameTime)
		{
			this.Graphics.Clear(this.BackgroundColor);

			if (this.Graphics.BeginDraw())
			{				
				this.Draw(gameTime);

				this.Graphics.EndDraw();
				this.Graphics.Present();
			}
		}

		/// <summary>
		/// Called when the Game should draw.
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Draw(GameTime gameTime)
		{
		}

		/// <summary>
		/// Called when the Game transitions to or from fullscreen mode.
		/// </summary>
		protected virtual void OnFullscreenToggled()
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
		/// Called when the game is exiting.
		/// </summary>
		protected virtual void OnExiting()
		{
		}
	}
}
