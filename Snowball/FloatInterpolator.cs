using System;

namespace Snowball
{
	public class FloatInterpolator : Interpolator<float>
	{
		protected override float CalculateValue()
		{
			return this.Easing(this.ElapsedTime, this.BeginValue, this.EndValue - this.BeginValue, this.Duration);
		}
	}
}
