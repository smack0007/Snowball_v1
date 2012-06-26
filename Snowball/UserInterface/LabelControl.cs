using System;
using Snowball.Graphics;

namespace Snowball.UserInterface
{
	public class LabelControl : UserInterfaceControl
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

		public LabelControl()
			: base()
		{
			this.TextColor = Color.White;
		}

		public override void Draw(IGraphicsBatch renderer)
		{
			if (renderer == null)
				throw new ArgumentNullException("renderer");

			if (this.Font == null)
				throw new InvalidOperationException("LabelControl requires the Font property to be set on either the control or the manager.");

			if (!string.IsNullOrEmpty(this.Text))
				renderer.DrawString(this.Font, this.Text, this.Position, this.TextColor);
		}
	}
}
