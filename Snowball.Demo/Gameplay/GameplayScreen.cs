using System;
using Snowball;
using Snowball.Graphics;
using Snowball.Input;

namespace Snowball.Demo.Gameplay
{
	public class GameplayScreen : GameScreen
	{
		IGameWindow window;
		IGraphicsManager graphics;
		IKeyboardDevice keyboard;
		
		Starfield starfield;
		Ship ship;

		public GameplayScreen(IGraphicsManager graphics, IKeyboardDevice keyboard)
			: base()
		{
			if(graphics == null)
				throw new ArgumentNullException("graphics");

			if(keyboard == null)
				throw new ArgumentNullException("keyboard");
			
			this.graphics = graphics;
			this.keyboard = keyboard;
		}

		public override void Initialize()
		{
			base.Initialize();

			this.starfield = new Starfield(this.graphics.DisplayWidth, this.graphics.DisplayHeight);
			this.Entities.Add(this.starfield);

			this.ship = new Ship(this.graphics, this.keyboard);
			this.Entities.Add(this.ship);
		}
	}
}
