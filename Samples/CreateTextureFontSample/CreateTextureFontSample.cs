using System;
using Snowball.Graphics;

namespace Snowball.Demo
{
	public class CreateTextureFontSample : Game
	{
		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		TextureFont textureFont;

		public CreateTextureFontSample()
			: base()
		{
			this.Window.Title = "Snowball CreateTextureFont Sample";

			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);
		}
				
		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice();

			this.graphics = new GraphicsBatch(this.graphicsDevice);
			this.textureFont = new TextureFont(this.graphicsDevice, "Segoe UI", 24, true);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.Black);

				this.graphics.Begin();
				this.graphics.DrawTexture(this.textureFont.Texture, new Vector2(10, 10), Color.White);
				this.graphics.DrawString(this.textureFont, "Hello World!", new Vector2(10, 410), Color.White);
				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		public static void Main()
		{
			using(CreateTextureFontSample sample = new CreateTextureFontSample())
				sample.Run();
		}
	}
}
