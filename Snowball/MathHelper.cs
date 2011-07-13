using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball
{
	public static class MathHelper
	{
		public const float TwoPi = 6.28318531f;

		public static float ToDegrees(float radians)
		{
			return (float)(radians * 57.295779513082320876798154814105);
		}

		public static float ToRadians(float degrees)
		{
			return (float)(degrees * 0.01745329251994329576923690768489);
		}
	}
}
