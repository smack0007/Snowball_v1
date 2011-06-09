using System;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Demo.Gameplay;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		public DemoGame()
			: base()
		{
			this.Window.Title = "Snowball Demo Game";
		}

		public override void Initialize()
		{
			base.Initialize();
									
			this.Screens.Add("Gameplay", new GameplayScreen(this.Graphics, this.Keyboard));
		}

		public override void Draw(GameTime gameTime)
		{			
			base.Draw(gameTime);
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
