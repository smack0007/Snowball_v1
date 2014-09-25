using System;
using NUnit.Framework;

namespace Snowball.Tests
{
	[TestFixture]
	public class ColorTests
	{
		[Test]
		public void LimitedColorHasCorrectValues()
		{
			Color color = new Color(40, 80, 120, 160);

			Assert.AreEqual(new Color(0, 80, 120, 160), color.Limit(0, 255, 255, 255));
			Assert.AreEqual(new Color(40, 40, 120, 160), color.Limit(255, 40, 255, 255));
			Assert.AreEqual(new Color(40, 80, 120, 160), color.Limit(255, 255, 255, 255));
			Assert.AreEqual(new Color(40, 80, 120, 16), color.Limit(new Color(255, 255, 255, 16)));
		}
	}
}
