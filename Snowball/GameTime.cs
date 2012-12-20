using System;

namespace Snowball
{
	/// <summary>
	/// Contains values relating to the time passed in the game.
	/// </summary>
	public sealed class GameTime
	{
		public static readonly GameTime Zero = new GameTime(TimeSpan.Zero, TimeSpan.Zero);

		/// <summary>
		/// The amount of elapsed time since the last update.
		/// </summary>
		public TimeSpan ElapsedTime
		{
			get;
			set;
		}
				
		/// <summary>
		/// The total amount of time in the game.
		/// </summary>
		public TimeSpan TotalTime
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the TotalSeconds property of the ElapsedTime timespan as a float.
		/// </summary>
		public float ElapsedTotalSeconds
		{
			get { return (float)this.ElapsedTime.TotalSeconds; }
		}

		/// <summary>
		/// Returns the TotalSeconds property of the TotalTime timespan as a float.
		/// </summary>
		public float TotalSeconds
		{
			get { return (float)this.TotalTime.TotalSeconds; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameTime()
			: this(TimeSpan.Zero, TimeSpan.Zero)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="elapsedTime"></param>
		/// <param name="totalTime"></param>
		public GameTime(TimeSpan elapsedTime, TimeSpan totalTime)
		{
			this.ElapsedTime = elapsedTime;
			this.TotalTime = totalTime;
		}
	}
}
