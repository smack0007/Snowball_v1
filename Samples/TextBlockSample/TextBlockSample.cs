using System;
using Snowball;
using Snowball.Graphics;
using Snowball.Content;

namespace TextBlockSample
{
	public class TextBlockSample : Game
	{
		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		
		ContentLoader contentLoader;

		TextureFont font;

		TextBlock topLeftTextBlock;
		TextBlock topCenterTextBlock;
		TextBlock topRightTextBlock;

		TextBlock middleLeftTextBlock;
		TextBlock middleCenterTextBlock;
		TextBlock middleRightTextBlock;

		TextBlock bottomLeftTextBlock;
		TextBlock bottomCenterTextBlock;
		TextBlock bottomRightTextBlock;

		public TextBlockSample()
			: base()
		{
			this.Window.Title = "TextBlock Sample";

			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.contentLoader = new ContentLoader(this.Services);
			this.Services.AddService(typeof(IContentLoader), this.contentLoader);
		}

		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice();

			this.graphics = new GraphicsBatch(this.graphicsDevice);

			this.font = this.contentLoader.Load<TextureFont>(new LoadTextureFontArgs()
			{
				FileName = "TextBlockFont.xml"
			});

			Size textBlockSize = new Size(this.graphicsDevice.BackBufferWidth / 3, this.graphicsDevice.BackBufferHeight / 3);

			this.topLeftTextBlock = new TextBlock(this.font, "Top\nLeft\nText\nBlock", textBlockSize, TextAlignment.TopLeft);
			this.topCenterTextBlock = new TextBlock(this.font, "Top\nCenter\nText\nBlock", textBlockSize, TextAlignment.TopCenter);
			this.topRightTextBlock = new TextBlock(this.font, "Top\nRight\nText\nBlock", textBlockSize, TextAlignment.TopRight);

			this.middleLeftTextBlock = new TextBlock(this.font, "Middle\nLeft\nText\nBlock", textBlockSize, TextAlignment.MiddleLeft);
			this.middleCenterTextBlock = new TextBlock(this.font, "Middle\nCenter\nText\nBlock", textBlockSize, TextAlignment.MiddleCenter);
			this.middleRightTextBlock = new TextBlock(this.font, "Middle\nRight\nText\nBlock", textBlockSize, TextAlignment.MiddleRight);

			this.bottomLeftTextBlock = new TextBlock(this.font, "Bottom\nLeft\nText\nBlock", textBlockSize, TextAlignment.BottomLeft);
			this.bottomCenterTextBlock = new TextBlock(this.font, "Bottom\nCenter\nText\nBlock", textBlockSize, TextAlignment.BottomCenter);
			this.bottomRightTextBlock = new TextBlock(this.font, "Bottom\nRight\nText\nBlock", textBlockSize, TextAlignment.BottomRight);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.Black);

				this.graphics.Begin();

				Vector2 oneThirdScreen = new Vector2((int)(this.graphicsDevice.BackBufferWidth / 3), (int)(this.graphicsDevice.BackBufferHeight / 3));

				Rectangle oneThirdScreenRectangle = new Rectangle(0, 0, (int)oneThirdScreen.X, (int)oneThirdScreen.Y);

				for (int y = 0; y < 3; y++)
				{
					for (int x = 0; x < 3; x++)
					{
						oneThirdScreenRectangle.X = (int)oneThirdScreen.X * x;
						oneThirdScreenRectangle.Y = (int)oneThirdScreen.Y * y;
						this.graphics.DrawFilledRectangle(oneThirdScreenRectangle, new Color((byte)(80 * x), (byte)(80 * (x - y)), (byte)(80 * (3 - y)), 255));
					}
				}

				this.graphics.DrawTextBlock(this.topLeftTextBlock, new Vector2(0, 0), Color.White);
				this.graphics.DrawTextBlock(this.topCenterTextBlock, new Vector2(oneThirdScreen.X, 0), Color.White);
				this.graphics.DrawTextBlock(this.topRightTextBlock, new Vector2(oneThirdScreen.X * 2, 0), Color.White);

				this.graphics.DrawTextBlock(this.middleLeftTextBlock, new Vector2(0, oneThirdScreen.Y), Color.White);
				this.graphics.DrawTextBlock(this.middleCenterTextBlock, new Vector2(oneThirdScreen.X, oneThirdScreen.Y), Color.White);
				this.graphics.DrawTextBlock(this.middleRightTextBlock, new Vector2(oneThirdScreen.X * 2, oneThirdScreen.Y), Color.White);

				this.graphics.DrawTextBlock(this.bottomLeftTextBlock, new Vector2(0, oneThirdScreen.Y * 2), Color.White);
				this.graphics.DrawTextBlock(this.bottomCenterTextBlock, new Vector2(oneThirdScreen.X, oneThirdScreen.Y * 2), Color.White);
				this.graphics.DrawTextBlock(this.bottomRightTextBlock, new Vector2(oneThirdScreen.X * 2, oneThirdScreen.Y * 2), Color.White);
								
				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		public static void Main()
		{
			using (TextBlockSample game = new TextBlockSample())
				game.Run();
		}
	}
}
