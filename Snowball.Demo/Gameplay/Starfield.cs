using System;
using Snowball.Graphics;

namespace Snowball.Demo.Gameplay
{
	/// <summary>
	/// Displays the stars the fly by in the background during gameplay.
	/// </summary>
	public class Starfield
	{
		int width, height;
		Star[] stars;
		Random random;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Starfield(int width, int height)
		{
			this.width = width;
			this.height = height;

			this.stars = new Star[100];
			for(int i = 0; i < this.stars.Length; i++)
				this.stars[i] = new Star();

			this.random = new Random();

			foreach(Star star in this.stars)
				RandomizeStar(star, true);
		}

		/// <summary>
		/// Randomizes a star.
		/// </summary>
		/// <param name="star"></param>
		/// <param name="yPosition">If true, the y position of the star will be randomized.</param>
		private void RandomizeStar(Star star, bool yPosition)
		{
			if (yPosition)
				star.Position = this.random.NextVector2(this.width, this.height);
			else
				star.Position = new Vector2(this.random.NextFloat(this.width), 0);

			star.Speed = this.random.Next(100, this.height / 2);
			star.Size = this.random.Next(1, 4);
		}
		
		public void Update(GameTime gameTime)
		{
			foreach(Star star in this.stars)
			{
				star.Y += star.Speed * gameTime.ElapsedTotalSeconds;

				if (star.Y >= this.height)
					this.RandomizeStar(star, false);
			}
		}

		public void Draw(IRenderer renderer)
		{
			for(int i = 0; i < this.stars.Length; i++)
				renderer.DrawFilledRectangle(new Rectangle((int)this.stars[i].X, (int)this.stars[i].Y, this.stars[i].Size, this.stars[i].Size), Color.White);
		}
	}
}
