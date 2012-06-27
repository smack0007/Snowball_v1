using System;
using Snowball;
using Snowball.Content;
using Snowball.Graphics;

namespace WavingFlag
{
	public class WavingFlagSample : Game
	{
		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		
		ContentLoader contentLoader;

		GraphicsBatchEffectWrapper effect;
		Texture texture;
		
		public WavingFlagSample()
			: base()
		{
			this.Window.Title = "Snowball Waving Flag Sample";

			this.graphicsDevice = new GraphicsDevice(this.Window);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.contentLoader = new ContentLoader(this.Services);
		}
				
		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice(800, 600);

			// Renderer must be created after the graphics device is created.
			this.graphics = new GraphicsBatch(this.graphicsDevice);

			this.effect = new GraphicsBatchEffectWrapper(this.contentLoader.Load<Effect>(new LoadEffectArgs()
			{
				FileName = "WavingFlag.fx"
			}));

			this.texture = this.contentLoader.Load<Texture>(new LoadTextureArgs()
			{
				FileName = "Flag.png"
			});
		}
		
		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.CornflowerBlue);

				this.effect.SetValue<float>("StartX", 100.0f);
				this.effect.SetValue<float>("Time", (float)gameTime.TotalTime.TotalSeconds);

				this.graphics.Begin(this.effect, 0, 0);

				int totalChunks = 100;
				int chunkSize = this.texture.Width / totalChunks;
				Rectangle destination = new Rectangle(100, 142, chunkSize, this.texture.Height);
				Rectangle source = new Rectangle(0, 0, chunkSize, this.texture.Height);

				for (int i = 0; i < totalChunks; i++)
				{
					this.graphics.DrawTexture(this.texture, destination, source, Color.White);

					destination.X += chunkSize;
					source.X += chunkSize;
				}

				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		public static void Main()
		{
			using(WavingFlagSample sample = new WavingFlagSample())
				sample.Run();
		}
	}
}
