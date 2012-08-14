using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;
using Snowball.Content;

namespace Snowball.UI
{
	public class UIRoot : Control
	{					
		/// <summary>
		/// Constructor.
		/// </summary>
		public UIRoot()
			: base()
		{			
		}
				
		public override void Initialize(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			IUIContentLoader contentLoader = (IUIContentLoader)services.GetRequiredService<IUIContentLoader>();
			this.Font = contentLoader.LoadFont();

			if (this.Font == null)
				throw new InvalidOperationException("IUIContentLoader failed to load font.");

			base.Initialize(services);
		}
	}
}
