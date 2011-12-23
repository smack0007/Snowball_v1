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
			if (destA.Intersects(destB))
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

						if (colorA.A != 0 && colorB.A != 0)
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
			if (destA.Intersects(destB))
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

						if (colorA.A != 0 && colorB.A != 0)
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

		/// <summary>
		/// Determines if there is overlap of the non-transparent pixels between two
		/// sprites.
		/// </summary>
		/// <param name="transformA">World transform of the first sprite.</param>
		/// <param name="widthA">Width of the first sprite's texture.</param>
		/// <param name="heightA">Height of the first sprite's texture.</param>
		/// <param name="dataA">Pixel color data of the first sprite.</param>
		/// <param name="transformB">World transform of the second sprite.</param>
		/// <param name="widthB">Width of the second sprite's texture.</param>
		/// <param name="heightB">Height of the second sprite's texture.</param>
		/// <param name="dataB">Pixel color data of the second sprite.</param>
		/// <returns>True if non-transparent pixels overlap; false otherwise</returns>
		public static bool PerPixelIntersect(Matrix transformA, Color[] dataA, int widthA, int heightA, 
											 Matrix transformB, Color[] dataB, int widthB, int heightB)
		{
			// Calculate a matrix which transforms from A's local space into
			// world space and then into B's local space
			Matrix transformAToB = transformA * Matrix.Invert(ref transformB);

			// When a point moves in A's local space, it moves in B's local space with a
			// fixed direction and distance proportional to the movement in A.
			// This algorithm steps through A one pixel at a time along A's X and Y axes
			// Calculate the analogous steps in B:
			Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, ref transformAToB);
			Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, ref transformAToB);

			// Calculate the top left corner of A in B's local space
			// This variable will be reused to keep track of the start of each row
			Vector2 yPosInB = Vector2.Transform(Vector2.Zero, ref transformAToB);

			// For each row of pixels in A
			for(int yA = 0; yA < heightA; yA++)
			{
				// Start at the beginning of the row
				Vector2 posInB = yPosInB;

				// For each pixel in this row
				for(int xA = 0; xA < widthA; xA++)
				{
					// Round to the nearest pixel
					int xB = (int)Math.Round(posInB.X);
					int yB = (int)Math.Round(posInB.Y);

					// If the pixel lies within the bounds of B
					if (0 <= xB && xB < widthB &&
					   0 <= yB && yB < heightB)
					{
						// Get the colors of the overlapping pixels
						Color colorA = dataA[xA + yA * widthA];
						Color colorB = dataB[xB + yB * widthB];

						// If both pixels are not completely transparent,
						if (colorA.A != 0 && colorB.A != 0)
						{
							// then an intersection has been found
							return true;
						}
					}

					// Move to the next pixel in the row
					posInB += stepX;
				}

				// Move to the next row
				yPosInB += stepY;
			}

			// No intersection found
			return false;
		}
	}
}
