using System;
using Snowball;
using Snowball.Graphics;
using Snowball.UserInterface;

namespace SpriteAnimationViewer
{
	public class SpriteAnimationViewerApp : Game
	{
		bool shouldRequestFileName;

		UserInterfaceManager userInterface;
		Renderer renderer;
		Texture spriteTexture;
		SpriteSheet spriteSheet;

		public SpriteAnimationViewerApp()
			: base()
		{
			this.Window.Title = "Snowball Sprite Animation Viewer";

			this.userInterface = new UserInterfaceManager(this.Window);
			this.userInterface.AddControl(new LabelControl() { Text = "Hello World!" });

			this.shouldRequestFileName = true;
		}

		protected override void InitializeDevices()
		{
			this.Graphics.CreateDevice(800, 600);
		}

		protected override void Initialize()
		{
			this.renderer = new Renderer(this.Graphics);
		}

		protected override void LoadContent()
		{
			this.userInterface.Font = new TextureFont(this.Graphics, "Arial", 12, true);
		}

		protected override void UnloadContent()
		{
			this.userInterface.Font.Dispose();
			this.userInterface.Font = null;
		}

		protected override void Update(GameTime gameTime)
		{
			if (this.shouldRequestFileName)
			{
				string fileName;

				if (this.Window.ShowOpenFileDialog("Image Files", new string[] { "*.png", "*.bmp" }, out fileName))
				{
					this.spriteTexture = Texture.FromFile(this.Graphics, fileName, null);
					this.spriteSheet = new SpriteSheet(this.spriteTexture, 32, 32);
				}

				this.shouldRequestFileName = false;
			}

			this.userInterface.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.renderer.Begin();

			if (this.spriteSheet != null && this.spriteSheet.FrameCount > 0)
				this.renderer.DrawSprite(this.spriteSheet, 0, Vector2.Zero, Color.White);
				
			this.userInterface.Draw(this.renderer);

			this.renderer.End();
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
