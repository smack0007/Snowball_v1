using System;
using Snowball.Content;

namespace Snowball.Demo
{
	public class DemoContentLoader : ContentLoader
	{
		public DemoContentLoader(IServiceProvider services)
			: base(services)
		{
			this.RegisterTexture("ConsoleBackground", "ConsoleBackground.png", null);
			this.RegisterSpriteSheet("Ship", "Ship.png", Color.Magenta, 80, 80, 0, 0);
			this.RegisterSpriteSheet("ShipFlame", "ShipFlame.png", Color.Magenta, 16, 16, 0, 0);
		}
	}
}
