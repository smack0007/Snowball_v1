using System;

namespace Snowball
{
	/// <summary>
	/// Base implementation of interpolator.
	/// </summary>
	public abstract class Interpolator<T> : IInterpolator<T>
	{
		/// <summary>
		/// Gets the amount of time since the interpolator began.
		/// </summary>
		public float ElapsedTime
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the starting value of the interpolator.
		/// </summary>
		public T BeginValue
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the final value after the interpolator has finished.
		/// </summary>
		public T EndValue
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the amount of time the interpolation will last.
		/// </summary>
		public float Duration
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the current value of the interpolator.
		/// </summary>
		public T Value
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a flag indicating if the interpolator has finished.
		/// </summary>
		public bool IsFinished
		{
			get { return this.ElapsedTime >= this.Duration; }
		}

		/// <summary>
		/// Gets the easing function used to manage the progression of the
		/// interpolator.
		/// </summary>
		public EasingFunction Easing
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public Interpolator()
		{
		}
				
		/// <summary>
		/// Resets the interpolator.
		/// </summary>
		public void Reset()
		{
			this.Reset(this.BeginValue, this.EndValue, this.Duration, this.Easing);
		}
				
		/// <summary>
		/// Resets the interpolator.
		/// </summary>
		/// <param name="beginValue"></param>
		/// <param name="endValue"></param>
		/// <param name="duration"></param>
		public void Reset(T beginValue, T endValue, float duration)
		{
			this.Reset(beginValue, endValue, duration, this.Easing);
		}

		/// <summary>
		/// Resets the interpolator.
		/// </summary>
		/// <param name="beginValue"></param>
		/// <param name="endValue"></param>
		/// <param name="duration"></param>
		/// <param name="easing"></param>
		public void Reset(T beginValue, T endValue, float duration, EasingFunction easing)
		{
			if(easing == null)
				easing = EasingFunctions.Linear;

			this.BeginValue = beginValue;
			this.EndValue = endValue;
			this.Duration = duration;
			this.Easing = easing;

			this.ElapsedTime = 0.0f;
			this.Value = beginValue;

			if(this.Duration == 0.0f)
				this.Value = endValue;
		}

		/// <summary>
		/// Updates the interpolator.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			if(!this.IsFinished)
			{
				this.ElapsedTime += gameTime.ElapsedTotalSeconds;
				this.Value = this.CalculateValue();

				if(this.IsFinished)
					this.Value = this.EndValue;
			}
			else
			{
				this.Value = this.EndValue;
			}
		}

		protected abstract T CalculateValue();
	}
}
