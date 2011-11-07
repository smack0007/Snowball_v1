using System;
using Snowball.Graphics;

namespace Snowball.Demo
{
	public class CreateTextureFontSample : Game
	{
		GraphicsDevice graphics;
		Renderer renderer;
		TextureFont textureFont;

		public CreateTextureFontSample()
			: base()
		{
			this.Window.Title = "Snowball CreateTextureFont Sample";

			this.graphics = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphics);
		}

		protected override void Initialize()
		{
			this.graphics.CreateDevice();
			this.renderer = new Renderer(this.graphics);
			this.textureFont = new TextureFont(this.graphics, "Segoe UI", 24, true);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			if(this.graphics.BeginDraw())
			{
				this.graphics.Clear(Color.Black);

				this.renderer.Begin();
				this.renderer.DrawTexture(this.textureFont.Texture, new Vector2(10, 10), Color.White);
				this.renderer.DrawString(this.textureFont, "Hello World!", new Vector2(10, 410), Color.White);
				this.renderer.End();

				this.graphics.EndDraw();
				this.graphics.Present();
			}
		}

		public static void Main()
		{
			using(CreateTextureFontSample sample = new CreateTextureFontSample())
				sample.Run();
		}
	}
}
