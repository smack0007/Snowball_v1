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
		public const string DefaultFontName = "UIFont";

		public static readonly Options DefaultOptions = new Options();

		public class Options
		{
			public bool ShouldLoadFont = true;

			public string FontName = DefaultFontName;
		}

		Options options;
				
		/// <summary>
		/// Constructor.
		/// </summary>
		public UIRoot()
			: this(DefaultOptions)
		{			
		}

		public UIRoot(Options options)
			: base()
		{
			if (options == null)
				throw new ArgumentNullException("options");

			this.options = options;
		}
				
		public override void Initialize(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			IContentLoader contentLoader = (IContentLoader)services.GetRequiredService<IContentLoader>();

			if (this.options.ShouldLoadFont)
				this.Font = contentLoader.Load<TextureFont>(this.options.FontName);

			base.Initialize(services);
		}
	}
}
