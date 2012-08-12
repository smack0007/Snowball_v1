using System;
using Snowball.Graphics;
using Snowball.Content;

namespace Snowball.UI
{
	public abstract class Control
	{
		public static readonly ControlOptions DefaultOptions = new ControlOptions();

		ControlOptions options;

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
			get;
			set;
		}

		public IContentLoader ContentLoader
		{
			get;
			set;
		}
				
		public TextureFont Font
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public Control()
			: this(DefaultOptions)
		{	
		}

		public Control(ControlOptions options)
		{
			this.Controls = new ControlCollection(this);

			if (options == null)
				throw new ArgumentNullException("options");

			this.options = options;
			
			this.Enabled = true;
			this.Visible = true;
		}

		public virtual void Initialize(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.Controls.Initialize(services);

			if (this.ContentLoader == null)
				this.ContentLoader = (IContentLoader)services.GetRequiredService<IContentLoader>();

			this.Font = this.ContentLoader.Load<TextureFont>(this.options.FontName);

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
