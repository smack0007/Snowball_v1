using System;
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

		int desiredDisplayWidth;
		int desiredDisplayHeight;
		
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
		/// The display width which will be used when the graphics device is created by the GraphicsManager.
		/// </summary>
		public int DesiredDisplayWidth
		{
			get { return this.desiredDisplayWidth; }

			set
			{
				if(this.Graphics.IsDeviceCreated)
					throw new InvalidOperationException("The graphics device has already been created.");

				this.desiredDisplayWidth = value;
			}
		}

		/// <summary>
		/// The display height which will be used when the graphics device is created by the GraphicsManager.
		/// </summary>
		public int DesiredDisplayHeight
		{
			get { return this.desiredDisplayHeight; }

			set
			{
				if(this.Graphics.IsDeviceCreated)
					throw new InvalidOperationException("The graphics device has already been created.");

				this.desiredDisplayHeight = value;
			}
		}
										
		/// <summary>
		/// The keyboard input device.
		/// </summary>
		public KeyboardDevice Keyboard
		{
			get;
			private set;
		}

		/// <summary>
		/// The mouse input device.
		/// </summary>
		public MouseDevice Mouse
		{
			get;
			private set;
		}

		/// <summary>
		/// The sound manager.
		/// </summary>
		public SoundManager Sound
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
			this.desiredDisplayWidth = this.Window.ClientWidth;
			this.desiredDisplayHeight = this.Window.ClientHeight;

			this.gameClock = new GameClock();
			this.gameTime = new GameTime();

			this.Graphics = new GraphicsManager();
												
			this.Keyboard = new KeyboardDevice();
			this.Mouse = new MouseDevice(this.Window);

			this.Sound = new SoundManager();
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
				if(this.Graphics != null)
				{
					this.Graphics.Dispose();
					this.Graphics = null;
				}

				if(this.Window != null)
				{
					this.UnsubscribeWindowEvents();
					this.Window = null;
				}

				// Cleans up SlimDX COM handles
				foreach(var item in SlimDX.ObjectTable.Objects)
					if(!item.Disposed)
						item.Dispose();
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
		/// Triggers the main loop for the game.
		/// </summary>
		public void Run()
		{
			this.InitializeGraphics();
			this.InitializeSound();
			this.Initialize();
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
				this.Keyboard.Update(this.gameTime);
				this.Mouse.Update(this.gameTime);
				this.Update(this.gameTime);
				this.gameClock.ResetShouldUpdate();
			}

			if(this.gameClock.ShouldDraw)
			{
				if(this.Graphics != null && this.Graphics.IsDeviceCreated)
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
			this.OnExiting(EventArgs.Empty);
		}

		/// <summary>
		/// Initializes the graphics device.
		/// </summary>
		protected virtual void InitializeGraphics()
		{
			this.Graphics.CreateDevice(this.Window, this.desiredDisplayWidth, this.desiredDisplayHeight);
		}

		/// <summary>
		/// Initializes the sound device.
		/// </summary>
		protected virtual void InitializeSound()
		{
			this.Sound.CreateDevice();
		}

		/// <summary>
		/// Called when the game should initialize.
		/// </summary>
		public virtual void Initialize()
		{
		}

		/// <summary>
		/// Called when the game should update.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
		}

		/// <summary>
		/// Called when the game should draw.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Draw(GameTime gameTime)
		{
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
