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
				
		TextureFont font;
						
		public DemoGame()
			: base()
		{
			this.Window.Title = "Snow Engine Demo Game";
		}

		public override void Initialize()
		{
			base.Initialize();
									
			this.States.Add("Gameplay", new GameplayState(this.Graphics, this.Keyboard));

			this.font = this.Graphics.LoadTextureFont("font.xml", null);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			
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
			base.Draw(gameTime);
						
			if(this.Mouse.IsWithinClientArea)
			{
				Rectangle mouseRect = new Rectangle(this.Mouse.Position.X, this.Mouse.Position.Y, 10, 10);

				if(this.Mouse.IsButtonDown(MouseButtons.Left))
					mouseRect.Width = 20;

				if(this.Mouse.IsButtonDown(MouseButtons.Right))
					mouseRect.Height = 20;

				Color rectColor = Color.Red;
				if(this.Keyboard.IsKeyDown(Keys.ControlKey))
					rectColor = Color.Green;
				else if(this.Keyboard.IsKeyDown(Keys.AltKey))
					rectColor = Color.Blue;

				this.Renderer.DrawFilledRectangle(mouseRect, rectColor);
			}

			this.Renderer.DrawString(this.font, "Hello World!", Vector2.Zero, Color.White);
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
