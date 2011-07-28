using System;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;
using Snowball.Demo.Gameplay;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		GraphicsManager graphics;
		Renderer renderer;
		KeyboardDevice keyboard;
		GameConsole console;

		Starfield starfield;
		Ship ship;
				
		RenderTarget renderTarget;

		int fps;
		float fpsTime;

		public DemoGame()
			: base()
			//: base(new DemoGameWindow())
		{
			this.Window.Title = "Snowball Demo Game";
		}

		public override void Initialize()
		{
			base.Initialize();

			this.graphics = new GraphicsManager();
			this.graphics.CreateDevice(this.Window, 800, 600);

			this.renderer = new Renderer(this.graphics);

			this.keyboard = new KeyboardDevice();

			this.console = new GameConsole(this.Window, this.keyboard, this.graphics.CreateTextureFont("Segoe", 16, true));
			this.console.BackgroundTexture = this.graphics.LoadTexture("ConsoleBackground.png", null);
			this.console.InputColor = Color.Blue;
			this.console.CommandEntered += (s, e) =>
			{
				this.console.WriteLine(e.Command);
			};

			this.starfield = new Starfield(this.graphics.DisplayWidth, this.graphics.DisplayHeight);
			
			this.ship = new Ship(this.graphics, this.keyboard);
						
			this.renderTarget = this.graphics.CreateRenderTarget(200, 200);
			this.graphics.SetRenderTarget(this.renderTarget);
			this.graphics.BeginDraw();
			this.graphics.Clear(Color.Blue);
			this.renderer.Begin();
			this.renderer.DrawLine(new Vector2(0, 0), new Vector2(200, 200), Color.Red);
			this.renderer.End();
			this.graphics.EndDraw();
			this.graphics.SetRenderTarget(null);
		}

		public override void Update(GameTime gameTime)
		{
			this.keyboard.Update(gameTime);
			this.starfield.Update(gameTime);
			this.ship.Update(gameTime);
			this.console.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			this.graphics.Clear(Color.Black);
			this.graphics.BeginDraw();
			this.renderer.Begin();
			
			this.starfield.Draw(this.renderer);
			this.ship.Draw(this.renderer);
			this.renderer.DrawTexture(this.renderTarget, Vector2.Zero, Color.White);

			this.console.Draw(this.renderer);

			this.renderer.End();
			this.graphics.EndDraw();
			this.graphics.Present();
			
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
