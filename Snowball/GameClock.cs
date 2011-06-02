using System;
using System.Diagnostics;

namespace Snowball
{
	public class GameClock
	{
		TimeSpan lastElapsedSinceStart;
				
		TimeSpan elapsedWhenPauseBegan;
		TimeSpan timeLostToPause;

		Stopwatch stopwatch;

		int pause;

		/// <summary>
		/// The amount of time that has passed since the last tick.
		/// </summary>
		public TimeSpan ElapsedTimeSinceTick
		{
			get;
			private set;
		}

		/// <summary>
		/// The amount of time that has passed since the last update.
		/// </summary>
		public TimeSpan ElapsedTimeSinceUpdate
		{
			get;
			private set;
		}

		/// <summary>
		/// The amount of time that has passed since the last draw.
		/// </summary>
		public TimeSpan ElapsedTimeSinceDraw
		{
			get;
			private set;
		}

		/// <summary>
		/// The desired number of updates per second.
		/// </summary>
		public int DesiredUpdatesPerSecond
		{
			get;
			set;
		}
		
		/// <summary>
		/// If true, the game should be updated.
		/// </summary>
		public bool ShouldUpdate
		{
			get;
			private set;
		}

		/// <summary>
		/// The desired number of draws per second.
		/// </summary>
		public int DesiredDrawsPerSecond
		{
			get;
			set;
		}
		
		/// <summary>
		/// If true, the game should be drawn.
		/// </summary>
		public bool ShouldDraw
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new GameClock.
		/// </summary>
		public GameClock()
		{
			this.lastElapsedSinceStart = TimeSpan.Zero;

			this.DesiredDrawsPerSecond = 60;
			this.DesiredUpdatesPerSecond = 60;

			this.stopwatch = new Stopwatch();
			this.stopwatch.Start();			
		}

		/// <summary>
		/// Allows the clock to process.
		/// </summary>
		public void Tick()
		{
			if(this.pause <= 0)
			{
				TimeSpan elapsedSinceStart = this.stopwatch.Elapsed;

				this.ElapsedTimeSinceTick = elapsedSinceStart - this.lastElapsedSinceStart - this.timeLostToPause;
								
				this.ElapsedTimeSinceUpdate += this.ElapsedTimeSinceTick;
				if(this.ElapsedTimeSinceUpdate.TotalMilliseconds > (1000.0f / (float)this.DesiredUpdatesPerSecond))
					this.ShouldUpdate = true;

				this.ElapsedTimeSinceDraw += this.ElapsedTimeSinceTick;
				if(this.ElapsedTimeSinceDraw.TotalMilliseconds > (1000.0f / (float)this.DesiredDrawsPerSecond))
					this.ShouldDraw = true;

				this.timeLostToPause = TimeSpan.Zero;

				this.lastElapsedSinceStart = elapsedSinceStart;
			}
			else
			{
				this.ElapsedTimeSinceTick = TimeSpan.Zero;
				this.ShouldUpdate = false;
				this.ShouldDraw = false;
			}
		}

		/// <summary>
		/// Triggers a clock pause.
		/// </summary>
		public void Pause()
		{
			this.pause++;

			if(this.pause == 1)
				this.elapsedWhenPauseBegan = this.stopwatch.Elapsed;
		}

		/// <summary>
		/// Triggers a clock resume.
		/// </summary>
		public void Resume()
		{
			this.pause--;

			if(this.pause < 0)
				this.pause = 0;

			if(this.pause == 0)
			{
				this.timeLostToPause = this.stopwatch.Elapsed - this.elapsedWhenPauseBegan;
				this.elapsedWhenPauseBegan = TimeSpan.Zero;
			}
		}

		/// <summary>
		/// Signals the clock that an update has occured and the flag can be reset.
		/// </summary>
		public void ResetShouldUpdate()
		{
			this.ShouldUpdate = false;
			this.ElapsedTimeSinceUpdate = TimeSpan.Zero;
		}

		/// <summary>
		/// Signals the clock that a draw has occured and the flag can be reset.
		/// </summary>
		public void ResetShouldDraw()
		{
			this.ShouldDraw = false;
			this.ElapsedTimeSinceDraw = TimeSpan.Zero;
		}
	}
}
