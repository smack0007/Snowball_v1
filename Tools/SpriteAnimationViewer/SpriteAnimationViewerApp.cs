using System;
using Snowball;
using Snowball.Graphics;
using Snowball.UserInterface;

namespace SpriteAnimationViewer
{
	public class SpriteAnimationViewerApp : Game
	{
		bool shouldRequestFileName;

		//UserInterfaceManager userInterface;

		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		
		Texture spriteTexture;
		SpriteSheet spriteSheet;

		public SpriteAnimationViewerApp()
			: base()
		{
			this.Window.Title = "Snowball Sprite Animation Viewer";

			//this.userInterface = new UserInterfaceManager(this.Window);
			//this.userInterface.AddControl(new LabelControl() { Text = "Hello World!" });

			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.shouldRequestFileName = true;
		}
		
		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice(800, 600);

			this.graphics = new GraphicsBatch(this.graphicsDevice);

			//this.userInterface.Font = new TextureFont(this.Graphics, "Arial", 12, true);
		}
		
		protected override void Update(GameTime gameTime)
		{
			if (this.shouldRequestFileName)
			{
				string fileName;

				if (this.Window.ShowOpenFileDialog("Image Files", new string[] { "*.png", "*.bmp" }, out fileName))
				{
					this.spriteTexture = Texture.FromFile(this.graphicsDevice, fileName, null);
					this.spriteSheet = new SpriteSheet(this.spriteTexture, 32, 32);
				}

				this.shouldRequestFileName = false;
			}

			//this.userInterface.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.CornflowerBlue);

				this.graphics.Begin();

				if (this.spriteSheet != null && this.spriteSheet.FrameCount > 0)
					this.graphics.DrawSprite(this.spriteSheet, 0, Vector2.Zero, Color.White);

				//this.userInterface.Draw(this.graphics);

				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			using (SpriteAnimationViewerApp app = new SpriteAnimationViewerApp())
				app.Run();
		}
	}
}
