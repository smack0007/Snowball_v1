using System;
using Snowball.Graphics;

namespace Snowball.UserInterface
{
	public abstract class UserInterfaceControl
	{
		TextureFont font;

		/// <summary>
		/// The manager of the control.
		/// </summary>
		public UserInterfaceManager Manager
		{
			get;
			set;
		}

		public TextureFont Font
		{
			get
			{
				if (this.font != null)
					return this.font;

				if (this.Manager != null && this.Manager.Font != null)
					return this.Manager.Font;

				return null;
			}

			set
			{
				this.font = value;
			}
		}

		public Vector2 Position
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public UserInterfaceControl()
		{
		}

		public abstract void Draw(IRenderer renderer);
	}
}
