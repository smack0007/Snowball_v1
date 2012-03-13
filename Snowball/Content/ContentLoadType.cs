using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Content
{
	public enum ContentLoadType
	{
		/// <summary>
		/// The content will be loaded from a resource file.
		/// </summary>
		FromFile = 1,

		/// <summary>
		/// The content will be constructed.
		/// </summary>
		Construct
	}
}
