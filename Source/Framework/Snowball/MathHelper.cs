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

        /// <summary>
        /// Rounds up to the next power of 2. Returns 0 if given 0.
        /// </summary>
        /// <remarks>Implementation of algorithm found at "http://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2".</remarks>
        /// <param name="input"></param>
        /// <returns></returns>
        public static uint NextPowerOf2(uint input)
        {
            if (input == 0)
                return 0;

            input--;
            input |= input >> 1;
            input |= input >> 2;
            input |= input >> 4;
            input |= input >> 8;
            input |= input >> 16;
            input++;

            return input;
        }
	}
}
