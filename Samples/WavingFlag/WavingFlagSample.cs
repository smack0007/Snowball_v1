using System;
using Snowball;
using Snowball.Content;
using Snowball.Graphics;

namespace WavingFlag
{
	public class WavingFlagSample : Game
	{
		GraphicsBatch graphicsBatch;

		GraphicsBatchEffectWrapper effect;
		Texture texture;
		
		public WavingFlagSample()
			: base()
		{
			this.Window.Title = "Snowball Waving Flag Sample";
		}
				
		protected override void Initialize()
		{
			this.Graphics.CreateDevice(800, 600);

			// Renderer must be created after the graphics device is created.
			this.graphicsBatch = new GraphicsBatch(this.Graphics);

			this.effect = new GraphicsBatchEffectWrapper(this.ContentLoader.Load<Effect>(new LoadEffectArgs()
			{
				FileName = "WavingFlag.fx"
			}));

			this.texture = this.ContentLoader.Load<Texture>(new LoadTextureArgs()
			{
				FileName = "Flag.png"
			});
		}
		
		protected override void Draw(GameTime gameTime)
		{
			this.effect.SetValue<float>("StartX", 100.0f);
			this.effect.SetValue<float>("Time", (float)gameTime.TotalTime.TotalSeconds);

			this.graphicsBatch.Begin(this.effect, 0, 0);

			int totalChunks = 100;
			int chunkSize = this.texture.Width / totalChunks;
			Rectangle destination = new Rectangle(100, 142, chunkSize, this.texture.Height);
			Rectangle source = new Rectangle(0, 0, chunkSize, this.texture.Height);

			for (int i = 0; i < totalChunks; i++)
			{
				this.graphicsBatch.DrawTexture(this.texture, destination, source, Color.White);

				destination.X += chunkSize;
				source.X += chunkSize;
			}
						
			this.graphicsBatch.End();
		}

		public static void Main()
		{
			using(WavingFlagSample sample = new WavingFlagSample())
				sample.Run();
		}
	}
}
