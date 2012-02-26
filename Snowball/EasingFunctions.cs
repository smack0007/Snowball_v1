// Definately need to credit http://www.robertpenner.com/easing/ and http://xnatweener.codeplex.com/ for helping with these functions.

using System;

namespace Snowball
{
	/// <summary>
	/// Delegate for easing functions.
	/// </summary>
	/// <param name="elapsedTime"></param>
	/// <param name="beginValue"></param>
	/// <param name="valueDelta"></param>
	/// <param name="duration"></param>
	/// <returns></returns>
	public delegate float EasingFunction(float elapsedTime, float beginValue, float valueDelta, float duration);

	/// <summary>
	/// Contains a set of easing functions.
	/// </summary>
	public static class EasingFunctions
	{
		public static float Linear(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * elapsedTime / duration + beginValue;
		}

		public static float BackEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * (elapsedTime /= duration) * elapsedTime * ((1.70158f + 1) * elapsedTime - 1.70158f) + beginValue;
		}

		public static float BackEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * ((elapsedTime = elapsedTime / duration - 1) * elapsedTime * ((1.70158f + 1) * elapsedTime + 1.70158f) + 1) + beginValue;
		}

		public static float BackEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			float s = 1.70158f;

			if((elapsedTime /= duration / 2) < 1)
				return valueDelta / 2 * (elapsedTime * elapsedTime * (((s *= (1.525f)) + 1) * elapsedTime - s)) + beginValue;

			return valueDelta / 2 * ((elapsedTime -= 2) * elapsedTime * (((s *= (1.525f)) + 1) * elapsedTime + s) + 2) + beginValue;
		}

		public static float BounceEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta - BounceEaseOut(duration - elapsedTime, 0, valueDelta, duration) + beginValue;
		}

		public static float BounceEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if((elapsedTime /= duration) < (1 / 2.75))
			{
				return valueDelta * (7.5625f * elapsedTime * elapsedTime) + beginValue;
			}
			else if(elapsedTime < (2 / 2.75))
			{
				return valueDelta * (7.5625f * (elapsedTime -= (1.5f / 2.75f)) * elapsedTime + .75f) + beginValue;
			}
			else if(elapsedTime < (2.5 / 2.75))
			{
				return valueDelta * (7.5625f * (elapsedTime -= (2.25f / 2.75f)) * elapsedTime + .9375f) + beginValue;
			}
			else
			{
				return valueDelta * (7.5625f * (elapsedTime -= (2.625f / 2.75f)) * elapsedTime + .984375f) + beginValue;
			}
		}

		public static float BounceEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if(elapsedTime < duration / 2)
				return BounceEaseIn(elapsedTime * 2, 0, valueDelta, duration) * 0.5f + beginValue;

			else return BounceEaseOut(elapsedTime * 2 - duration, 0, valueDelta, duration) * .5f + valueDelta * 0.5f + beginValue;
		}

		public static float CircularEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return -valueDelta * ((float)Math.Sqrt(1 - (elapsedTime /= duration) * elapsedTime) - 1) + beginValue;
		}

		public static float CircularEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * (float)Math.Sqrt(1 - (elapsedTime = elapsedTime / duration - 1) * elapsedTime) + beginValue;
		}

		public static float CircularEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if((elapsedTime /= duration / 2) < 1)
				return -valueDelta / 2 * ((float)Math.Sqrt(1 - elapsedTime * elapsedTime) - 1) + beginValue;

			return valueDelta / 2 * ((float)Math.Sqrt(1 - (elapsedTime -= 2) * elapsedTime) + 1) + beginValue;
		}

		public static float CubicEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * (elapsedTime /= duration) * elapsedTime * elapsedTime + beginValue;
		}

		public static float CubicEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * ((elapsedTime = elapsedTime / duration - 1) * elapsedTime * elapsedTime + 1) + beginValue;
		}

		public static float CubicEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if((elapsedTime /= duration / 2) < 1)
				return valueDelta / 2 * elapsedTime * elapsedTime * elapsedTime + beginValue;

			return valueDelta / 2 * ((elapsedTime -= 2) * elapsedTime * elapsedTime + 2) + beginValue;
		}

		public static float ElasticEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if(elapsedTime == 0)
				return beginValue;

			if((elapsedTime /= duration) == 1)
				return beginValue + valueDelta;

			float p = duration * 0.3f;
			float s = p / 4;

			return -(valueDelta * (float)Math.Pow(2, 10 * (elapsedTime -= 1)) * (float)Math.Sin((elapsedTime * duration - s) * (2 * Math.PI) / p)) + beginValue;
		}

		public static float ElasticEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if(elapsedTime == 0)
				return beginValue;

			if((elapsedTime /= duration) == 1)
				return beginValue + valueDelta;

			float p = duration * .3f;
			float s = p / 4;

			return (float)(valueDelta * Math.Pow(2, -10 * elapsedTime) * Math.Sin((elapsedTime * duration - s) * (2 * Math.PI) / p) + valueDelta + beginValue);
		}

		public static float ElasticEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if(elapsedTime == 0)
				return beginValue;

			if((elapsedTime /= duration / 2) == 2)
				return beginValue + valueDelta;

			float p = duration * (.3f * 1.5f);
			float a = valueDelta;
			float s = p / 4;

			if(elapsedTime < 1)
				return -.5f * (float)(a * Math.Pow(2, 10 * (elapsedTime -= 1)) * Math.Sin((elapsedTime * duration - s) * (2 * Math.PI) / p)) + beginValue;

			return (float)(a * Math.Pow(2, -10 * (elapsedTime -= 1)) * Math.Sin((elapsedTime * duration - s) * (2 * Math.PI) / p) * .5f + valueDelta + beginValue);
		}

		public static float ExponentialEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if(elapsedTime == 0)
				return beginValue;

			return valueDelta * (float)Math.Pow(2, 10 * (elapsedTime / duration - 1)) + beginValue;
		}

		public static float ExponentialEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if(elapsedTime == duration)
				return beginValue + valueDelta;

			return valueDelta * (float)(-Math.Pow(2, -10 * elapsedTime / duration) + 1) + beginValue;
		}

		public static float ExponentialEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if(elapsedTime == 0)
				return beginValue;

			if(elapsedTime == duration)
				return beginValue + valueDelta;

			if((elapsedTime /= duration / 2) < 1)
				return valueDelta / 2 * (float)Math.Pow(2, 10 * (elapsedTime - 1)) + beginValue;

			return valueDelta / 2 * (float)(-Math.Pow(2, -10 * --elapsedTime) + 2) + beginValue;
		}

		public static float QuadraticEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * (elapsedTime /= duration) * elapsedTime + beginValue;
		}

		public static float QuadraticEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return -valueDelta * (elapsedTime /= duration) * (elapsedTime - 2) + beginValue;
		}

		public static float QuadraticEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if((elapsedTime /= duration / 2) < 1)
				return valueDelta / 2 * elapsedTime * elapsedTime + beginValue;
			else
				return -valueDelta / 2 * ((--elapsedTime) * (elapsedTime - 2) - 1) + beginValue;
		}

		public static float QuarticEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * (elapsedTime /= duration) * elapsedTime * elapsedTime * elapsedTime + beginValue;
		}

		public static float QuarticEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return -valueDelta * ((elapsedTime = elapsedTime / duration - 1) * elapsedTime * elapsedTime * elapsedTime - 1) + beginValue;
		}

		public static float QuarticEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if((elapsedTime /= duration / 2) < 1)
				return valueDelta / 2 * elapsedTime * elapsedTime * elapsedTime * elapsedTime + beginValue;

			return -valueDelta / 2 * ((elapsedTime -= 2) * elapsedTime * elapsedTime * elapsedTime - 2) + beginValue;
		}

		public static float QuinticEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * (elapsedTime /= duration) * elapsedTime * elapsedTime * elapsedTime * elapsedTime + beginValue;
		}

		public static float QuinticEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * ((elapsedTime = elapsedTime / duration - 1) * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 1) + beginValue;
		}

		public static float QuinticEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			if((elapsedTime /= duration / 2) < 1)
				return valueDelta / 2 * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + beginValue;

			return valueDelta / 2 * ((elapsedTime -= 2) * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 2) + beginValue;
		}

		public static float SinusoidalEaseIn(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return -valueDelta * (float)Math.Cos(elapsedTime / duration * (Math.PI / 2)) + valueDelta + beginValue;
		}

		public static float SinusoidalEaseOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return valueDelta * (float)Math.Sin(elapsedTime / duration * (Math.PI / 2)) + beginValue;
		}

		public static float SinusoidalEaseInOut(float elapsedTime, float beginValue, float valueDelta, float duration)
		{
			return -valueDelta / 2 * ((float)Math.Cos(Math.PI * elapsedTime / duration) - 1) + beginValue;
		}
	}
}
