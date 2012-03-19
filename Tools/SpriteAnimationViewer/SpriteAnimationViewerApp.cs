using System;
using Snowball;
using Snowball.Graphics;

namespace SpriteAnimationViewer
{
	public class SpriteAnimationViewerApp : Game
	{
		bool shouldRequestFileName;

		Renderer renderer;
		Texture spriteTexture;

		public SpriteAnimationViewerApp()
			: base()
		{
			this.Window.Title = "Snowball Sprite Animation Viewer";

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

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (this.shouldRequestFileName)
			{
				string fileName;

				if (this.Window.ShowOpenFileDialog("Image Files", new string[] { "*.png", "*.bmp" }, out fileName))
				{
					this.spriteTexture = Texture.FromFile(this.Graphics, fileName, null);
				}

				this.shouldRequestFileName = false;
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if (this.spriteTexture != null)
			{
				this.renderer.Begin();
				this.renderer.DrawTexture(this.spriteTexture, Vector2.Zero, Color.White);
				this.renderer.End();
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
