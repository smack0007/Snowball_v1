using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;

namespace Snowball.UI
{
	public class Button : Control
	{
		public Texture Texture
		{
			get;
			set;
		}

		public Button()
			: base()
		{
		}
		
		public override void Initialize(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			IUIContentLoader contentLoader = (IUIContentLoader)services.GetRequiredService<IUIContentLoader>();
			this.Texture = contentLoader.LoadButtonTexture();

			if (this.Texture == null)
				throw new InvalidOperationException("IUIContentLoader failed to load Button texture.");

			base.Initialize(services);
		}

		public override void Draw(IGraphicsBatch graphics)
		{
			Rectangle srcTopLeft = new Rectangle(0, 0, 8, 8);
			Rectangle srcTopCenter = new Rectangle(8, 0, 8, 8);
			Rectangle srcTopRight = new Rectangle(16, 0, 8, 8);
			
			Rectangle srcMiddleLeft = new Rectangle(0, 8, 8, 8);
			Rectangle srcMiddleCenter = new Rectangle(8, 8, 8, 8);
			Rectangle srcMiddleRight = new Rectangle(16, 8, 8, 8);
			
			Rectangle srcBottomLeft = new Rectangle(0, 16, 8, 8);
			Rectangle srcBottomCenter = new Rectangle(8, 16, 8, 8);
			Rectangle srcBottomRight = new Rectangle(16, 16, 8, 8);

			int x = (int)this.X;
			int y = (int)this.Y;

			int midWidth = this.Width - 16;
			int midHeight = this.Height - 16;

			Rectangle destTopLeft = new Rectangle(x, y, 8, 8);
			Rectangle destTopCenter = new Rectangle(x + 8, y, midWidth, 8);
			Rectangle destTopRight = new Rectangle(x + 8 + midWidth, y, 8, 8);
						
			Rectangle destMiddleLeft = new Rectangle(x, y + 8, 8, midHeight);
			Rectangle destMiddleCenter = new Rectangle(x + 8, y + 8, midWidth, midHeight);
			Rectangle destMiddleRight = new Rectangle(x + 8 + midWidth, y + 8, 8, midHeight);
			
			Rectangle destBottomLeft = new Rectangle(x, y + 8 + midHeight, 8, 8);
			Rectangle destBottomCenter = new Rectangle(x + 8, y + 8 + midHeight, midWidth, 8);
			Rectangle destBottomRight = new Rectangle(x + 8 + midWidth, y + 8 + midHeight, 8, 8);

			graphics.DrawTexture(this.Texture, destTopLeft, srcTopLeft, Color.White);
			graphics.DrawTexture(this.Texture, destTopCenter, srcTopCenter, Color.White);
			graphics.DrawTexture(this.Texture, destTopRight, srcTopRight, Color.White);
			
			graphics.DrawTexture(this.Texture, destMiddleLeft, srcMiddleLeft, Color.White);
			graphics.DrawTexture(this.Texture, destMiddleCenter, srcMiddleCenter, Color.White);
			graphics.DrawTexture(this.Texture, destMiddleRight, srcMiddleRight, Color.White);

			graphics.DrawTexture(this.Texture, destBottomLeft, srcBottomLeft, Color.White);
			graphics.DrawTexture(this.Texture, destBottomCenter, srcBottomCenter, Color.White);
			graphics.DrawTexture(this.Texture, destBottomRight, srcBottomRight, Color.White);

			base.Draw(graphics);
		}
	}
}
