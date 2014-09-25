using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Commands
{
	[AttributeUsage(AttributeTargets.Field)]
	public class OptionDescriptionAttribute : Attribute
	{
		public string Description
		{
			get;
			set;
		}

		public OptionDescriptionAttribute(string description)
			: base()
		{
			if (description == null)
				throw new ArgumentNullException("description");

			this.Description = description;
		}
	}
}
