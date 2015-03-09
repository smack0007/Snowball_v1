using System;
using Snowball.GameFramework;
using Snowball.Graphics;

namespace Snowball.Demo.Gameplay
{
	/// <summary>
	/// Displays the stars the fly by in the background during gameplay.
	/// </summary>
	public class Starfield
	{
		IGraphicsDevice graphicsDevice;

		Star[] stars;
		Random random;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public Starfield(IGraphicsDevice graphicsDevice)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			this.graphicsDevice = graphicsDevice;

			this.stars = new Star[100];
			for (int i = 0; i < this.stars.Length; i++)
				this.stars[i] = new Star();

			this.random = new Random();
		}

		/// <summary>
		/// Randomizes a star.
		/// </summary>
		/// <param name="star"></param>
		/// <param name="yPosition">If true, the y position of the star will be randomized.</param>
		private void RandomizeStar(Star star, bool yPosition)
		{
			if (yPosition)
				star.Position = this.random.NextVector2(this.graphicsDevice.BackBufferWidth, this.graphicsDevice.BackBufferHeight);
			else
				star.Position = new Vector2(this.random.NextFloat(this.graphicsDevice.BackBufferWidth), 0);

			star.Speed = this.random.Next(100, this.graphicsDevice.BackBufferHeight / 2);
			star.Size = this.random.Next(1, 4);
		}

		public void Initialize()
		{
			foreach (Star star in this.stars)
				RandomizeStar(star, true);
		}
		
		public void Update(GameTime gameTime)
		{
			foreach (Star star in this.stars)
			{
				star.Y += star.Speed * gameTime.ElapsedTotalSeconds;

				if (star.Y >= this.graphicsDevice.BackBufferHeight)
					this.RandomizeStar(star, false);
			}
		}

		public void Draw(IGraphicsBatch graphics)
		{
			for (int i = 0; i < this.stars.Length; i++)
				graphics.DrawFilledRectangle(new Rectangle((int)this.stars[i].X, (int)this.stars[i].Y, this.stars[i].Size, this.stars[i].Size), Color.White);
		}
	}
}
