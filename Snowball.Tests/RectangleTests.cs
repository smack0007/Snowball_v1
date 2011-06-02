using System;
using NUnit.Framework;

namespace Snowball.Tests
{
	[TestFixture]
	public class RectangleTests
	{
		[Test]
		public void RightSetterSetsCorrectXValue()
		{
			Rectangle rect = new Rectangle(0, 0, 10, 10);
			rect.Right = 12;

			Assert.AreEqual(2, rect.X);
		}

		[Test]
		public void BottomSetterSetsCorrectYValue()
		{
			Rectangle rect = new Rectangle(0, 0, 10, 10);
			rect.Bottom = 15;

			Assert.AreEqual(5, rect.Y);
		}

		[Test]
		public void IntersectsReturnsTrueWhenRectanglesIntersect()
		{
			Rectangle r1 = new Rectangle(0, 0, 10, 10);
			Rectangle r2 = new Rectangle(5, 5, 10, 10);

			Assert.IsTrue(Rectangle.Intersects(ref r1, ref r2));

			r2.X = 7;
			r2.Y = 8;

			Assert.IsTrue(Rectangle.Intersects(ref r1, ref r2));

			r1.Width = 9;

			Assert.IsTrue(Rectangle.Intersects(ref r1, ref r2));
		}

		[Test]
		public void IntersectsReturnsFalseWhenRectanglesDoNotIntersect()
		{
			Rectangle r1 = new Rectangle(0, 0, 10, 10);
			Rectangle r2 = new Rectangle(15, 15, 10, 10);

			Assert.IsFalse(Rectangle.Intersects(ref r1, ref r2));

			r2.Y = -10;

			Assert.IsFalse(Rectangle.Intersects(ref r1, ref r2));

			r1.Width = 3;
			r1.Height = 15;

			Assert.IsFalse(Rectangle.Intersects(ref r1, ref r2));
		}
	}
}
