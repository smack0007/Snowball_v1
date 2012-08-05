using System;
using Snowball.Content;
using Snowball.Demo.Gameplay;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;

		ContentLoader contentLoader;

		Keyboard keyboard;
		Mouse mouse;
		GamePad gamePad;
		
		SoundDevice soundDevice;
		
		GameConsole console;

		Starfield starfield;
		Ship ship;
		
		int fps;
		float fpsTime;

		public DemoGame()
			: base()
		{
			this.Window.Title = "Snowball Demo Game";
			
			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.keyboard = new Keyboard();
			this.Services.AddService(typeof(IKeyboard), this.keyboard);

			this.mouse = new Mouse(this.Window);
			this.Services.AddService(typeof(IMouse), this.mouse);

			this.gamePad = new GamePad(PlayerIndex.One);

			this.soundDevice = new SoundDevice();
			this.Services.AddService(typeof(ISoundDevice), this.soundDevice);
									
			this.starfield = new Starfield(this.graphicsDevice);

			this.ship = new Ship(this.graphicsDevice, this.keyboard, this.gamePad);

			this.console = new GameConsole(this.Window);
			this.console.InputEnabled = true;
			this.console.InputColor = Color.Blue;
			this.console.InputReceived += (s, e) => { this.console.WriteLine(e.Text); };
			this.console.IsVisibleChanged += (s, e) => { this.console.WriteLine("Console toggled."); };

			this.contentLoader = new ContentLoader(this.Services);

			this.RegisterContent();
		}

		private void RegisterContent()
		{			
			this.contentLoader.Register<Texture>("ConsoleBackground", new LoadTextureArgs()
			{
				FileName = "ConsoleBackground.png"
			});

			this.contentLoader.Register<SpriteSheet>("Ship", new LoadSpriteSheetArgs()
			{
				FileName = "Ship.png",
				ColorKey = Color.Magenta,
				FrameWidth = 80,
				FrameHeight = 80
			});

			this.contentLoader.Register<SpriteSheet>("ShipFlame", new LoadSpriteSheetArgs()
			{
				FileName = "ShipFlame.png",
				ColorKey = Color.Magenta,
				FrameWidth = 16,
				FrameHeight = 16
			});

			this.contentLoader.Register<SoundEffect>("Blaster", new LoadSoundEffectArgs()
			{
				FileName = "blaster.wav"
			});
		}

		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice(800, 600, false);
			
			this.soundDevice.CreateDevice();

			this.console.Font = new BasicTextureFont(this.graphicsDevice);
			this.console.BackgroundTexture = this.contentLoader.Load<Texture>("ConsoleBackground");

			this.ship.LoadContent(this.contentLoader);
		
			this.graphics = new GraphicsBatch(this.graphicsDevice);

			this.starfield.Initialize();
			this.ship.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			this.keyboard.Update();
			this.mouse.Update(gameTime);
			this.gamePad.Update();

			if (this.keyboard.IsKeyPressed(Keys.Escape) || this.gamePad.Back)
				this.Exit();

			if (!this.console.IsVisible)
			{
				this.starfield.Update(gameTime);
				this.ship.Update(gameTime);
			}

			if (this.mouse.IsButtonDoubleClicked(MouseButtons.Left))
				this.console.WriteLine("Double Click!");

			this.console.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.Black);

				this.fps++;
				this.fpsTime += gameTime.ElapsedTotalSeconds;
				if (this.fpsTime >= 1.0f)
				{
					this.console.WriteLine(this.fps.ToString() + " FPS", Color.Blue);
					this.fps = 0;
					this.fpsTime -= 1.0f;
				}

				this.graphics.Begin();

				this.starfield.Draw(this.graphics);

				this.ship.Draw(this.graphics);

				this.console.Draw(this.graphics);

				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		protected override void Shutdown()
		{
			this.graphicsDevice.Dispose();
			this.soundDevice.Dispose();
		}

		public static void Main()
		{
			using (DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
