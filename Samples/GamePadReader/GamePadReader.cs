using System;
using Snowball;
using Snowball.GameFramework;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Content;
using System.Text;

namespace GamePadReader
{
	public class GamePadReader : Game
	{
		const string ButtonPressedText = "1";
		const string ButtonNotPressedText = "0";

		GamePad gamePad;

		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		ContentLoader contentLoader;
		TextureFont font;

		public GamePadReader()
			: base()
		{
			this.Window.Title = "Snowball GamePad Reader";
			
			this.gamePad = new GamePad(GamePadIndex.One);

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
			StringBuilder sb = new StringBuilder();

			sb.Append("A: ");
			sb.Append(this.gamePad.A ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" B: ");
			sb.Append(this.gamePad.B ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" X: ");
			sb.Append(this.gamePad.X ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" Y: ");
			sb.AppendLine(this.gamePad.Y ? ButtonPressedText : ButtonNotPressedText);

			sb.Append("Start: ");
			sb.Append(this.gamePad.Start ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" Back: ");
			sb.AppendLine(this.gamePad.Back ? ButtonPressedText : ButtonNotPressedText);
			
			sb.Append("DPadUp: ");
			sb.Append(this.gamePad.DPadUp ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" DPadRight: ");
			sb.Append(this.gamePad.DPadRight ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" DPadDown: ");
			sb.Append(this.gamePad.DPadDown ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" DPadLeft: ");
			sb.AppendLine(this.gamePad.DPadLeft ? ButtonPressedText : ButtonNotPressedText);

			sb.Append("LeftShoulder: ");
			sb.Append(this.gamePad.LeftShoulder ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" RightShoulder: ");
			sb.AppendLine(this.gamePad.RightShoulder ? ButtonPressedText : ButtonNotPressedText);

			sb.Append("LeftThumb: ");
			sb.Append(this.gamePad.LeftThumb ? ButtonPressedText : ButtonNotPressedText);
			sb.Append(" RightThumb: ");
			sb.AppendLine(this.gamePad.RightThumb ? ButtonPressedText : ButtonNotPressedText);
			
			sb.Append("LeftThumbStick: ");
			sb.AppendLine(this.gamePad.LeftThumbStick.ToString());

			sb.Append("RightThumbStick: ");
			sb.AppendLine(this.gamePad.RightThumbStick.ToString());

			sb.Append("LeftTrigger: ");
			sb.AppendLine(this.gamePad.LeftTrigger.ToString());

			sb.Append("RightTrigger: ");
			sb.AppendLine(this.gamePad.RightTrigger.ToString());

			return sb.ToString();
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
