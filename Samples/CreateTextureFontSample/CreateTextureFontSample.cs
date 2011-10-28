using System;
using Snowball.Graphics;

namespace Snowball.Demo
{
	public class CreateTextureFontSample : Game
	{
		Renderer renderer;
		TextureFont textureFont;

		public CreateTextureFontSample()
			: base()
		{
			this.Window.Title = "Snowball CreateTextureFont Sample";
		}

		protected override void Initialize()
		{
			this.Graphics.CreateDevice();
			this.renderer = new Renderer(this.Graphics);
			this.textureFont = new TextureFont(this.Graphics, "Segoe UI", 24, true);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			this.Graphics.Clear(Color.Black);
			this.Graphics.BeginDraw();

			this.renderer.Begin();
			this.renderer.DrawTexture(this.textureFont.Texture, new Vector2(10, 10), Color.White);
			this.renderer.DrawString(this.textureFont, "Hello World!", new Vector2(10, 410), Color.White);
			this.renderer.End();

			this.Graphics.EndDraw();
			this.Graphics.Present();
		}

		public static void Main()
		{
			using(CreateTextureFontSample sample = new CreateTextureFontSample())
				sample.Run();
		}
	}
}
