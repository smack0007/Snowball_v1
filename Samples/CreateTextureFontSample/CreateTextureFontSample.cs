using System;
using Snowball.Graphics;

namespace Snowball.Demo
{
	public class CreateTextureFontSample : Game
	{
		GraphicsBatch graphics;
		TextureFont textureFont;

		public CreateTextureFontSample()
			: base()
		{
			this.Window.Title = "Snowball CreateTextureFont Sample";
			this.BackgroundColor = Color.Black;
		}
				
		protected override void Initialize()
		{
			this.Graphics.CreateDevice();

			this.graphics = new GraphicsBatch(this.Graphics);
			this.textureFont = new TextureFont(this.Graphics, "Segoe UI", 24, true);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			this.graphics.Begin();
			this.graphics.DrawTexture(this.textureFont.Texture, new Vector2(10, 10), Color.White);
			this.graphics.DrawString(this.textureFont, "Hello World!", new Vector2(10, 410), Color.White);
			this.graphics.End();
		}

		public static void Main()
		{
			using(CreateTextureFontSample sample = new CreateTextureFontSample())
				sample.Run();
		}
	}
}
