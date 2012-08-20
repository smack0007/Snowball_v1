using System;
using System.Collections.Generic;
using Snowball.Graphics;
using Snowball.Content;
using Snowball.Input;

namespace Snowball.UI
{
	public abstract class Control
	{		
		public class ControlCollection : IEnumerable<Control>
		{
			Control parent;
			List<Control> controls;
			
			internal ControlCollection(Control parent)
			{
				if (parent == null)
					throw new ArgumentNullException("parent");

				this.parent = parent;
				this.controls = new List<Control>();
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

				this.parent.DoControlAdded(control);
			}

			public void Remove(Control control)
			{
				if (control == null)
					throw new ArgumentNullException("control");

				if (control.Parent != this.parent)
					throw new InvalidOperationException("Given control does not belong to this control.");

				if (this.controls.Remove(control))
				{
					control.Parent = null;
					this.parent.DoControlRemoved(control);
				}
			}
		}

		Control parent;
		Point position;
		Size size;
		ITextureFont font;
		
		bool isMouseOver;
		bool isLeftMouseDown;

		ControlEventArgs controlEventArgs;
		MouseEventArgs mouseEventArgs;
		
		public ControlCollection Controls
		{
			get;
			private set;
		}
		
		public Control Parent
		{
			get { return this.parent; }

			internal set
			{
				if (value != this.parent)
				{
					this.parent = value;
					this.OnParentChanged(EventArgs.Empty);
				}
			}
		}
				
		public bool IsInitialized
		{
			get;
			protected set;
		}

		protected IServiceProvider Services
		{
			get;
			private set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public bool Visible
		{
			get;
			set;
		}

		public virtual Point Position
		{
			get { return this.position; }
			
			set
			{
				if (this.position != value)
				{
					this.position = value;
					this.OnPositionChanged(EventArgs.Empty);
				}
			}
		}

		public int X
		{
			get { return this.position.X; }
			set { this.Position = new Point(value, this.position.Y); }
		}

		public int Y
		{
			get { return this.position.Y; }
			set { this.Position = new Point(this.position.X, value); }
		}
				
		public virtual Size Size
		{
			get { return this.size; }
			
			set
			{
				if (value != this.size)
				{
					this.size = value;
					this.OnSizeChanged(EventArgs.Empty);
				}
			}
		}

		public int Width
		{
			get { return this.size.Width; }
			set { this.Size = new Size(value, this.size.Height); }
		}

		public int Height
		{
			get { return this.size.Height; }
			set { this.Size = new Size(this.size.Width, value); }
		}

		public Point ScreenPosition
		{
			get
			{
				if (this.Parent != null)
					return new Point(this.Parent.X + this.position.X, this.Parent.Y + this.position.Y);

				return this.position;
			}
		}

		public Rectangle ScreenRectangle
		{
			get
			{
				if (this.Parent != null)
					return new Rectangle(this.Parent.X + this.position.X, this.Parent.Y + this.position.Y, this.size.Width, this.size.Height);

				return new Rectangle(this.position.X, this.position.Y, this.size.Width, this.size.Height);
			}
		}
				
		public ITextureFont Font
		{
			get
			{
				if (this.font != null)
					return this.font;

				if (this.Parent != null)
					return this.Parent.Font;

				return null;
			}

			set
			{
				if (value != this.font)
				{
					this.font = value;
					this.OnFontChanged(EventArgs.Empty);

					foreach (Control child in this.Controls)
					{
						if (child.font == null)
							child.OnFontChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>
		/// If true, the mouse is currently hovering over the control.
		/// </summary>
		protected bool IsMouseOver
		{
			get { return this.isMouseOver; }

			private set
			{
				if (value != this.isMouseOver)
				{
					this.isMouseOver = value;

					if (this.isMouseOver)
					{
						this.OnMouseEnter(EventArgs.Empty);
					}
					else
					{
						this.OnMouseLeave(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>
		/// If true, the left mouse button is currently down and the mouse is over the control.
		/// </summary>
		protected bool IsLeftMouseDown
		{
			get { return this.isLeftMouseDown; }

			private set
			{
				if (value != this.isLeftMouseDown)
				{
					this.isLeftMouseDown = value;

					this.mouseEventArgs.Button = MouseButtons.Left;

					if (this.isLeftMouseDown)
					{
						this.OnMouseDown(this.mouseEventArgs);
					}
					else
					{
						this.OnMouseUp(this.mouseEventArgs);
					}
				}
			}
		}

		public event EventHandler<ControlEventArgs> ControlAdded;

		public event EventHandler<ControlEventArgs> ControlRemoved;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Control()
		{
			this.Enabled = true;
			this.Visible = true;
			this.Controls = new ControlCollection(this);

			this.controlEventArgs = new ControlEventArgs();
			this.mouseEventArgs = new MouseEventArgs();
		}

		internal void DoControlAdded(Control control)
		{
			if (this.IsInitialized && !control.IsInitialized)
				control.InitializeControl(this.Services);

			this.controlEventArgs.Control = control;
			this.OnControlAdded(this.controlEventArgs);
		}

		internal void DoControlRemoved(Control control)
		{
			this.controlEventArgs.Control = control;
			this.OnControlRemoved(this.controlEventArgs);
		}
		
		protected virtual void InitializeControl(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.Services = services;
			this.IsInitialized = true;

			foreach (Control control in this.Controls)
			{
				if (!control.IsInitialized)
					control.InitializeControl(services);
			}
		}

		protected virtual void ProcessMouseInput(IMouse mouse)
		{
			if (mouse == null)
				throw new ArgumentNullException("mouse");

			// Update the eventArgs object.
			this.mouseEventArgs.Position = mouse.Position;

			Rectangle rectangle = this.ScreenRectangle;
			bool mouseIsWithinRectangle = false;

			if (rectangle.Contains(mouse.Position))
				mouseIsWithinRectangle = true;

			if (mouseIsWithinRectangle)
			{
				this.IsMouseOver = true;
				this.IsLeftMouseDown = mouse.IsButtonDown(MouseButtons.Left);
			}
			else
			{
				this.IsLeftMouseDown = false;
				this.IsMouseOver = false;
			}

			foreach (Control control in this.Controls)
			{
				if (control.Enabled)
					control.ProcessMouseInput(mouse);
			}
		}

		protected virtual void DrawControl(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			foreach (Control control in this.Controls)
			{
				if (control.Visible)
					control.DrawControl(graphics);
			}
		}

		protected virtual void OnControlAdded(ControlEventArgs e)
		{
			if (this.ControlAdded != null)
				this.ControlAdded(this, e);
		}

		protected virtual void OnControlRemoved(ControlEventArgs e)
		{
			if (this.ControlRemoved != null)
				this.ControlRemoved(this, e);
		}

		protected virtual void OnParentChanged(EventArgs e)
		{
		}

		protected virtual void OnPositionChanged(EventArgs e)
		{
		}

		protected virtual void OnSizeChanged(EventArgs e)
		{
		}

		protected virtual void OnFontChanged(EventArgs e)
		{
		}

		protected virtual void OnMouseEnter(EventArgs e)
		{
		}

		protected virtual void OnMouseLeave(EventArgs e)
		{
		}

		protected virtual void OnMouseDown(EventArgs e)
		{
		}

		protected virtual void OnMouseUp(EventArgs e)
		{
		}
	}
}
