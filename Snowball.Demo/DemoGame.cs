using System;
using Snowball.Content;
using Snowball.Demo.Gameplay;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;
using System.Windows.Forms;

using Keys = Snowball.Input.Keys;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		Renderer renderer;
		Keyboard keyboard;
		GamePad gamePad;
		SoundDevice sound;
		
		GameConsole console;

		Starfield starfield;
		Ship ship;
				
		//RenderTarget renderTarget;

		int fps;
		float fpsTime;

		public DemoGame()
			: base()
		{
			this.Window.Title = "Snowball Demo Game";
			this.BackgroundColor = Color.Black;

			this.keyboard = new Keyboard();
			this.Services.AddService(typeof(IKeyboard), this.keyboard);

			this.gamePad = new GamePad(PlayerIndex.One);

			this.sound = new SoundDevice();
			this.Services.AddService(typeof(ISoundDevice), this.sound);
									
			this.starfield = new Starfield(this.Graphics);

			this.ship = new Ship(this.Graphics, this.keyboard, this.gamePad);

			this.console = new GameConsole(this.Window);
			this.console.InputColor = Color.Blue;
			this.console.CommandEntered += (s, e) =>
			{
				this.console.WriteLine(e.Command);
			};

			this.RegisterContent();
		}

		private void RegisterContent()
		{
			this.ContentLoader.Register<TextureFont>("ConsoleFont", new LoadTextureFontArgs()
			{
				LoadType = ContentLoadType.Construct,
				FontName = "Arial",
				FontSize = 12,
				Antialias = true
			});

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
			this.console.Font = this.ContentLoader.Load<TextureFont>("ConsoleFont");
			this.console.BackgroundTexture = this.ContentLoader.Load<Texture>("ConsoleBackground");

			this.ship.LoadContent(this.ContentLoader);
		}

		protected override void UnloadContent()
		{
			this.console.Font.Dispose();
			this.console.BackgroundTexture.Dispose();
		}

		protected override void Initialize()
		{
			this.renderer = new Renderer(this.Graphics);

			//this.renderTarget = new RenderTarget(this.Graphics, 200, 200);
			//if (this.Graphics.BeginDraw(this.renderTarget))
			//{
			//    this.Graphics.Clear(Color.Blue);
			//    this.renderer.Begin();
			//    this.renderer.DrawLine(new Vector2(0, 0), new Vector2(200, 200), Color.Red);
			//    this.renderer.End();
			//    this.Graphics.EndDraw();
			//}

			this.starfield.Initialize();
			this.ship.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			this.keyboard.Update();
			this.gamePad.Update();

			if (this.keyboard.IsKeyPressed(Keys.Escape) || this.gamePad.Back)
				this.Exit();

			if (this.keyboard.IsKeyPressed(Keys.F12))
				this.Graphics.ToggleFullscreen();

			if (!this.console.IsVisible)
			{
				this.starfield.Update(gameTime);
				this.ship.Update(gameTime);
			}

			this.console.Update(gameTime, this.keyboard);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.fps++;
			this.fpsTime += gameTime.ElapsedTotalSeconds;
			if (this.fpsTime >= 1.0f)
			{
				this.console.WriteLine(this.fps.ToString() + " FPS", Color.Green);
				this.fps = 0;
				this.fpsTime -= 1.0f;
			}

			this.renderer.Begin();

			this.starfield.Draw(this.renderer);
			this.ship.Draw(this.renderer);
			
			//this.renderer.DrawRenderTarget(this.renderTarget, Vector2.Zero, Color.White);

			this.console.Draw(this.renderer);

			this.renderer.End();
		}

		public static void Main()
		{
			using (DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
