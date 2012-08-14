using System;
using Snowball;
using Snowball.UI;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Storage;

namespace Snowball.Demo.UI
{
	public class UIDemoGame : Game
	{
		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		
		UIContentLoader uiContentLoader;
		UIRoot ui;

		public UIDemoGame()
			: base()
		{
			this.Window.Title = "Snowball UI Demo";

			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.uiContentLoader = new UIContentLoader(this.Services);
			this.Services.AddService(typeof(IUIContentLoader), this.uiContentLoader);
			
			this.ui = new UIRoot();
			this.ui.Controls.Add(new Label() { Text = "Hello World!" });
			this.ui.Controls.Add(new Button() { X = 100, Y = 100, Width = 100, Height = 24 });
		}

		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice(800, 600);

			this.graphics = new GraphicsBatch(this.graphicsDevice);

			this.ui.Initialize(this.Services);
		}

		protected override void Update(GameTime gameTime)
		{
			this.ui.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.CornflowerBlue);

				this.graphics.Begin();
				this.ui.Draw(this.graphics);
				this.graphics.End();

				this.graphicsDevice.EndDraw();
				this.graphicsDevice.Present();
			}
		}
		
		public static void Main()
		{
			using (UIDemoGame game = new UIDemoGame())
				game.Run();
		}
	}
}
