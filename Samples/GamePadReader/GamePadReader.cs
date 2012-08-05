using System;
using Snowball;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Content;

namespace GamePadReader
{
	public class GamePadReader : Game
	{
		GamePad gamePad;

		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		ContentLoader contentLoader;
		TextureFont font;

		public GamePadReader()
			: base()
		{
			this.Window.Title = "Snowball GamePad Reader";
			
			this.gamePad = new GamePad(PlayerIndex.One);

			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.contentLoader = new ContentLoader(this.Services);
		}
		
		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice();

			this.graphics = new GraphicsBatch(this.graphicsDevice);
			this.font = this.contentLoader.Load<TextureFont>(new LoadTextureFontArgs()
			{
				FileName = "GamePadReaderFont.xml"
			});
		}

		protected override void Update(GameTime gameTime)
		{
			this.gamePad.Update();
		}

		private string GetGamePadStatus()
		{
			string output = string.Empty;

			output += "A: " + (this.gamePad.A ? 1 : 0);
			output += " B: " + (this.gamePad.B ? 1 : 0);
			output += " X: " + (this.gamePad.X ? 1 : 0);
			output += " Y: " + (this.gamePad.Y ? 1 : 0) + "\n";
			output += "Start: " + (this.gamePad.Start ? 1 : 0);
			output += " Back: " + (this.gamePad.Back ? 1 : 0) + "\n";
			output += "DPadUp: " + (this.gamePad.DPadUp ? 1 : 0);
			output += " DPadRight: " + (this.gamePad.DPadRight ? 1 : 0);
			output += " DPadDown: " + (this.gamePad.DPadDown ? 1 : 0);
			output += " DPadLeft: " + (this.gamePad.DPadLeft ? 1 : 0) + "\n";
			output += "LeftShoulder: " + (this.gamePad.LeftShoulder ? 1 : 0);
			output += " RightShoulder: " + (this.gamePad.RightShoulder ? 1 : 0) + "\n";
			output += "LeftThumb: " + (this.gamePad.LeftThumb ? 1 : 0);
			output += " RightThumb: " + (this.gamePad.RightThumb ? 1 : 0) + "\n";
			output += "LeftThumbStick: " + this.gamePad.LeftThumbStick + "\n";
			output += "RightThumbStick: " + this.gamePad.RightThumbStick + "\n";
			output += "LeftTrigger: " + this.gamePad.LeftTrigger + "\n";
			output += "RightTrigger: " + this.gamePad.RightTrigger;

			return output;
		}

		protected override void Draw(GameTime gameTime)
		{
			string gamePadStatus = this.GetGamePadStatus();

			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.CornflowerBlue);

				this.graphics.Begin();

				this.graphics.DrawString(this.font, gamePadStatus, new Vector2(5, 5), Color.White);

				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		public static void Main()
		{
			using(GamePadReader game = new GamePadReader())
				game.Run();
		}
	}
}
