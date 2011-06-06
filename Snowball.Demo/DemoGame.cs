using System;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Demo.Gameplay;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		float time = 0;
		int fps = 0;

		KeyboardDevice keyboard;
		MouseDevice mouse;

		Renderer renderer;

		GameEntityManager entities;
		
		Starfield starfield;
		Ship ship;
		float flameTimer = 0.0f;

		TextureFont font;
						
		public DemoGame()
			: base()
		{
			this.Window.Title = "Snow Engine Demo Game";
		}

		public override void Initialize()
		{
			base.Initialize();

			this.keyboard = new KeyboardDevice();
			this.Subsystems.Add(this.keyboard);

			this.mouse = new MouseDevice();
			this.Subsystems.Add(this.mouse);

			this.renderer = new Renderer(this.Graphics);

			this.entities = new GameEntityManager();
			this.entities.Initialize();

			this.starfield = new Starfield(this.renderer, this.Window.ClientWidth, this.Window.ClientHeight);
			this.entities.Add(this.starfield);

			this.ship = new Ship(this.Graphics, this.renderer, this.keyboard);
			this.entities.Add(this.ship);

			this.font = this.Graphics.LoadTextureFont("font.xml", null);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			this.entities.Update(gameTime);

			this.fps++;
			this.time += gameTime.ElapsedTotalSeconds;
			if(this.time >= 1.0f)
			{
				this.Window.Title = this.fps.ToString() + " FPS";
				this.fps = 0;
				this.time -= 1.0f;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			this.renderer.Begin();

			base.Draw(gameTime);

			this.entities.Draw(this.renderer);

			if(this.mouse.IsWithinClientArea)
			{
				Rectangle mouseRect = new Rectangle(this.mouse.Position.X, this.mouse.Position.Y, 10, 10);

				if(this.mouse.IsButtonDown(MouseButtons.Left))
					mouseRect.Width = 20;

				if(this.mouse.IsButtonDown(MouseButtons.Right))
					mouseRect.Height = 20;

				Color rectColor = Color.Red;
				if(this.keyboard.IsKeyDown(Keys.ControlKey))
					rectColor = Color.Green;
				else if(this.keyboard.IsKeyDown(Keys.AltKey))
					rectColor = Color.Blue;

				this.renderer.DrawFilledRectangle(mouseRect, rectColor);
			}

			this.renderer.DrawString(this.font, "Hello World!", Vector2.Zero, Color.White);
			
			this.renderer.End();
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
