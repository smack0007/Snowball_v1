using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Utilities
{
	[AttributeUsage(AttributeTargets.Field)]
	public class CommandLineOptionDescriptionAttribute : Attribute
	{
		public string Description
		{
			get;
			set;
		}

		public CommandLineOptionDescriptionAttribute(string description)
			: base()
		{
			if (description == null)
				throw new ArgumentNullException("description");

			this.Description = description;
		}
	}
}
