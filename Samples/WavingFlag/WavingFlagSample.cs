using System;
using Snowball;
using Snowball.Content;
using Snowball.Graphics;

namespace WavingFlag
{
	public class WavingFlagSample : Game
	{
		GraphicsBatch graphicsBatch;

		Effect effect;
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

			this.effect = this.ContentLoader.Load<Effect>(new LoadEffectArgs()
			{
				FileName = "WavingFlag.fx"
			});

			this.texture = this.ContentLoader.Load<Texture>(new LoadTextureArgs()
			{
				FileName = "Flag.png"
			});
		}
		
		protected override void Draw(GameTime gameTime)
		{
			Matrix matrix = new Matrix()
			{
				M11 = 2f * 1f / 800.0f,
				M22 = 2f * -1f / 600.0f,
				M33 = 1f,
				M44 = 1f,
				M41 = -1f,
				M42 = 1f
			};

			matrix.M41 -= matrix.M11;
			matrix.M42 -= matrix.M22;

			//Matrix matrix = Matrix.CreateOrthographicOffCenter(-400.0f, 400.0f, 300.0f, -300.0f, 0, 1);

			this.graphicsBatch.Begin(this.effect, 0, 0);
						
			this.effect.SetValue<Matrix>("TransformMatrix", matrix);
			this.effect.SetValue<float>("Time", (float)gameTime.TotalTime.TotalSeconds);

			// Draw the flag.
			this.graphicsBatch.DrawTexture(this.texture, new Vector2(100, 142), Color.White);
			
			this.graphicsBatch.End();
		}

		public static void Main()
		{
			using(WavingFlagSample sample = new WavingFlagSample())
				sample.Run();
		}
	}
}
