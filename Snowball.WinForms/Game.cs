using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snowball.WinForms
{
	public abstract class Game
	{
		Stopwatch stopwatch;
		TimeSpan accumulatedTime;
		TimeSpan lastTime;

		public bool IsRunning
		{
			get;
			private set;
		}

		protected TimeSpan TargetElapsedTime
		{
			get;
			private set;
		}

		protected TimeSpan MaxElapsedTime
		{
			get;
			private set;
		}
		

		protected Game()
		{
			this.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
			this.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 10);
		}
				
		public void Run(Form mainForm)
		{
			if (mainForm == null)
				throw new ArgumentNullException("mainForm");
					
			if (this.IsRunning)
				throw new InvalidOperationException("Game is already running.");
						
			this.IsRunning = true;

			mainForm.HandleCreated += this.MainForm_HandleCreated;
			Application.Idle += this.Application_Idle;
			
			stopwatch = Stopwatch.StartNew();

			if (mainForm.IsHandleCreated)
				this.Initialize();

			Application.Run(mainForm);

			stopwatch.Stop();

			mainForm.HandleCreated -= this.MainForm_HandleCreated;
			Application.Idle -= this.Application_Idle;

			this.IsRunning = false;
		}

		private void MainForm_HandleCreated(object sender, EventArgs e)
		{
			this.Initialize();
		}

		private void Application_Idle(object sender, EventArgs e)
		{
			Win32Message message;

			while (!Win32Methods.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
			{
				this.Tick();
			}
		}

		private void Tick()
		{
			TimeSpan currentTime = this.stopwatch.Elapsed;
			TimeSpan elapsedTime = currentTime - lastTime;
			lastTime = currentTime;

			if (elapsedTime > this.MaxElapsedTime)
				elapsedTime = this.MaxElapsedTime;

			this.accumulatedTime += elapsedTime;

			bool shouldDraw = false;

			while (this.accumulatedTime >= this.TargetElapsedTime)
			{
				this.Update(this.TargetElapsedTime);
				this.accumulatedTime -= this.TargetElapsedTime;

				shouldDraw = true;
			}

			if (shouldDraw)
				this.Draw();
		}

		public virtual void Initialize()
		{
		}

		public virtual void Update(TimeSpan elapsed)
		{
		}

		public virtual void Draw()
		{
		}
	}
}
