using System;

namespace Snowball.Demo
{
	public static class RandomExtensions
	{
		public static float NextFloat(this Random random, float max)
		{
			return (float)random.NextDouble() * max;
		}

		public static Vector2 NextVector2(this Random random, float maxX, float maxY)
		{
			return new Vector2(random.NextFloat(maxX), random.NextFloat(maxY));
		}
	}
}
