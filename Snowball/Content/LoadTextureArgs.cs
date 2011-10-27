using System;

namespace Snowball.Content
{
	public class LoadTextureArgs : LoadContentArgs
	{
		public Color? ColorKey
		{
			get;
			set;
		}

		public LoadTextureArgs()
			: base()
		{
		}
	}
}
