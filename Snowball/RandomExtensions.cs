using System;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Contains extension methods related to the Random class.
	/// </summary>
	public static class RandomHelper
	{
		public static Color NextColor(this Random random)
		{
			return new Color((byte)random.Next(), (byte)random.Next(), (byte)random.Next(), (byte)random.Next());
		}

		public static float NextFloat(this Random random)
		{
			return (float)random.NextDouble();
		}

		public static float NextFloat(this Random random, float max)
		{
			return (float)random.NextDouble() * max;
		}

		public static float NextFloat(this Random random, float min, float max)
		{
			return ((float)random.NextDouble() * (max - min)) + min;
		}

		public static Vector2 NextVector2(this Random random)
		{
			return new Vector2((float)random.NextDouble(), (float)random.NextDouble());
		}

		public static Vector2 NextVector2(this Random random, float maxX, float maxY)
		{
			return new Vector2(random.NextFloat(maxX), random.NextFloat(maxY));
		}

		public static Vector2 NextVector2(this Random random, float minX, float maxX, float minY, float maxY)
		{
			return new Vector2(random.NextFloat(minX, maxX), random.NextFloat(minY, maxY));
		}
	}
}
