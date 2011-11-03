using System;
using NUnit.Framework;
using Snowball;
using Snowball.Graphics;

namespace Snowball.Tests
{
	[TestFixture]
	public class CollisionHelperTests
	{
		private Color[] CreateOneFrameColorData()
		{
			Color[] color = new Color[16];

			color[0] = Color.Transparent;
			color[1] = Color.Transparent;
			color[2] = Color.Transparent;
			color[3] = Color.Transparent;

			color[4] = Color.Transparent;
			color[5] = Color.White;
			color[6] = Color.White;
			color[7] = Color.Transparent;

			color[8] = Color.Transparent;
			color[9] = Color.White;
			color[10] = Color.White;
			color[11] = Color.Transparent;

			color[12] = Color.Transparent;
			color[13] = Color.Transparent;
			color[14] = Color.Transparent;
			color[15] = Color.Transparent;

			return color;
		}

		private Color[] CreateTwoFrameColorData()
		{
			Color[] color = new Color[32];

			color[0] = Color.Transparent;
			color[1] = Color.Transparent;
			color[2] = Color.Transparent;
			color[3] = Color.Transparent;
			color[4] = Color.Transparent;
			color[5] = Color.Transparent;
			color[6] = Color.Transparent;
			color[7] = Color.Transparent;

			color[8] = Color.Transparent;
			color[9] = Color.White;
			color[10] = Color.White;
			color[11] = Color.Transparent;
			color[12] = Color.Transparent;
			color[13] = Color.Transparent;
			color[14] = Color.White;
			color[15] = Color.Transparent;

			color[16] = Color.Transparent;
			color[17] = Color.White;
			color[18] = Color.White;
			color[19] = Color.Transparent;
			color[20] = Color.Transparent;
			color[21] = Color.White;
			color[22] = Color.White;
			color[23] = Color.Transparent;

			color[24] = Color.Transparent;
			color[25] = Color.Transparent;
			color[26] = Color.Transparent;
			color[27] = Color.Transparent;
			color[28] = Color.Transparent;
			color[29] = Color.Transparent;
			color[30] = Color.Transparent;
			color[31] = Color.Transparent;

			return color;
		}

		[Test]
		public void PerPixelIntersectWithoutSrc()
		{
			Rectangle destA = new Rectangle(0, 0, 4, 4);
			Color[] dataA = CreateOneFrameColorData();

			Rectangle destB = new Rectangle(0, 0, 4, 4);
			Color[] dataB = CreateOneFrameColorData();

			Assert.IsTrue(CollisionHelper.PerPixelIntersect(dataA, destA, dataB, destB));

			destB = new Rectangle(0, 1, 4, 4);
			Assert.IsTrue(CollisionHelper.PerPixelIntersect(dataA, destA, dataB, destB));

			destB = new Rectangle(1, 1, 4, 4);
			Assert.IsTrue(CollisionHelper.PerPixelIntersect(dataA, destA, dataB, destB));

			destB = new Rectangle(2, 2, 4, 4);
			Assert.IsFalse(CollisionHelper.PerPixelIntersect(dataA, destA, dataB, destB));

			destB = new Rectangle(3, 3, 4, 4);
			Assert.IsFalse(CollisionHelper.PerPixelIntersect(dataA, destA, dataB, destB));
		}

		[Test]
		public void PerPixelIntersectWithSrc()
		{
			Rectangle destA = new Rectangle(0, 0, 4, 4);
			Rectangle srcA = new Rectangle(0, 0, 4, 4);
			Color[] dataA = CreateTwoFrameColorData();

			Rectangle destB = new Rectangle(0, 0, 4, 4);
			Rectangle srcB = new Rectangle(4, 0, 4, 4);
			Color[] dataB = CreateTwoFrameColorData();

			Assert.IsTrue(CollisionHelper.PerPixelIntersect(dataA, destA, srcA, 8, dataB, destB, srcB, 8));

			destB = new Rectangle(1, 1, 4, 4);
			Assert.IsFalse(CollisionHelper.PerPixelIntersect(dataA, destA, srcA, 8, dataB, destB, srcB, 8));

			destB = new Rectangle(2, 1, 4, 4);
			Assert.IsFalse(CollisionHelper.PerPixelIntersect(dataA, destA, srcA, 8, dataB, destB, srcB, 8));

			destB = new Rectangle(-1, -1, 4, 4);
			Assert.IsTrue(CollisionHelper.PerPixelIntersect(dataA, destA, srcA, 8, dataB, destB, srcB, 8));

			destB = new Rectangle(-1, 0, 4, 4);
			Assert.IsTrue(CollisionHelper.PerPixelIntersect(dataA, destA, srcA, 8, dataB, destB, srcB, 8));

			destB = new Rectangle(0, -1, 4, 4);
			Assert.IsTrue(CollisionHelper.PerPixelIntersect(dataA, destA, srcA, 8, dataB, destB, srcB, 8));
		}
	}
}
