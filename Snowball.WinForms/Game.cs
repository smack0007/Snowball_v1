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
		bool isRunning = false;
		Stopwatch stopwatch;

		TimeSpan targetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
		TimeSpan maxElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 10);
		TimeSpan accumulatedTime;
		TimeSpan lastTime;

		protected Game()
		{
		}
				
		public void Run(Form mainForm)
		{
			if (mainForm == null)
				throw new ArgumentNullException("mainForm");
					
			if (this.isRunning)
				throw new InvalidOperationException("Game is already running.");
						
			this.isRunning = true;

			mainForm.HandleCreated += this.MainForm_HandleCreated;
			Application.Idle += this.Application_Idle;
			
			stopwatch = Stopwatch.StartNew();

			if (mainForm.IsHandleCreated)
				this.Initialize();

			Application.Run(mainForm);

			stopwatch.Stop();

			mainForm.HandleCreated -= this.MainForm_HandleCreated;
			Application.Idle -= this.Application_Idle;

			this.isRunning = false;
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

			if (elapsedTime > this.maxElapsedTime)
				elapsedTime = this.maxElapsedTime;

			this.accumulatedTime += elapsedTime;

			bool shouldDraw = false;

			while (this.accumulatedTime >= this.targetElapsedTime)
			{
				this.Update(this.targetElapsedTime);
				this.accumulatedTime -= this.targetElapsedTime;

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
