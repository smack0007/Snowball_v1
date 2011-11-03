using System;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Contains methods for doing collision detection.
	/// </summary>
	public static class CollisionHelper
	{
		/// <summary>
		/// Determines if there is overlap in the non-transparent pixels between two Color arrays.
		/// </summary>
		/// <param name="dataA">The first Color array.</param>
		/// <param name="destA">The destination rectangle for Color array A.</param>
		/// <param name="dataB">The second Color array.</param>
		/// <param name="destB">The destination rectangle for Color array B.</param>
		public static bool PerPixelIntersect(Color[] dataA, Rectangle destA, Color[] dataB, Rectangle destB)
		{
			if(destA.Intersects(destB))
			{
				int top = Math.Max(destA.Top, destB.Top);
				int bottom = Math.Min(destA.Bottom, destB.Bottom);
				int left = Math.Max(destA.Left, destB.Left);
				int right = Math.Min(destA.Right, destB.Right);

				for(int y = top; y < bottom; y++)
				{
					for(int x = left; x < right; x++)
					{
						Color colorA = dataA[x - destA.Left + ((y - destA.Top) * destA.Width)];
						Color colorB = dataB[x - destB.Left + ((y - destB.Top) * destB.Width)];

						if(colorA.A != 0 && colorB.A != 0)
							return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Determines if there is overlap in the non-transparent pixels between two Color arrays.
		/// </summary>
		/// <param name="dataA">The first Color array.</param>
		/// <param name="destA">The destination rectangle for Color array A.</param>
		/// <param name="srcA">The source rectangle for Color array A.</param>
		/// <param name="srcA">The width of each line in Color array A.</param>
		/// <param name="dataB">The second Color array.</param>
		/// <param name="destB">The destination rectangle for Color array B.</param>
		/// <param name="srcB">The source rectangle for Color array B.</param>
		/// <param name="srcB">The width of each line in Color array B.</param>
		public static bool PerPixelIntersect(Color[] dataA, Rectangle destA, Rectangle srcA, int dataAWidth,
										     Color[] dataB, Rectangle destB, Rectangle srcB, int dataBWidth)
		{
			if(destA.Intersects(destB))
			{
				int top = Math.Max(destA.Top, destB.Top);
				int bottom = Math.Min(destA.Bottom, destB.Bottom);
				int left = Math.Max(destA.Left, destB.Left);
				int right = Math.Min(destA.Right, destB.Right);

				for(int y = top; y < bottom; y++)
				{
					for(int x = left; x < right; x++)
					{
						int xA = srcA.Left + (x - destA.Left);
						int yA = srcA.Top + (y - destA.Top);

						Color colorA = dataA[xA + (yA * dataAWidth)];

						int xB = srcB.Left + (x - destB.Left);
						int yB = srcB.Top + (y - destB.Top);

						Color colorB = dataB[xB + (yB * dataBWidth)];

						if(colorA.A != 0 && colorB.A != 0)
							return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Determines if there is overlap in the non-transparent pixels between two SpriteSheets.
		/// </summary>
		/// <param name="spriteSheetA">The first SpriteSheet.</param>
		/// <param name="destA">The destination rectangle for SpriteSheet A.</param>
		/// <param name="currentFrameA">The current frame for SpriteSheet A.</param>
		/// <param name="spriteSheetB">The second SpriteSheet.</param>
		/// <param name="destB">The destination rectangle for SpriteSheet B.</param>
		/// <param name="currentFrameB">The current frame for SpriteSheet B.</param>
		/// <returns></returns>
		public static bool PerPixelIntersect(SpriteSheet spriteSheetA, Rectangle destA, int currentFrameA,
											 SpriteSheet spriteSheetB, Rectangle destB, int currentFrameB)
		{
			return PerPixelIntersect(spriteSheetA.GetColorData(), destA, spriteSheetA[currentFrameA], spriteSheetA.Texture.Width,
									 spriteSheetB.GetColorData(), destB, spriteSheetB[currentFrameB], spriteSheetB.Texture.Width);
		}

		/// <summary>
		/// Determines if there is overlap in the non-transparent pixels between two SpriteSheets.
		/// </summary>
		/// <param name="spriteSheetA">The first SpriteSheet.</param>
		/// <param name="positionA">The position of SpriteSheet A.</param>
		/// <param name="currentFrameA">The current frame for SpriteSheet A.</param>
		/// <param name="spriteSheetB">The second SpriteSheet.</param>
		/// <param name="positionB">The position of SpriteSheet B.</param>
		/// <param name="currentFrameB">The current frame for SpriteSheet B.</param>
		/// <returns></returns>
		public static bool PerPixelIntersect(SpriteSheet spriteSheetA, Vector2 positionA, int currentFrameA,
											 SpriteSheet spriteSheetB, Vector2 positionB, int currentFrameB)
		{
			Rectangle srcA = spriteSheetA[currentFrameA];
			Rectangle srcB = spriteSheetB[currentFrameB];

			return PerPixelIntersect(spriteSheetA, new Rectangle((int)positionA.X, (int)positionA.Y, srcA.Width, srcA.Height), currentFrameA,
									 spriteSheetB, new Rectangle((int)positionB.X, (int)positionB.Y, srcB.Width, srcB.Height), currentFrameB);
		}
	}
}
