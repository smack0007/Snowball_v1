using System;
using Snowball;
using Snowball.UI;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Storage;
using Snowball.Input;

namespace Snowball.Demo.UI
{
	public class UIDemoGame : Game
	{
		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;

		Mouse mouse;

		UIContentLoader uiContentLoader;
		UIController uiController;

		public UIDemoGame()
			: base()
		{
			this.Window.Title = "Snowball UI Demo";

			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.mouse = new Mouse(this.Window);
			this.Services.AddService(typeof(IMouse), this.mouse);

			this.uiContentLoader = new UIContentLoader(this.Services);
			this.Services.AddService(typeof(IUIContentLoader), this.uiContentLoader);
			
			this.uiController = new UIController();
			this.uiController.Controls.Add(new Label() { Text = "Hello World!" });
			this.uiController.Controls.Add(new Button() { X = 100, Y = 100, Width = 100, Height = 24, Text = "Click me!" });
		}

		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice(800, 600);

			this.graphics = new GraphicsBatch(this.graphicsDevice);

			this.uiController.Initialize(this.Services);
		}

		protected override void Update(GameTime gameTime)
		{
			this.mouse.Update(gameTime);
			this.uiController.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.CornflowerBlue);

				this.graphics.Begin();
				this.uiController.Draw(this.graphics);
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
