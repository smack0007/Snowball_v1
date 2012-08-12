using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;
using Snowball.Content;

namespace Snowball.UI
{
	public class UIManager
	{					
		public ControlCollection Controls
		{
			get;
			private set;
		}
				
		/// <summary>
		/// Constructor.
		/// </summary>
		public UIManager()
		{			
			this.Controls = new ControlCollection();
		}
				
		public void Initialize(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.Controls.Initialize(services);
		}

		public void Update(GameTime gameTime)
		{
			if (gameTime == null)
				throw new ArgumentNullException("gameTime");

			this.Controls.Update(gameTime);
		}

		public void Draw(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			this.Controls.Draw(graphics);
		}
	}
}
