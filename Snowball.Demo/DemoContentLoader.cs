using System;
using Snowball.Content;
using Snowball.Graphics;

namespace Snowball.Demo
{
	public class DemoContentLoader : ContentLoader
	{
		public DemoContentLoader(IServiceProvider services)
			: base(services)
		{
			this.Register<Texture>("ConsoleBackground", new LoadTextureArgs()
			{
				FileName = "ConsoleBackground.png"
			});

			this.Register<SpriteSheet>("Ship", new LoadSpriteSheetArgs()
			{
				FileName = "Ship.png",
				ColorKey = Color.Magenta,
				FrameWidth = 80,
				FrameHeight = 80
			});

			this.Register<SpriteSheet>("ShipFlame", new LoadSpriteSheetArgs()
			{
				FileName = "ShipFlame.png",
				ColorKey = Color.Magenta,
				FrameWidth = 16,
				FrameHeight = 16
			});
		}
	}
}
