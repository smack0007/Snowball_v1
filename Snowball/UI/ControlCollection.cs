using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;
using Snowball.Content;

namespace Snowball.UI
{
	public class ControlCollection : IEnumerable<Control>
	{
		Control parent;
		List<Control> controls;
		ControlEventArgs eventArgs;

		IServiceProvider services;
		bool isInitialized;

		public event EventHandler<ControlEventArgs> ControlAdded;

		public event EventHandler<ControlEventArgs> ControlRemoved;

		public ControlCollection()
			: this(null)
		{
		}

		public ControlCollection(Control parent)
		{
			this.parent = parent;
			this.controls = new List<Control>();
			this.eventArgs = new ControlEventArgs();
		}

		public IEnumerator<Control> GetEnumerator()
		{
			return this.controls.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(Control control)
		{
			if (control == null)
				throw new ArgumentNullException("control");

			if (control.Parent != null)
				control.Parent.Controls.Remove(control);

			control.Parent = this.parent;
			this.controls.Add(control);

			if (this.isInitialized && !control.IsInitialized)
				control.Initialize(this.services);

			if (this.ControlAdded != null)
			{
				this.eventArgs.Control = control;
				this.ControlAdded(this, this.eventArgs);
			}
		}

		public void Remove(Control control)
		{
			if (control == null)
				throw new ArgumentNullException("control");

			if (control.Parent != this.parent)
				throw new InvalidOperationException("Given control does not belong to this Control.");

			if (this.controls.Remove(control))
			{
				control.Parent = null;

				if (this.ControlRemoved != null)
				{
					this.eventArgs.Control = control;
					this.ControlRemoved(this, this.eventArgs);
				}
			}
		}

		public void Initialize(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			foreach (Control control in this.controls)
			{
				if (!control.IsInitialized)
					control.Initialize(services);
			}

			this.services = services;
			this.isInitialized = true;
		}

		public void Update(GameTime gameTime)
		{
			if (gameTime == null)
				throw new ArgumentNullException("gameTime");

			foreach (Control control in this.controls)
			{
				if (control.Enabled)
					control.Update(gameTime);
			}
		}

		public void Draw(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			foreach (Control control in this.controls)
			{
				if (control.Visible)
					control.Draw(graphics);
			}
		}
	}
}
