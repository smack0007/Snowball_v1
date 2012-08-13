using System;
using Snowball.Graphics;

namespace Snowball.UI
{
	public class Label : Control
	{
		public string Text
		{
			get;
			set;
		}

		public Color TextColor
		{
			get;
			set;
		}

		public Color BackgrounColor
		{
			get;
			set;
		}

		public Label()
			: base()
		{
			this.TextColor = Color.White;
			this.BackgrounColor = Color.Transparent;
		}
						
		public override void Draw(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");
						
			if (!string.IsNullOrEmpty(this.Text))
				graphics.DrawString(this.Font, this.Text, this.ScreenPosition, this.TextColor);

			base.Draw(graphics);
		}
	}
}
