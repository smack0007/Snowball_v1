using System;
using Snowball.Graphics;
using Snowball.Content;

namespace Snowball.UI
{
	public abstract class Control
	{
		Vector2 position;
		ITextureFont font;

		public Control Parent
		{
			get;
			internal set;
		}

		public ControlCollection Controls
		{
			get;
			private set;
		}

		public bool IsInitialized
		{
			get;
			protected set;
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

		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}

		public float X
		{
			get { return this.position.X; }
			set { this.position.X = value; }
		}

		public float Y
		{
			get { return this.position.Y; }
			set { this.position.Y = value; }
		}

		public Vector2 ScreenPosition
		{
			get
			{
				if (this.Parent != null)
					return new Vector2(this.Parent.X + this.position.X, this.Parent.Y + this.position.Y);

				return this.position;
			}
		}

		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
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
				this.font = value;
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public Control()
		{
			this.Enabled = true;
			this.Visible = true;
			this.Controls = new ControlCollection(this);
		}
		
		public virtual void Initialize(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.Controls.Initialize(services);
						
			this.IsInitialized = true;
		}

		public virtual void Update(GameTime gameTime)
		{
			if (gameTime == null)
				throw new ArgumentNullException("gameTime");

			this.Controls.Update(gameTime);
		}

		public virtual void Draw(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			this.Controls.Draw(graphics);
		}
	}
}
