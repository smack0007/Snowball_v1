using System;

namespace Snowball
{
	/// <summary>
	/// Interface for interpolator objects.
	/// </summary>
	public interface IInterpolator
	{
		float ElapsedTime { get; }

		float Duration { get; }

		bool IsFinished { get; }

		EasingFunction Easing { get; }

		void Update(GameTime gameTime);
	}

	/// <summary>
	/// Interface for interpolator objects.
	/// </summary>
	public interface IInterpolator<T> : IInterpolator
	{
		T BeginValue { get; }

		T EndValue { get; }

		T Value { get; }
	}
}
