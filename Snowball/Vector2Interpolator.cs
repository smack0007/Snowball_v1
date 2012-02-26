using System;

namespace Snowball
{
	public class Vector2Interpolator : Interpolator<Vector2>
	{
		protected override Vector2 CalculateValue()
		{
			return new Vector2(this.Easing(this.ElapsedTime, this.BeginValue.X, this.EndValue.X - this.BeginValue.X, this.Duration),
				               this.Easing(this.ElapsedTime, this.BeginValue.Y, this.EndValue.Y - this.BeginValue.Y, this.Duration));
		}
	}
}
