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
		GraphicsDevice graphics;
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

			this.graphics = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphics);
						
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
			base.Initialize();

			this.graphics.CreateDevice(800, 600);
			this.graphics.FullscreenToggled += (s, e) => { this.DrawRenderTarget(); };
			
			this.sound.CreateDevice();

			this.console.Initialize();
			this.console.Font = new TextureFont(this.graphics, "Arial", 12, true);
			this.console.BackgroundTexture = this.content.Load<Texture>("ConsoleBackground");
			this.console.InputColor = Color.Blue;
			this.console.CommandEntered += (s, e) =>
			{
			    this.console.WriteLine(e.Command);
			};

			this.starfield = new Starfield(this.graphics.DisplayWidth, this.graphics.DisplayHeight);

			this.ship.Initialize();

			this.renderer = new Renderer(this.graphics);

			this.renderTarget = this.graphics.CreateRenderTarget(200, 200);
			this.DrawRenderTarget();
		}
		
		private void DrawRenderTarget()
		{
			this.graphics.SetRenderTarget(this.renderTarget);
			
			if(this.graphics.BeginDraw())
			{
				this.graphics.Clear(Color.Blue);
				this.renderer.Begin();
				this.renderer.DrawLine(new Vector2(0, 0), new Vector2(200, 200), Color.Red);
				this.renderer.End();
				this.graphics.EndDraw();
				this.graphics.SetRenderTarget(null);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			this.keyboard.Update(gameTime);
			this.gamePad.Update(gameTime);

			if(this.keyboard.IsKeyPressed(Keys.Escape) || this.gamePad.Back)
				this.Exit();

			if(this.keyboard.IsKeyPressed(Keys.F12))
				this.graphics.ToggleFullscreen();

			if(!this.console.IsVisible)
			{
				this.starfield.Update(gameTime);
				this.ship.Update(gameTime);
			}

			this.console.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{			
			if(this.graphics.BeginDraw())
			{
				this.graphics.Clear(Color.Black);
				this.renderer.Begin();

				this.starfield.Draw(this.renderer);
				this.ship.Draw(this.renderer);
				//this.renderer.DrawRenderTarget(this.renderTarget, Vector2.Zero, Color.White);

				this.console.Draw(this.renderer);

				this.renderer.End();
				this.graphics.EndDraw();
				this.graphics.Present();
			}
			
			this.fps++;
			this.fpsTime += gameTime.ElapsedTotalSeconds;
			if(this.fpsTime >= 1.0f)
			{
			    this.console.WriteLine(this.fps.ToString() + " FPS", Color.Green);
			    this.fps = 0;
			    this.fpsTime -= 1.0f;
			}
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
