using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Commands
{
	[AttributeUsage(AttributeTargets.Field)]
	public class OptionNameAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public OptionNameAttribute(string name)
			: base()
		{
			if (name == null)
				throw new ArgumentNullException("name");

			this.Name = name;
		}
	}
}
