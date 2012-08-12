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

		public Label()
			: this(Control.DefaultOptions)
		{
		}

		public Label(ControlOptions options)
			: base(options)
		{
			
			this.TextColor = Color.White;
		}
		
		public override void Draw(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			if (!string.IsNullOrEmpty(this.Text))
				graphics.DrawString(this.Font, this.Text, this.Position, this.TextColor);

			base.Draw(graphics);
		}
	}
}
