using System;

namespace Snowball.UI
{
	public class ControlOptions
	{
		public const string DefaultFontName = "UIFont";

		public string FontName
		{
			get;
			set;
		}

		public ControlOptions()
		{
			this.FontName = DefaultFontName;
		}
	}
}
