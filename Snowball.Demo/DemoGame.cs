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
		KeyboardDevice keyboard;
		GamePadDevice gamePad;
		SoundDevice sound;
		GameConsole console;

		Starfield starfield;
		Ship ship;
				
		RenderTarget renderTarget;

		int fps;
		float fpsTime;

		public DemoGame()
			: base()
		{
			this.Window.Title = "Snowball Demo Game";
			
			this.keyboard = new KeyboardDevice();
			this.Services.AddService(typeof(IKeyboardDevice), this.keyboard);

			this.gamePad = new GamePadDevice(PlayerIndex.One);

			this.sound = new SoundDevice();
			this.Services.AddService(typeof(ISoundDevice), this.sound);
									
			this.starfield = new Starfield(this.Graphics);
			this.Components.AddComponent(this.starfield);

			this.ship = new Ship(this.Graphics, this.keyboard, this.gamePad);
			this.Components.AddComponent(this.ship);

			this.console = new GameConsole(this.Window, this.keyboard);
			this.console.InputColor = Color.Blue;
			this.console.CommandEntered += (s, e) =>
			{
				this.console.WriteLine(e.Command);
			};

			this.RegisterContent();
		}

		private void RegisterContent()
		{
			this.ContentLoader.Register<Texture>("ConsoleBackground", new LoadTextureArgs()
			{
				FileName = "ConsoleBackground.png"
			});

			this.ContentLoader.Register<SpriteSheet>("Ship", new LoadSpriteSheetArgs()
			{
				FileName = "Ship.png",
				ColorKey = Color.Magenta,
				FrameWidth = 80,
				FrameHeight = 80
			});

			this.ContentLoader.Register<SpriteSheet>("ShipFlame", new LoadSpriteSheetArgs()
			{
				FileName = "ShipFlame.png",
				ColorKey = Color.Magenta,
				FrameWidth = 16,
				FrameHeight = 16
			});

			this.ContentLoader.Register<SoundEffect>("Blaster", new LoadSoundEffectArgs()
			{
				FileName = "blaster.wav"
			});
		}

		protected override void InitializeDevices()
		{
			this.Graphics.CreateDevice(800, 600);
			
			this.sound.CreateDevice();
		}
				
		protected override void LoadContent()
		{
			base.LoadContent();

			this.console.Font = new TextureFont(this.Graphics, "Arial", 12, true);
			this.console.BackgroundTexture = this.ContentLoader.Load<Texture>("ConsoleBackground");

			this.renderTarget = new RenderTarget(this.Graphics, 200, 200);
			if (this.Graphics.BeginDraw(this.renderTarget))
			{
				this.Graphics.Clear(Color.Blue);
				this.Renderer.Begin();
				this.Renderer.DrawLine(new Vector2(0, 0), new Vector2(200, 200), Color.Red);
				this.Renderer.End();
				this.Graphics.EndDraw();
			}
		}

		protected override void UnloadContent()
		{
			base.UnloadContent();

			this.console.Font.Dispose();
			this.console.BackgroundTexture.Dispose();
		}

		protected override void Initialize()
		{
			this.console.Initialize();

			base.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			this.keyboard.Update(gameTime);
			this.gamePad.Update(gameTime);

			if (this.keyboard.IsKeyPressed(Keys.Escape) || this.gamePad.Back)
				this.Exit();

			if (this.keyboard.IsKeyPressed(Keys.F12))
				this.Graphics.ToggleFullscreen();

			if (!this.console.IsVisible)
			{
				base.Update(gameTime);
			}

			this.console.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.Graphics.Clear(Color.Black);

			this.fps++;
			this.fpsTime += gameTime.ElapsedTotalSeconds;
			if (this.fpsTime >= 1.0f)
			{
				this.console.WriteLine(this.fps.ToString() + " FPS", Color.Green);
				this.fps = 0;
				this.fpsTime -= 1.0f;
			}

			base.Draw(gameTime);
			
			this.Renderer.DrawRenderTarget(this.renderTarget, Vector2.Zero, Color.White);

			this.console.Draw(this.Renderer);
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
