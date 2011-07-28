using System;
using Snowball.Graphics;

namespace Snowball.Demo
{
	public class CreateTextureFontSample : Game
	{
		GraphicsManager graphics;
		Renderer renderer;
		TextureFont textureFont;

		public CreateTextureFontSample()
			: base()
		{
			this.Window.Title = "Snowball CreateTextureFont Sample";
		}

		public override void Initialize()
		{
			this.graphics = new GraphicsManager();
			this.graphics.CreateDevice(this.Window);

			this.renderer = new Renderer(this.graphics);
			
			this.textureFont = this.graphics.CreateTextureFont("Segoe UI", 24, true);
		}
		
		public override void Draw(GameTime gameTime)
		{
			this.graphics.Clear(Color.Black);
			this.graphics.BeginDraw();
			this.renderer.Begin();
			this.renderer.DrawTexture(this.textureFont.Texture, new Vector2(10, 10), Color.White);
			this.renderer.DrawString(this.textureFont, "Hello World!", new Vector2(10, 410), Color.White);
			this.renderer.End();
			this.graphics.EndDraw();
			this.graphics.Present();
		}

		public static void Main()
		{
			using(CreateTextureFontSample sample = new CreateTextureFontSample())
				sample.Run();
		}
	}
}
