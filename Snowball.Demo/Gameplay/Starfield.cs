﻿using System;
using Snowball.Graphics;

namespace Snowball.Demo.Gameplay
{
	/// <summary>
	/// Displays the stars the fly by in the background during gameplay.
	/// </summary>
	public class Starfield : GameComponent
	{
		Star[] stars;
		Random random;

		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public Starfield()
		{
			this.stars = new Star[100];
			for(int i = 0; i < this.stars.Length; i++)
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
				star.Position = this.random.NextVector2(this.Width, this.Height);
			else
				star.Position = new Vector2(this.random.NextFloat(this.Width), 0);

			star.Speed = this.random.Next(100, this.Height / 2);
			star.Size = this.random.Next(1, 4);
		}

		public override void Initialize()
		{
			foreach (Star star in this.stars)
				RandomizeStar(star, true);

			this.IsInitialized = true;
		}
		
		public override void Update(GameTime gameTime)
		{
			foreach(Star star in this.stars)
			{
				star.Y += star.Speed * gameTime.ElapsedTotalSeconds;

				if (star.Y >= this.Height)
					this.RandomizeStar(star, false);
			}
		}

		public override void Draw(IRenderer renderer)
		{
			for(int i = 0; i < this.stars.Length; i++)
				renderer.DrawFilledRectangle(new Rectangle((int)this.stars[i].X, (int)this.stars[i].Y, this.stars[i].Size, this.stars[i].Size), Color.White);
		}
	}
}
