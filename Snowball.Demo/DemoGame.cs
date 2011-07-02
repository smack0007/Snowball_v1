using System;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;
using Snowball.Demo.Gameplay;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		Renderer renderer;

		Starfield starfield;
		Ship ship;

		SoundEffect blasterSound;

		public DemoGame()
			: base(new DemoGameWindow())
		{
			this.Window.Title = "Snowball Demo Game";
			this.DesiredDisplayWidth = 800;
			this.DesiredDisplayHeight = 600;
		}

		public override void Initialize()
		{
			base.Initialize();

			this.renderer = new Renderer(this.Graphics);

			this.starfield = new Starfield(this.Graphics.DisplayWidth, this.Graphics.DisplayHeight);
			
			this.ship = new Ship(this.Graphics, this.Keyboard);

			this.blasterSound = this.Sound.LoadSoundEffect("blaster.wav");
		}

		public override void Update(GameTime gameTime)
		{
			this.starfield.Update(gameTime);
			this.ship.Update(gameTime);

			if(this.Keyboard.IsKeyPressed(Keys.S))
				this.blasterSound.Play();
		}

		public override void Draw(GameTime gameTime)
		{
			this.renderer.Begin();
			this.starfield.Draw(this.renderer);
			this.ship.Draw(this.renderer);
			this.renderer.End();
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
