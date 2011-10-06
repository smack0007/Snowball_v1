using System;
using Snowball.Content;

namespace Snowball.Demo
{
	public class DemoContentLoader : ContentLoader
	{
		public DemoContentLoader(IServiceProvider services)
			: base(services)
		{
			this.RegisterTexture("ConsoleBackground", "ConsoleBackground.png", null);
		}
	}
}
