using System;
using Snowball.Graphics;

namespace Snowball
{
	public static class CollisionHelper
	{
		/// <summary>
		/// Determines if there is overlap of the non-transparent pixels
		/// between two sprites.
		/// </summary>
		public static bool PerPixelIntersect(Rectangle destA, Color[] dataA,
											 Rectangle destB, Color[] dataB)
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
		/// Determines if there is overlap of the non-transparent pixels between two sprites.
		/// </summary>
		public static bool PerPixelIntersect(Rectangle destA, Rectangle srcA, Color[] dataA, int dataAWidth,
										     Rectangle destB, Rectangle srcB, Color[] dataB, int dataBWidth)
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

		public static bool PerPixelIntersect(Rectangle destA, SpriteSheet spriteSheetA, int currentFrameA,
											 Rectangle destB, SpriteSheet spriteSheetB, int currentFrameB)
		{
			return PerPixelIntersect(destA, spriteSheetA[currentFrameA], spriteSheetA.Texture.GetColorData(), spriteSheetA.Texture.Width,
									 destB, spriteSheetB[currentFrameB], spriteSheetB.Texture.GetColorData(), spriteSheetB.Texture.Width);
		}
	}
}
