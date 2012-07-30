using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Utilities
{
	[AttributeUsage(AttributeTargets.Field)]
	public class CommandLineOptionNameAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public CommandLineOptionNameAttribute(string name)
			: base()
		{
			if (name == null)
				throw new ArgumentNullException("name");

			this.Name = name;
		}
	}
}
