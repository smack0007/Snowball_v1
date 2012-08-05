using System;
using Snowball;
using Snowball.Graphics;
using Snowball.Content;

namespace CustomGameWindowSample
{
	public class CustomGameWindowSample : Game
	{
		const string Text = "Hello World!";

		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		ContentLoader contentLoader;
		TextureFont font;
		Vector2 textPosition;

		public CustomGameWindowSample()
			: base(new CustomGameWindow())
		{
			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.contentLoader = new ContentLoader(this.Services);
		}

		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice(800, 600);

			this.graphics = new GraphicsBatch(this.graphicsDevice);
			this.font = this.contentLoader.Load<TextureFont>(new LoadTextureFontArgs()
			{
				FileName = "CustomGameWindowFont.xml"
			});

			Vector2 textSize = this.font.MeasureString(Text);
			this.textPosition = new Vector2(this.graphicsDevice.BackBufferWidth / 2 - textSize.X / 2, this.graphicsDevice.BackBufferHeight / 2 - textSize.Y / 2);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.Red);

				this.graphics.Begin();
				this.graphics.DrawString(this.font, Text, this.textPosition, Color.White);
				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		public static void Main()
		{
			using (CustomGameWindowSample game = new CustomGameWindowSample())
				game.Run();
		}
	}
}
