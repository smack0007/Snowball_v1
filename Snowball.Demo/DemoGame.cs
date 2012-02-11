using System;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;
using Snowball.Demo.Gameplay;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		Renderer renderer;
		DemoContentLoader content;
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
									
			this.content = new DemoContentLoader(this.Services);
			this.Services.AddService(typeof(IContentLoader), this.content);

			this.keyboard = new KeyboardDevice();
			this.Services.AddService(typeof(IKeyboardDevice), this.keyboard);

			this.gamePad = new GamePadDevice(PlayerIndex.One);

			this.sound = new SoundDevice();
			this.Services.AddService(typeof(ISoundDevice), this.sound);

			this.console = new GameConsole(this.Services);
			
			this.ship = new Ship(this.Services, this.gamePad);
		}

		protected override void Initialize()
		{
			this.GraphicsDevice.CreateDevice(800, 600);
			
			this.sound.CreateDevice();

			this.console.Initialize();
			this.console.InputColor = Color.Blue;
			this.console.CommandEntered += (s, e) =>
			{
			    this.console.WriteLine(e.Command);
			};

			this.starfield = new Starfield(this.GraphicsDevice.DisplayWidth, this.GraphicsDevice.DisplayHeight);

			this.ship.Initialize();

			this.renderer = new Renderer(this.GraphicsDevice);
		}

		protected override void LoadContent()
		{
			this.console.Font = new TextureFont(this.GraphicsDevice, "Arial", 12, true);
			this.console.BackgroundTexture = this.content.Load<Texture>("ConsoleBackground");

			this.renderTarget = new RenderTarget(this.GraphicsDevice, 200, 200);
			if (this.GraphicsDevice.BeginDraw(this.renderTarget))
			{
				this.GraphicsDevice.Clear(Color.Blue);
				this.renderer.Begin();
				this.renderer.DrawLine(new Vector2(0, 0), new Vector2(200, 200), Color.Red);
				this.renderer.End();
				this.GraphicsDevice.EndDraw();
			}
		}

		protected override void UnloadContent()
		{
			this.console.Font.Dispose();
			this.console.BackgroundTexture.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
			this.keyboard.Update(gameTime);
			this.gamePad.Update(gameTime);

			if (this.keyboard.IsKeyPressed(Keys.Escape) || this.gamePad.Back)
				this.Exit();

			if (this.keyboard.IsKeyPressed(Keys.F12))
				this.GraphicsDevice.ToggleFullscreen();

			if (!this.console.IsVisible)
			{
				this.starfield.Update(gameTime);
				this.ship.Update(gameTime);
			}

			this.console.Update(gameTime);
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
			
			this.GraphicsDevice.Clear(Color.Black);
			this.renderer.Begin();

			this.starfield.Draw(this.renderer);
			this.ship.Draw(this.renderer);
			this.renderer.DrawRenderTarget(this.renderTarget, Vector2.Zero, Color.White);

			this.console.Draw(this.renderer);

			this.renderer.End();
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
